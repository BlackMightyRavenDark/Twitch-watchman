using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Twitch_watchman.TwitchApi;

namespace Twitch_watchman
{
    public static class Utils
    {
        public const string FILENAME_FORMAT_DEFAULT = 
            "ffmpeg_twitch_<channelName>_<year>-<month>-<day>_<hour>h<minute>m<second>s<millisecond>z";
        public const int MAX_LOG_COUNT = 1000;

        public static List<string> channelNames = new List<string>();
        public static MainConfiguration config;

        public static string FormatFileName(string fmt, StreamItem streamItem)
        {
            DateTime now = DateTime.Now;
            string t = streamItem != null ?
                fmt.Replace("<channelName>", streamItem.ChannelName)
                    .Replace("<year>", now.Year.ToString())
                    .Replace("<month>", LeadZero(now.Month))
                    .Replace("<day>", LeadZero(now.Day))
                    .Replace("<hour>", LeadZero(now.Hour))
                    .Replace("<minute>", LeadZero(now.Minute))
                    .Replace("<second>", LeadZero(now.Second))
                    .Replace("<millisecond>", now.Millisecond.ToString().PadLeft(4, '0'))
                : now.ToString();
            return t;
        }

        public static string LeadZero(int n)
        {
            return n < 10 ? $"0{n}" : n.ToString();
        }

        public static void LaunchFfmpeg(StreamItem streamItem)
        {
            string trimmedFilePath = streamItem.DumpingFilePath.Substring(0, streamItem.DumpingFilePath.LastIndexOf("."));
            for (int rec = 1; rec <= streamItem.CopiesCount; ++rec)
            {
                string destFilePath = rec == 1 ? streamItem.DumpingFilePath : $"{trimmedFilePath}_{rec}.ts";
                LaunchFfmpeg(streamItem.PlaylistUrl, destFilePath);
            }
        }

        public static bool LaunchFfmpeg(string url, string dumpingFilePath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string dir = Path.GetDirectoryName(config.FfmpegPath);
            if (!string.IsNullOrEmpty(dir) && !string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
            {
                process.StartInfo.WorkingDirectory = dir;
            }
            string ffmpegName = Path.GetFileName(config.FfmpegPath);
            string cmd = $"/k {ffmpegName} -i {url} -c copy \"{dumpingFilePath}\"";

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = cmd;
            return process.Start();
        }

        public static long GetFileSize(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            return -1L;
        }

        public static int FindStreamItemInListView(StreamItem streamItem, ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; ++i)
            {
                if ((listView.Items[i].Tag as StreamItem) == streamItem)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int DownloadString(string url, out string resultString)
        {
            FileDownloader d = new FileDownloader() { Url = url};
            return d.DownloadString(out resultString);
        }

        public static int HttpsPost(string url, out string responseString)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseString = streamReader.ReadToEnd();
                    return (int)httpResponse.StatusCode;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
                    responseString = ex.Message;
                    return (int)httpWebResponse.StatusCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            responseString = "Client error";
            return 400;
        }

        public static int HttpsPost(string url, string body, out string responseString)
        {
            responseString = "Client error";
            int res = 400;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Client-ID", TWITCH_CLIENT_ID_PRIVATE);
                httpWebRequest.Method = "POST";
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                }
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                try
                {
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        responseString = streamReader.ReadToEnd();
                        res = (int)httpResponse.StatusCode;
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
                        responseString = ex.Message;
                        res = (int)httpWebResponse.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                res = ex.HResult;
            }
            return res;
        }

        public static DateTime JsonStringToDateTime(string t, string fmt = "MM/dd/yyyy HH:mm:ss")
        {
            return DateTime.ParseExact(t, fmt,
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public static string GetNumberedFileName(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrWhiteSpace(filePath) &&
                File.Exists(filePath))
            {
                int n = filePath.LastIndexOf(".");
                string part1 = filePath.Substring(0, n);
                string ext = filePath.Substring(n, filePath.Length - n);
                string newFileName;
                int i = 2;
                do
                {
                    newFileName = $"{part1}_{++i}{ext}";
                } while (File.Exists(newFileName));
                return newFileName;
            }
            return filePath;
        }

        public static string FormatSize(long n)
        {
            const int KB = 1000;
            const int MB = 1000000;
            const int GB = 1000000000;
            const long TB = 1000000000000;
            long b = n % KB;
            long kb = (n % MB) / KB;
            long mb = (n % GB) / MB;
            long gb = (n % TB) / GB;

            if (n >= 0 && n < KB)
                return string.Format("{0} B", b);
            if (n >= KB && n < MB)
                return string.Format("{0},{1:D3} KB", kb, b);
            if (n >= MB && n < GB)
                return string.Format("{0},{1:D3} MB", mb, kb);
            if (n >= GB && n < TB)
                return string.Format("{0},{1:D3},{2:D3} GB", gb, mb, kb);

            return string.Format("{0} {1:D3} {2:D3} {3:D3} bytes", gb, mb, kb, b);
        }

        public static bool SetClipboardText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            bool suc;
            do
            {
                try
                {
                    Clipboard.SetText(text);
                    suc = true;
                }
                catch
                {
                    suc = false;
                }
            } while (!suc);
            return true;
        }

        public static int SelectedIndex(this ListView listView)
        {
            return listView.Items.Count > 0 && listView.SelectedIndices.Count > 0 ? listView.SelectedIndices[0] : -1;
        }

        public static void SetDoubleBuffered(this Control control, bool enabled)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { enabled });
        }
    }

    public sealed class MainConfiguration
    {
        public string SelfDirPath { get; private set; }
        public string FilePath { get; private set; }
        public string DownloadingDirPath { get; set; }
        public string FileNameFormat { get; set; }
        public string FfmpegPath { get; set; }
        public bool SaveStreamInfo { get; set; }
        public int CheckingIntervalInactive { get; set; }
        public int CheckingIntervalActive { get; set; }
        public string StreamListFilePath { get; set; }

        public delegate void SavingDelegate(object sender, JObject root);
        public delegate void LoadingDelegate(object sender, JObject root);
        public SavingDelegate Saving;
        public LoadingDelegate Loading;

        public MainConfiguration()
        {
            SelfDirPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            FilePath = SelfDirPath + "tw_config.json";
            LoadDefaults();
        }

        public void LoadDefaults()
        {
            StreamListFilePath = SelfDirPath + "tw_channelList.json";
            DownloadingDirPath = SelfDirPath;
            FfmpegPath = "FFMPEG.EXE";
            FileNameFormat = Utils.FILENAME_FORMAT_DEFAULT;
            SaveStreamInfo = true;
            CheckingIntervalInactive = 3;
            CheckingIntervalActive = 10;
        }

        public void Load()
        {
            LoadDefaults();
            if (File.Exists(FilePath))
            {
                JObject json = JObject.Parse(File.ReadAllText(FilePath));
                Loading?.Invoke(this, json);
            }
        }

        public void Save()
        {
            JObject json = new JObject();
            Saving?.Invoke(this, json);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            File.WriteAllText(FilePath, json.ToString());
        }
    }

    public sealed class StreamItem
    {
        public string ChannelName { get; private set; }
        public string PlaylistUrl { get; set; }
        public string DumpingFilePath { get; set; } = null;
        public long DumpingFileSize { get; set; } = -1L;
        public bool IsChecking { get; set; } = false;
        public int TimerRemaining { get; set; } = 10;
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int CopiesCount { get; set; } = 1;
        public bool KeepAlive { get; set; } = true;
        public DateTime DateServer { get; set; } = DateTime.MinValue;
        public DateTime DateLocal { get; set; } = DateTime.MinValue;
        public bool IsImportant { get; set; } = false;
        public bool IsStreamActive => !string.IsNullOrEmpty(DumpingFilePath) && !string.IsNullOrEmpty(DumpingFilePath);
        public StreamItem Self => this;

        public StreamItem(string channelName)
        {
            ChannelName = channelName;
        }
    }
}
