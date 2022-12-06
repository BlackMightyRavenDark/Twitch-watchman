using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using static Twitch_watchman.Utils;

namespace Twitch_watchman
{
    public sealed class ThreadChecker
    {
        public StreamItem StreamItem { get; private set; }
        public int LastErrorCode { get; private set; }
        public string LastErrorMessage { get; private set; }
        private long OldFileSize { get; set; } = -1L;
        private bool SaveStreamInfo { get; set; }

        public delegate void CheckingStartedDelegate(object sender);
        public delegate void NewLiveDetectedDelegate(object sender);
        public delegate void TitleDetectedDelegate(object sender, string title);
        public delegate void PlaylistUrlDetectedDelegate(object sender, string playlistUrl);
        public delegate void DumpingStartedDelegate(object sender, int errorCode);
        public delegate void FileSizeChangedDelegate(object sender, long oldSize);
        public delegate void DumpingHaltedDelegate(object sender);
        public delegate void TitleChangedDelegate(object sender);
        public delegate void CompletedDelegate(object sender, int errorCode);
        public delegate void LogAddingDelegate(object sender, string logText);
        public CheckingStartedDelegate CheckingStarted;
        public NewLiveDetectedDelegate NewLiveDetected;
        public TitleDetectedDelegate TitleDetected;
        public PlaylistUrlDetectedDelegate PlaylistUrlDetected;
        public DumpingStartedDelegate DumpingStarted;
        public FileSizeChangedDelegate FileSizeChanged;
        public DumpingHaltedDelegate DumpingHalted;
        public TitleChangedDelegate TitleChanged;
        public CompletedDelegate Completed;
        public LogAddingDelegate LogAdding;

        public ThreadChecker(StreamItem streamItem, bool saveStreamInfo)
        {
            StreamItem = streamItem;
            SaveStreamInfo = saveStreamInfo;
        }

        public void Work(object context)
        {
            SynchronizationContext synchronizationContext = (SynchronizationContext)context;
            try
            {
                synchronizationContext?.Send(OnCheckingStarted_Context, this);
                
                TwitchApi api = new TwitchApi();
                LastErrorCode = api.GetUserInfo_Helix(StreamItem.ChannelName.ToLower(), 
                    out TwitchUserInfo userInfo, out string errorMsg);
                if (LastErrorCode != 200)
                {
                    ResetItem(StreamItem);
                    LastErrorMessage = errorMsg;
                    synchronizationContext?.Send(OnCompleted_Context, this);
                    return;
                }

                LastErrorCode = api.IsUserLive_Helix(userInfo.Id, out string response);
                if (LastErrorCode != 200)
                {
                    ResetItem(StreamItem);
                    LastErrorMessage = response;
                    synchronizationContext?.Send(OnCompleted_Context, this);
                    return;
                }

                JObject jsonLive = JObject.Parse(response);
                JToken jt = jsonLive.Value<JToken>("data");
                if (jt == null)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    synchronizationContext?.Send(OnCompleted_Context, this);
                    return;
                }
                JArray jaData = jt.Value<JArray>();
                if (jaData == null || jaData.Count <= 0)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    synchronizationContext?.Send(OnCompleted_Context, this);
                    return;
                }
                JObject jStreamInfo = jaData[0] as JObject;
                if (jStreamInfo == null)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    synchronizationContext?.Send(OnCompleted_Context, this);
                    return;
                }

                string t = jStreamInfo.Value<string>("started_at");
                DateTime newStreamDate = JsonStringToDateTime(t);
                if (newStreamDate > StreamItem.DateServer)
                {
                    StreamItem.DateServer = newStreamDate;
                    StreamItem.DateLocal = DateTime.Now;
                    string filePath = $"{config.DownloadingDirPath}{FormatFileName(config.FileNameFormat, StreamItem)}.ts";
                    StreamItem.DumpingFilePath = GetNumberedFileName(filePath);
                    StreamItem.DumpingFileSize = 0L;
                    synchronizationContext?.Send(OnNewLiveDetected_Context, this);

                    LastErrorCode = api.GetLiveStreamManifestUrl(userInfo.DisplayName.ToLower(), out string manifestUrl);
                    if (LastErrorCode != 200)
                    {
                        ResetItem(StreamItem);
                        synchronizationContext?.Send(OnCompleted_Context, this);
                        return;
                    }

                    LastErrorCode = DownloadString(manifestUrl, out string playlistUrl);
                    if (LastErrorCode == 200)
                    {
                        TwitchPlaylistsManifestParser parser = new TwitchPlaylistsManifestParser(playlistUrl);
                        bool found = false;
                        for (int i = 0; i < parser.GetCount(); i++)
                        {
                            if (parser.GetGroupId(i).Equals("chunked"))
                            {
                                playlistUrl = parser.GetUrl(i);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            playlistUrl = parser.GetCount() > 0 ? parser.GetUrl(0) : manifestUrl;
                            string s = "Play list marked as \"chunked\" not found!";
                            synchronizationContext?.Send(OnLogAdding_Context, s);
                        }
                    }
                    else
                    {
                        playlistUrl = manifestUrl;
                    }
                    StreamItem.PlaylistUrl = playlistUrl;
                    synchronizationContext?.Send(OnPlaylistUrlDetected_Context, this);

                    if (StreamItem.CopiesCount > 0)
                    {
                        LastErrorCode = FileDownloader.GetContentLength(playlistUrl, out _); //checking URL availability
                        if (LastErrorCode == 200)
                        {
                            LaunchFfmpeg(StreamItem);
                        }
                        else
                        {
                            ResetItem(StreamItem);
                        }
                        synchronizationContext?.Send(OnDumpingStarted_Context, this);
                    }

                    StreamItem.Title = jStreamInfo.Value<string>("title");
                    synchronizationContext?.Send(OnTitleDetected_Context, this);
                    string dateFormatted = StreamItem.DateLocal.ToString("yyyy.MM.dd, HH:mm:ss");
                    string titlesFp = StreamItem.DumpingFilePath + "_titles.txt";
                    File.AppendAllText(titlesFp, $"{dateFormatted}={StreamItem.Title}{Environment.NewLine}");

                    if (SaveStreamInfo)
                    {
                        string infoFp = StreamItem.DumpingFilePath + "_info.json";
                        File.WriteAllText(infoFp, jStreamInfo.ToString());
                    }
                }
                else //стрим уже идёт.
                {
                    //следим за изменением названия стрима.
                    string newTitle = jStreamInfo.Value<JObject>().Value<string>("title");
                    if (StreamItem.Title != newTitle)
                    {
                        StreamItem.Title = newTitle;
                        string dateFormatted = DateTime.Now.ToString("yyyy.MM.dd, HH:mm:ss");
                        if (string.IsNullOrEmpty(StreamItem.DumpingFilePath))
                        {
                            StreamItem.DumpingFilePath =
                                Path.GetFileNameWithoutExtension(FormatFileName(FILENAME_FORMAT_DEFAULT, StreamItem)) +
                                $"_{StreamItem.DateLocal:yyyy-MM-dd, HH-mm-ss_ffff}.ts";
                        }
                        string titlesFp = StreamItem.DumpingFilePath + "_titles.txt";
                        File.AppendAllText(titlesFp, $"{dateFormatted}={newTitle}{Environment.NewLine}");

                        synchronizationContext?.Send(OnTitleChanged_Context, this);
                    }

                    //антизависатор.
                    if (StreamItem.KeepAlive && StreamItem.CopiesCount > 0)
                    {
                        OldFileSize = StreamItem.DumpingFileSize;
                        long fileSize = GetFileSize(StreamItem.DumpingFilePath);
                        if (fileSize > OldFileSize)
                        {
                            StreamItem.DumpingFileSize = fileSize;
                            synchronizationContext?.Send(OnFileSizeChanged_Context, this);
                        }
                        else
                        {
                            ResetItem(StreamItem);
                            synchronizationContext?.Send(OnDumpingHalted_Context, this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                if (ex is System.ComponentModel.InvalidAsynchronousStateException)
                {
                    return;
                }
                LastErrorCode = ex.HResult;
                LastErrorMessage = ex.Message;
            }

            try
            {
                synchronizationContext?.Send(OnCompleted_Context, this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LastErrorCode = ex.HResult;
                LastErrorMessage = ex.Message;
            }
        }

        private void ResetItem(StreamItem streamItem)
        {
            streamItem.DumpingFilePath = null;
            streamItem.DumpingFileSize = -1L;
            streamItem.DateServer = DateTime.MinValue;
            streamItem.DateLocal = DateTime.MinValue;
            streamItem.PlaylistUrl = null;
        }





        private void OnCheckingStarted_Context(object obj)
        {
            CheckingStarted?.Invoke(obj);
        }

        private void OnNewLiveDetected_Context(object obj)
        {
            NewLiveDetected?.Invoke(obj);
        }

        private void OnTitleDetected_Context(object obj)
        {
            TitleDetected?.Invoke(obj, StreamItem.Title);
        }

        private void OnPlaylistUrlDetected_Context(object obj)
        {
            PlaylistUrlDetected?.Invoke(obj, StreamItem.PlaylistUrl);
        }

        private void OnDumpingStarted_Context(object obj)
        {
            DumpingStarted?.Invoke(obj, LastErrorCode);
        }

        private void OnFileSizeChanged_Context(object obj)
        {
            FileSizeChanged?.Invoke(obj, OldFileSize);
        }

        private void OnDumpingHalted_Context(object obj)
        {
            DumpingHalted?.Invoke(obj);
        }

        private void OnTitleChanged_Context(object obj)
        {
            TitleChanged?.Invoke(obj);
        }

        private void OnCompleted_Context(object obj)
        {
            Completed?.Invoke(obj, LastErrorCode);
        }

        private void OnLogAdding_Context(object obj)
        {
            LogAdding?.Invoke(this, (string)obj);
        }
    }
}
