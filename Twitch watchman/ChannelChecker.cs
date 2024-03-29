﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MultiThreadedDownloaderLib;
using HlsDumpLib;
using static HlsDumpLib.HlsDumper;
using static Twitch_watchman.Utils;

namespace Twitch_watchman
{
    public sealed class ChannelChecker
    {
        public StreamItem StreamItem { get; }
        public int LastErrorCode { get; private set; }
        public string LastErrorMessage { get; private set; }
        private bool SaveStreamInfo { get; }

        public const int ERROR_PARSE = -2000;

        public delegate void ChannelCheckingStartedDelegate(object sender);
        public delegate void ChannelCheckingCompletedDelegate(object sender, int errorCode);
        public delegate void NewLiveDetectedDelegate(object sender);
        public delegate void TitleDetectedDelegate(object sender, string title);
        public delegate void PlaylistUrlDetectedDelegate(object sender, string playlistUrl);
        public delegate void DumpingStartedDelegate(object sender, int errorCode);
        public delegate void TitleChangedDelegate(object sender);
        public delegate void LogMessageDelegate(object sender, string message);

        public ChannelChecker(StreamItem streamItem, bool saveStreamInfo)
        {
            StreamItem = streamItem;
            SaveStreamInfo = saveStreamInfo;
        }

        public void Check(
            ChannelCheckingStartedDelegate channelCheckingStarted,
            NewLiveDetectedDelegate newLiveDetected,
            PlaylistUrlDetectedDelegate playlistUrlDetected,
            PlaylistFirstArrivedDelegate playlistFirstArrived,
            OutputStreamAssignedDelegate outputStreamAssigned,
            OutputStreamClosedDelegate outputStreamClosed,
            DumpingStartedDelegate dumpingStarted,
            DumpProgressDelegate dumpingProgress,
            ChannelCheckingCompletedDelegate channelCheckingCompleted,
            TitleDetectedDelegate titleDetected,
            TitleChangedDelegate titleChanged,
            LogMessageDelegate logMessage,
            PlaylistCheckingDelegate dumperPlaylistChecking,
            PlaylistCheckedDelegate playlistChecked,
            PlaylistCheckingDelayCalculatedDelegate playlistCheckingDelayCalculated,
            NextChunkArrivedDelegate dumperNextChunkArrived,
            ChunkDownloadFailedDelegate dumperChunkDownloadFailed,
            ChunkAppendFailedDelegate dumperChunkAppendFailed,
            DumpMessageDelegate dumperDumpMessage,
            DumpWarningDelegate dumperDumpWarning,
            DumpErrorDelegate dumperDumpError,
            DumpFinishedDelegate dumperDumpFinished,
            UpdateErrorsDelegate updateErrors,
            int playlistCheckingIntervalMilliseconds,
            bool saveChunksInfo,
            int maxPlaylistErrorCountInRow,
            int maxOtherErrorsInRow)
        {
            string downloadingDirPath = config.DownloadingDirPath;
            string fileNameFormat = config.FileNameFormat;

            Task.Run(() =>
            {
                channelCheckingStarted?.Invoke(this);
                TwitchApi api = new TwitchApi();
                LastErrorCode = api.GetUserInfo_Helix(StreamItem.ChannelName.ToLower(),
                    out TwitchUserInfo userInfo, out string errorMsg);
                if (LastErrorCode != 200)
                {
                    ResetItem(StreamItem);
                    LastErrorMessage = errorMsg;
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                LastErrorCode = api.IsUserLive_Helix(userInfo.Id, out string response);
                if (LastErrorCode != 200)
                {
                    ResetItem(StreamItem);
                    LastErrorMessage = response;
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                JObject jsonLive = TryParseJson(response);
                if (jsonLive == null)
                {
                    LastErrorCode = ERROR_PARSE;
                    LastErrorMessage = "Data parse error!";
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                JToken jt = jsonLive.Value<JToken>("data");
                if (jt == null)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                JArray jaData = jt.Value<JArray>();
                if (jaData == null || jaData.Count <= 0)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                JObject jStreamInfo = jaData[0] as JObject;
                if (jStreamInfo == null)
                {
                    ResetItem(StreamItem);
                    LastErrorCode = TwitchApi.ERROR_USER_NOT_FOUND;
                    LastErrorMessage = response;
                    channelCheckingCompleted?.Invoke(this, LastErrorCode);
                    return;
                }

                string t = jStreamInfo.Value<string>("started_at");
                DateTime newStreamDate = JsonStringToDateTime(t);
                if (newStreamDate > StreamItem.DateServer)
                {
                    StreamItem.DateServer = newStreamDate;
                    StreamItem.DateLocal = DateTime.Now;
                    string filePath = Path.Combine(downloadingDirPath,
                        FormatFileName(fileNameFormat, StreamItem));
                    StreamItem.DumpingFilePath = GetNumberedFileName(filePath);
                    StreamItem.DumpingFileSize = 0L;
                    newLiveDetected?.Invoke(this);

                    LastErrorCode = api.GetLiveStreamManifestUrl(userInfo.DisplayName.ToLower(), out string manifestUrl);
                    if (LastErrorCode != 200)
                    {
                        ResetItem(StreamItem);
                        channelCheckingCompleted?.Invoke(this, LastErrorCode);
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
                            logMessage?.Invoke(this, s);
                        }
                    }
                    else
                    {
                        playlistUrl = manifestUrl;
                    }
                    StreamItem.PlaylistUrl = playlistUrl;
                    playlistUrlDetected?.Invoke(this, playlistUrl);

                    LastErrorCode = FileDownloader.GetUrlResponseHeaders(playlistUrl, null, out _, out _);
                    if (LastErrorCode == 200)
                    {
                        if (StreamItem.Dumper == null)
                        {
                            dumpingStarted?.Invoke(this, 200);
                            StreamItem.Dumper = new HlsDumper(playlistUrl);
                            StreamItem.Dumper.Dump(StreamItem.DumpingFilePath,
                                (s, url) => { dumperPlaylistChecking?.Invoke(this, url); },
                                (s, chunkCount, newChunkCount, firstChunkId, firstNewChunkId,
                                playlistContent, errorCode, playlistErrorCountInRow) =>
                                    {
                                        playlistChecked?.Invoke(this, chunkCount, newChunkCount, firstChunkId,
                                            firstNewChunkId, playlistContent, errorCode, playlistErrorCountInRow);
                                    },
                                (s, chunkCount, firstChunkId) => { playlistFirstArrived?.Invoke(this, chunkCount, firstChunkId); },
                                (s, stream, fn) => { outputStreamAssigned?.Invoke(this, stream, fn); },
                                (s, fn) => { outputStreamClosed?.Invoke(this, fn); },
                                (s, delay, checkingInterval, cycleProcessingTime) =>
                                    { playlistCheckingDelayCalculated?.Invoke(this, delay, checkingInterval, cycleProcessingTime); },
                                (s, chunk, chunkSize, sessionChunkId, chunkProcessingTime) =>
                                    { dumperNextChunkArrived?.Invoke(this, chunk, chunkSize, sessionChunkId, chunkProcessingTime); },
                                (s, playlistErrorCountInRow, playlistErrorCountInRowMax,
                                otherErrorCountInRow, otherErrorCountInRowMax,
                                chunkDownloadErrorCount, chunkAppendErrorCount, lostChunkCount) =>
                                {
                                    updateErrors?.Invoke(this, playlistErrorCountInRow, playlistErrorCountInRowMax,
                                        otherErrorCountInRow, otherErrorCountInRowMax,
                                        chunkDownloadErrorCount, chunkAppendErrorCount, lostChunkCount);
                                },
                                (s, fs, code) =>
                                {
                                    StreamItem.DumpingFileSize = fs;
                                    dumpingProgress?.Invoke(this, fs, code);
                                },
                                (s, code, count) => { dumperChunkDownloadFailed?.Invoke(this, code, count); },
                                (s, count) => { dumperChunkAppendFailed?.Invoke(this, count); },
                                (s, msg) => { dumperDumpMessage?.Invoke(this, msg); },
                                (s, msg, count) => { dumperDumpWarning?.Invoke(this, msg, count); },
                                (s, msg, count) => { dumperDumpError?.Invoke(this, msg, count); },
                                (s, code) =>
                                {
                                    dumperDumpFinished?.Invoke(this, code);
                                    StreamItem.Dumper = null;
                                },
                                playlistCheckingIntervalMilliseconds,
                                saveChunksInfo, maxPlaylistErrorCountInRow, maxOtherErrorsInRow);
                        }

                        StreamItem.Title = jStreamInfo.Value<string>("title");
                        titleDetected?.Invoke(this, StreamItem.Title);

                        string dateFormatted = StreamItem.DateLocal.ToString("yyyy.MM.dd, HH:mm:ss");
                        string titlesFp = StreamItem.DumpingFilePath + "_titles.txt";
                        File.AppendAllText(titlesFp, $"{dateFormatted}={StreamItem.Title}{Environment.NewLine}");

                        if (SaveStreamInfo)
                        {
                            string infoFp = StreamItem.DumpingFilePath + "_info.json";
                            File.WriteAllText(infoFp, jStreamInfo.ToString());
                        }
                    }
                    else
                    {
                        dumpingStarted?.Invoke(this, LastErrorCode);
                    }
                }
                else //стрим уже идёт.
                {
                    //следим за изменением названия стрима.
                    string newTitle = jStreamInfo.Value<string>("title");
                    if (StreamItem.Title != newTitle)
                    {
                        StreamItem.Title = newTitle;
                        titleChanged?.Invoke(this);

                        string dateFormatted = DateTime.Now.ToString("yyyy.MM.dd, HH:mm:ss");
                        if (string.IsNullOrEmpty(StreamItem.DumpingFilePath))
                        {
                            string fn = FormatFileName(FILENAME_FORMAT_DEFAULT, StreamItem);
                            string dateFileName = StreamItem.DateLocal.ToString("yyyy-MM-dd, HH-mm-ss_ffff");
                            string fullFilePath = Path.Combine(downloadingDirPath, $"{fn}_{dateFileName}");
                            StreamItem.DumpingFilePath = GetNumberedFileName(fullFilePath);
                        }
                        string titlesFp = StreamItem.DumpingFilePath + "_titles.txt";
                        File.AppendAllText(titlesFp, $"{dateFormatted}={newTitle}{Environment.NewLine}");
                    }
                }

                channelCheckingCompleted?.Invoke(this, LastErrorCode);
            });
        }

        private void ResetItem(StreamItem streamItem)
        {
            streamItem.DumpingFilePath = null;
            streamItem.DumpingFileSize = -1L;
            streamItem.DateServer = DateTime.MinValue;
            streamItem.DateLocal = DateTime.MinValue;
            streamItem.PlaylistUrl = null;
            streamItem.Dumper = null;
        }
    }
}
