using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using MultiThreadedDownloaderLib;

namespace Twitch_watchman
{
    public static class Utils
    {
        public const string FILENAME_FORMAT_DEFAULT =
            "hlsdump_twitch_<channelName>_<year>-<month>-<day>_<hour>h<minute>m<second>s<millisecond>ms";
        public const int MAX_LOG_COUNT = 1000;

        public static List<string> channelNames = new List<string>();
        internal static Configurator config;

        public const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/112.0";

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
                    .Replace("<millisecond>", now.Millisecond.ToString().PadLeft(3, '0'))
                : now.ToString();
            return t;
        }

        public static string LeadZero(int n)
        {
            return n < 10 ? $"0{n}" : n.ToString();
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
            FileDownloader d = new FileDownloader() { Url = url };
            return d.DownloadString(out resultString);
        }

        public static int HttpsPost(string url, out string responseString)
        {
            return HttpsPost(url, null, null, out responseString);
        }

        public static int HttpsPost(string url, string body, NameValueCollection headers, out string responseString)
        {
            try
            {
                using (HttpRequestResult requestResult = HttpRequestSender.Send("POST", url, body, headers))
                {
                    if (requestResult.ErrorCode == 200)
                    {
                        if (requestResult.WebContent == null)
                        {
                            responseString = null;
                            return 404;
                        }
                        return requestResult.WebContent.ContentToString(out responseString);
                    }

                    responseString = requestResult.ErrorMessage;
                    return requestResult.ErrorCode;
                }
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                responseString = null;
                return 400;
            }
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

        public static JObject TryParseJson(string jsonString)
        {
            try
            {
                return JObject.Parse(jsonString);
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
