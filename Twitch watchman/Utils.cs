﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using HlsDumpLib;

namespace Twitch_watchman
{
    public static class Utils
    {
        public const string FILENAME_FORMAT_DEFAULT = 
            "hlsdump_twitch_<channelName>_<year>-<month>-<day>_<hour>h<minute>m<second>s<millisecond>ms";
        public const int MAX_LOG_COUNT = 1000;

        public static List<string> channelNames = new List<string>();
        public static MainConfiguration config;

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

        public static int HttpsPost(string url, string body, NameValueCollection headers, out string responseString)
        {
            responseString = "Client error";
            int res = 400;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(body))
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(body);
                    httpWebRequest.ContentLength = buffer.Length;
                }
                else
                {
                    httpWebRequest.ContentLength = 0;
                }

                if (headers != null)
                {
                    SetRequestHeaders(httpWebRequest, headers);
                }

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

        public static void SetRequestHeaders(HttpWebRequest request, NameValueCollection headers)
        {
            request.Headers.Clear();
            for (int i = 0; i < headers.Count; ++i)
            {
                string headerName = headers.GetKey(i);
                string headerValue = headers.Get(i);
                string headerNameLowercased = headerName.ToLower();

                //TODO: Complete headers support.
                if (headerNameLowercased.Equals("accept"))
                {
                    request.Accept = headerValue;
                    continue;
                }
                else if (headerNameLowercased.Equals("user-agent"))
                {
                    request.UserAgent = headerValue;
                    continue;
                }
                else if (headerNameLowercased.Equals("referer"))
                {
                    request.Referer = headerValue;
                    continue;
                }
                else if (headerNameLowercased.Equals("host"))
                {
                    request.Host = headerValue;
                    continue;
                }
                else if (headerNameLowercased.Equals("content-type"))
                {
                    request.ContentType = headerValue;
                    continue;
                }
                else if (headerNameLowercased.Equals("content-length"))
                {
                    if (long.TryParse(headerValue, out long length))
                    {
                        request.ContentLength = length;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Can't parse value of \"Content-Length\" header!");
                    }
                    continue;
                }
                else if (headerNameLowercased.Equals("connection"))
                {
                    System.Diagnostics.Debug.WriteLine("The \"Connection\" header is not supported yet.");
                    continue;
                }
                else if (headerNameLowercased.Equals("range"))
                {
                    continue;
                }
                else if (headerNameLowercased.Equals("if-modified-since"))
                {
                    System.Diagnostics.Debug.WriteLine("The \"If-Modified-Since\" header is not supported yet.");
                    continue;
                }
                else if (headerNameLowercased.Equals("transfer-encoding"))
                {
                    System.Diagnostics.Debug.WriteLine("The \"Transfer-Encoding\" header is not supported yet.");
                    continue;
                }

                request.Headers.Add(headerName, headerValue);
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
    }

    public sealed class MainConfiguration
    {
        public string SelfDirPath { get; private set; }
        public string FilePath { get; private set; }
        public string DownloadingDirPath { get; set; }
        public string FileNameFormat { get; set; }
        public string ChannelListFilePath { get; set; }
        public bool SaveStreamInfo { get; set; }
        public bool SaveChunksInfo { get; set; }
        public bool StopIfPlaylistLost { get; set; }
        public int CheckingIntervalInactive { get; set; }
        public int CheckingIntervalActive { get; set; }

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
            ChannelListFilePath = SelfDirPath + "tw_channelList.json";
            DownloadingDirPath = SelfDirPath;
            FileNameFormat = Utils.FILENAME_FORMAT_DEFAULT;
            SaveStreamInfo = true;
            SaveChunksInfo = true;
            StopIfPlaylistLost = true;
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
        public DateTime DateServer { get; set; } = DateTime.MinValue;
        public DateTime DateLocal { get; set; } = DateTime.MinValue;
        public bool IsImportant { get; set; } = false;
        public bool IsStreamActive => Dumper != null;
        public HlsDumper Dumper { get; set; }

        public StreamItem(string channelName)
        {
            ChannelName = channelName;
        }
    }
}
