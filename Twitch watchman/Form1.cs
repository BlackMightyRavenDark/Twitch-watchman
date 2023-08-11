using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using HlsDumpLib;
using static Twitch_watchman.Utils;

namespace Twitch_watchman
{
    public partial class Form1 : Form
    {
        private const int COLUMN_ID_CHANNEL_NAME = 0;
        private const int COLUMN_ID_TIMER = 1;
        private const int COLUMN_ID_FILEPATH = 2;
        private const int COLUMN_ID_FILESIZE = 3;
        private const int COLUMN_ID_DELAY = 4;
        private const int COLUMN_ID_CHUNKPROCESSINGTIME = 5;
        private const int COLUMN_ID_NEWCHUNKS = 6;
        private const int COLUMN_ID_CHUNKID = 7;
        private const int COLUMN_ID_CHUNKLENGTH = 8;
        private const int COLUMN_ID_CHUNKSIZE = 9;
        private const int COLUMN_ID_CHUNKFILENAME = 10;
        private const int COLUMN_ID_CHUNKURL = 11;
        private const int COLUMN_ID_PROCESSEDCHUNKS = 12;
        private const int COLUMN_ID_FIRSTCHUNKID = 13;
        private const int COLUMN_ID_LOSTCHUNKS = 14;
        private const int COLUMN_ID_PLAYLISTERRORS = 15;
        private const int COLUMN_ID_CHUNKDOWNLOADERRORS = 16;
        private const int COLUMN_ID_CHUNKAPPENDERRORS = 17;
        private const int COLUMN_ID_OTHERERRORS = 18;
        private const int COLUMN_ID_DUMPSTARTDATE = 19;
        private const int COLUMN_ID_STATUS = 20;
        private const int COLUMN_ID_PLAYLIST_URL = 21;

        private bool _isClosing = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 100;

            config = new MainConfiguration();
            config.Saving += (s, json) =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    json["left"] = Left;
                    json["top"] = Top;
                    json["width"] = Width;
                    json["height"] = Height;
                }
                json["downloadingDirPath"] = config.DownloadingDirPath;
                json["fileNameFormat"] = config.FileNameFormat;
                json["checkingIntervalInactive"] = config.CheckingIntervalInactive;
                json["checkingIntervalActive"] = config.CheckingIntervalActive;
                json["saveStreamInfo"] = config.SaveStreamInfo;
                json["saveChunksInfo"] = config.SaveChunksInfo;
                json["stopIfPlaylistLost"] = config.StopIfPlaylistLost;

                JArray jaColumns = new JArray();
                foreach (ColumnHeader column in listViewStreams.Columns)
                {
                    JObject jColumn = new JObject();
                    jColumn["displayIndex"] = column.DisplayIndex;
                    jColumn["width"] = column.Width;
                    jaColumns.Add(jColumn);
                }

                json.Add(new JProperty("columns", jaColumns));
            };
            config.Loading += (s, json) =>
            {
                JToken jt = json.Value<JToken>("left");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("top");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }
                jt = json.Value<JToken>("width");
                if (jt != null)
                {
                    Width = jt.Value<int>();
                }
                jt = json.Value<JToken>("height");
                if (jt != null)
                {
                    Height = jt.Value<int>();
                }

                jt = json.Value<JToken>("downloadingDirPath");
                if (jt != null)
                {
                    config.DownloadingDirPath = jt.Value<string>();
                }
                textBoxDownloadingDir.Text = config.DownloadingDirPath;

                jt = json.Value<JToken>("fileNameFormat");
                if (jt != null)
                {
                    config.FileNameFormat = jt.Value<string>();
                    if (string.IsNullOrEmpty(config.FileNameFormat))
                    {
                        config.FileNameFormat = FILENAME_FORMAT_DEFAULT;
                    }
                }

                jt = json.Value<JToken>("checkingIntervalInactive");
                if (jt != null)
                {
                    config.CheckingIntervalInactive = jt.Value<int>();
                    if (config.CheckingIntervalInactive < (int)numericUpDownTimerIntervalInactive.Minimum)
                    {
                        config.CheckingIntervalInactive = (int)numericUpDownTimerIntervalInactive.Minimum;
                    }
                    else if (config.CheckingIntervalInactive >= (int)numericUpDownTimerIntervalInactive.Maximum)
                    {
                        config.CheckingIntervalInactive = (int)numericUpDownTimerIntervalInactive.Maximum;
                    }
                }

                jt = json.Value<JToken>("checkingIntervalActive");
                if (jt != null)
                {
                    config.CheckingIntervalActive = jt.Value<int>();
                    if (config.CheckingIntervalActive < (int)numericUpDownTimerIntervalActive.Minimum)
                    {
                        config.CheckingIntervalActive = (int)numericUpDownTimerIntervalActive.Minimum;
                    }
                    else if (config.CheckingIntervalActive >= (int)numericUpDownTimerIntervalActive.Maximum)
                    {
                        config.CheckingIntervalActive = (int)numericUpDownTimerIntervalActive.Maximum;
                    }
                }

                jt = json.Value<JToken>("saveStreamInfo");
                if (jt != null)
                {
                    config.SaveStreamInfo = jt.Value<bool>();
                }

                jt = json.Value<JToken>("saveChunksInfo");
                if (jt != null)
                {
                    config.SaveChunksInfo = jt.Value<bool>();
                }

                jt = json.Value<JToken>("stopIfPlaylistLost");
                if (jt != null)
                {
                    config.StopIfPlaylistLost = jt.Value<bool>();
                }

                JArray jaColumns = json.Value<JArray>("columns");
                if (jaColumns != null && jaColumns.Count > 0)
                {
                    for (int i = 0; i < jaColumns.Count; ++i)
                    {
                        JObject j = jaColumns[i] as JObject;
                        int displayIndex = j.Value<int>("displayIndex");
                        if (displayIndex < 0) { displayIndex = 0; }
                        int columnWidth = j.Value<int>("width");
                        if (columnWidth < 50) { columnWidth = 50; }

                        listViewStreams.Columns[i].DisplayIndex = displayIndex;
                        listViewStreams.Columns[i].Width = columnWidth;
                    }

                    if (columnHeaderChannelName.DisplayIndex != 0)
                    {
                        columnHeaderChannelName.DisplayIndex = 0;
                    }
                }
            };
            config.Load();
            textBoxFileNameFormat.Text = config.FileNameFormat;
            numericUpDownTimerIntervalInactive.Value = config.CheckingIntervalInactive;
            numericUpDownTimerIntervalActive.Value = config.CheckingIntervalActive;
            checkBoxSaveStreamInfo.Checked = config.SaveStreamInfo;
            checkBoxSaveChunksInfo.Checked = config.SaveChunksInfo;
            checkBoxStopIfPlaylistLost.Checked = config.StopIfPlaylistLost;

            if (File.Exists(config.ChannelListFilePath))
            {
                LoadChannelList(config.ChannelListFilePath);
            }

            string[] args = Environment.GetCommandLineArgs();
            foreach (string s in args)
            {
                if (s.ToLower().Equals("/enabled"))
                {
                    checkBoxTimerEnabled.Checked = true;
                }
                else if (s.ToLower().Equals("/check"))
                {
                    CheckAllItems();
                }
            }

            listViewStreams.SetDoubleBuffered(true);

            AddToLog(null, "Программа запущена");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                timerCheck.Enabled = false;
                checkBoxTimerEnabled.Enabled = false;
                if (IsUnfinishedTaskPresent())
                {
                    e.Cancel = true;
                    if (!_isClosing)
                    {
                        _isClosing = true;
                        StopAll();
                        bool unfinished = true;
                        Task.Run(() =>
                        {
                            while (unfinished)
                            {
                                Thread.Sleep(200);
                                Invoke(new MethodInvoker(() => unfinished = IsUnfinishedTaskPresent()));
                            }

                            Invoke(new MethodInvoker(() => Close()));
                        });
                    }
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                config.Save();
                SaveChannelList(config.ChannelListFilePath);
            }
        }

        private void SaveChannelList(string filePath)
        {
            JArray jArray = new JArray();
            foreach (ListViewItem item in listViewStreams.Items)
            {
                StreamItem streamItem = item.Tag as StreamItem;
                JObject jItem = new JObject();
                jItem["channelName"] = streamItem.ChannelName;
                jItem["important"] = streamItem.IsImportant;
                jArray.Add(jItem);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, jArray.ToString());
        }

        private void LoadChannelList(string filePath)
        {
            JArray jArray = JArray.Parse(File.ReadAllText(filePath));
            foreach (JObject j in jArray)
            {
                string channelName = j.Value<string>("channelName");
                StreamItem streamItem = new StreamItem(channelName);
                streamItem.IsImportant = j.Value<bool>("important");
                streamItem.TimerRemaining = config.CheckingIntervalInactive;

                AddStream(streamItem);
            }
        }

        private void listViewStreams_KeyDown(object sender, KeyEventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        CheckItem(id);
                        break;

                    case Keys.Delete:
                        if (MessageBox.Show("Удалить выделенный элемент?", "Удаление",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            listViewStreams.Items.RemoveAt(id);
                        }
                        break;
                }
            }
        }

        private void listViewStreams_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listViewStreams.SelectedIndices.Count > 0)
            {
                int id = listViewStreams.SelectedIndices[0];
                CheckItem(id);
            }
        }

        private void btnBrowseDownloadingDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Выберите папку для скачивания";
            folderBrowserDialog.SelectedPath =
                (!config.DownloadingDirPath.Equals(string.Empty) && Directory.Exists(config.DownloadingDirPath)) ?
                config.DownloadingDirPath : config.SelfDirPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                config.DownloadingDirPath =
                    folderBrowserDialog.SelectedPath.EndsWith("\\")
                    ? folderBrowserDialog.SelectedPath : folderBrowserDialog.SelectedPath + "\\";
                textBoxDownloadingDir.Text = config.DownloadingDirPath;
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            CheckAllItems();
        }

        private void textBoxDownloadingDir_Leave(object sender, EventArgs e)
        {
            config.DownloadingDirPath = textBoxDownloadingDir.Text;
        }

        private void textBoxFileNameFormat_Leave(object sender, EventArgs e)
        {
            config.FileNameFormat = textBoxFileNameFormat.Text;
        }

        private void AddStream(StreamItem streamItem)
        {
            channelNames.Add(streamItem.ChannelName.ToLower());
            ListViewItem item = new ListViewItem(streamItem.ChannelName);
            item.SubItems.Add(timerCheck.Enabled ? streamItem.TimerRemaining.ToString() : "Отключен!");
            item.SubItems.Add(streamItem.DumpingFilePath);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add("Остановлено");
            item.SubItems.Add(string.Empty);
            item.Tag = streamItem;
            listViewStreams.Items.Add(item);
        }

        private void ResetItem(StreamItem streamItem)
        {
            streamItem.DumpingFilePath = null;
            streamItem.DumpingFileSize = -1L;
            streamItem.DateServer = DateTime.MinValue;
            streamItem.DateLocal = DateTime.MinValue;
            streamItem.TimerRemaining = config.CheckingIntervalInactive;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text =
                    timerCheck.Enabled ? streamItem.TimerRemaining.ToString() : "Отключен!";
                listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_DELAY].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKPROCESSINGTIME].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_NEWCHUNKS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKID].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKLENGTH].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKSIZE].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKURL].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKFILENAME].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_PROCESSEDCHUNKS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_FIRSTCHUNKID].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_LOSTCHUNKS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLISTERRORS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKDOWNLOADERRORS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKAPPENDERRORS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_OTHERERRORS].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_DUMPSTARTDATE].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_STATUS].Text = "Остановлено";
                listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLIST_URL].Text = null;
            }
        }

        private void CheckItem(StreamItem streamItem, int itemIndex)
        {
            if (!streamItem.IsChecking)
            {
                streamItem.IsChecking = true;
                listViewStreams.Items[itemIndex].SubItems[COLUMN_ID_TIMER].Text = "Запуск проверки...";

                const int playlistCheckingIntervalMilliseconds = 2000;
                bool saveChunksInfo = config.SaveChunksInfo;
                int maxPlaylistErrorCountInRow = config.StopIfPlaylistLost ? 1 : 5;
                const int otherErrorCountInRow = 5;

                Task.Run(() =>
                {
                    ChannelChecker checker = new ChannelChecker(streamItem, config.SaveStreamInfo);
                    checker.Check(OnChannelCheckingStarted, OnNewLiveDetected,
                        OnPlaylistUrlDetected, OnPlaylistFirstArrived, null, null,
                        OnDumpingStarted, OnDumpingProgress, OnChannelCheckingCompleted,
                        OnTitleDetected, OnTitleChanged, OnLogMessage,
                        OnPlaylistCheckingStarted, OnPlaylistCheckingCompleted,
                        OnPlaylistCheckingDelayCalculated, OnNextChunkArrived, null,
                        null, null, null, null, OnDumpingFinished, OnUpdateErrors,
                        playlistCheckingIntervalMilliseconds, saveChunksInfo,
                        maxPlaylistErrorCountInRow, otherErrorCountInRow);
                });
            }
        }

        private void CheckItem(int itemIndex)
        {
            StreamItem streamItem = listViewStreams.Items[itemIndex].Tag as StreamItem;
            CheckItem(streamItem, itemIndex);
        }

        private void CheckAllItems()
        {
            for (int i = 0; i < listViewStreams.Items.Count; ++i)
            {
                CheckItem(i);
            }
        }

        private void timerCheck_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewStreams.Items.Count; ++i)
            {
                StreamItem streamItem = listViewStreams.Items[i].Tag as StreamItem;
                if (!streamItem.IsChecking)
                {
                    streamItem.TimerRemaining--;
                    listViewStreams.Items[i].SubItems[COLUMN_ID_TIMER].Text = streamItem.TimerRemaining.ToString();
                    if (streamItem.TimerRemaining <= 0)
                    {
                        CheckItem(streamItem, i);
                    }
                }
            }
        }

        private void OnChannelCheckingStarted(object sender)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnChannelCheckingStarted(sender); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text = "Проверка...";
                }
            }
        }

        private void OnPlaylistFirstArrived(object sender, int chunkCount, int firstChunkId)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnPlaylistFirstArrived(sender, chunkCount, firstChunkId); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FIRSTCHUNKID].Text = firstChunkId.ToString();
                }
            }
        }

        private void OnNewLiveDetected(object sender)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnNewLiveDetected(sender); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                notifyIcon1.BalloonTipTitle = "СТРИИИИИИМ!!!!!!!";
                notifyIcon1.BalloonTipText = $"Канал {streamItem.ChannelName} начал трансляцию!";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(5000);

                AddToLog(streamItem.ChannelName, "Новая трансляция!");
            }
        }

        private void OnTitleDetected(object sender, string title)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnTitleDetected(sender, title); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                AddToLog(streamItem.ChannelName, $"Название стрима: {title}");
            }
        }

        private void OnPlaylistUrlDetected(object sender, string playlistUrl)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnPlaylistUrlDetected(sender, playlistUrl); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLIST_URL].Text = playlistUrl;
                }
            }
        }

        public void OnPlaylistCheckingStarted(object sender, string playlistUrl)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnPlaylistCheckingStarted(sender, playlistUrl); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_STATUS].Text = "Плейлист проверяется...";
                }
            }
        }

        public void OnPlaylistCheckingCompleted(object sender,
            int chunkCount, int newChunkCount, int firstChunkId, int firstNewChunkId,
            string playlistContent, int errorCode, int playlistErrorCountInRow)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    OnPlaylistCheckingCompleted(sender, chunkCount, newChunkCount,
                        firstChunkId, firstNewChunkId, playlistContent,
                        errorCode, playlistErrorCountInRow);
                });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_NEWCHUNKS].Text =
                        $"{newChunkCount} / {chunkCount}";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLISTERRORS].Text =
                        $"{playlistErrorCountInRow} / {streamItem.Dumper.PlaylistErrorCountInRowMax}";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_STATUS].Text =
                        $"Плейлист проверен (код: {errorCode})";

                    if (newChunkCount <= 0)
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKPROCESSINGTIME].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKID].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKLENGTH].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKSIZE].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKFILENAME].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKURL].Text = null;
                    }
                }
            }
        }

        public void OnPlaylistCheckingDelayCalculated(object sender,
            int delay, int checkingInterval, int cycleProcessingTime)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    OnPlaylistCheckingDelayCalculated(sender,
                        delay, checkingInterval, cycleProcessingTime);
                });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_DELAY].Text =
                        $"{delay}ms / {checkingInterval}ms";
                }
            }
        }

        private void OnTitleChanged(object sender)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnTitleChanged(sender); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;

                if (!_isClosing)
                {
                    notifyIcon1.BalloonTipTitle = streamItem.ChannelName;
                    notifyIcon1.BalloonTipText = $"Изменилось название: {streamItem.Title}";
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.ShowBalloonTip(5000);
                }

                AddToLog(streamItem.ChannelName, $"Новое название стрима: {streamItem.Title}");
            }
        }

        private void OnDumpingStarted(object sender, int errorCode)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnDumpingStarted(sender, errorCode); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    if (errorCode == 200)
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text = streamItem.DumpingFilePath;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = "0";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_NEWCHUNKS].Text = null;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_PROCESSEDCHUNKS].Text = "0";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_LOSTCHUNKS].Text = "0";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_DUMPSTARTDATE].Text = streamItem.DateLocal.ToString();
                    }
                    else
                    {
                        notifyIcon1.BalloonTipTitle = streamItem.ChannelName;
                        notifyIcon1.BalloonTipText = "Ошибка доступа к плейлисту!";
                        notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                        notifyIcon1.ShowBalloonTip(5000);

                        AddToLog(streamItem.ChannelName, $"Ошибка доступа к плейлисту! Код ошибки: {errorCode}");

                        ResetItem(streamItem);
                    }
                }
            }
        }

        private void OnDumpingProgress(object sender, long fileSize, int errorCode)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnDumpingProgress(sender, fileSize, errorCode); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = FormatSize(fileSize);
                    listViewStreams.Items[id].SubItems[COLUMN_ID_STATUS].Text = "Дампинг...";
                }
            }
        }

        public void OnNextChunkArrived(object sender, StreamSegment chunk,
            long chunkSize, int sessionChunkId, int chunkProcessingTime)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    OnNextChunkArrived(sender, chunk, chunkSize,
                        sessionChunkId, chunkProcessingTime);
                });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKPROCESSINGTIME].Text =
                        $"{chunkProcessingTime}ms";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKSIZE].Text =
                        FormatSize(chunkSize);
                    listViewStreams.Items[id].SubItems[COLUMN_ID_PROCESSEDCHUNKS].Text =
                        (sessionChunkId + 1).ToString();
                    if (chunk != null)
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKID].Text =
                            chunk.Id.ToString();
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKLENGTH].Text =
                            $"{chunk.LengthSeconds} сек.";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKFILENAME].Text =
                            chunk.FileName;
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKURL].Text =
                            chunk.Url;
                    }
                    else
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKID].Text = "null";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKLENGTH].Text = "null";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKFILENAME].Text = "null";
                        listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKURL].Text = "null";
                    }
                }
            }
        }

        private void OnDumpingFinished(object sender, int errorCode)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnDumpingFinished(sender, errorCode); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    if (!_isClosing)
                    {
                        notifyIcon1.BalloonTipTitle = streamItem.ChannelName;
                        notifyIcon1.BalloonTipText = "Трансляция завершена!";
                        notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                        notifyIcon1.ShowBalloonTip(5000);
                    }

                    AddToLog(streamItem.ChannelName, errorCode == HlsDumper.DUMPING_ERROR_CANCELED ?
                        "Дампинг остановлен!" :
                        $"Плейлист потерян! Возможно, трансляция завершена! Код ошибки: {errorCode}");

                    ResetItem(streamItem);

                    listViewStreams.Items[id].SubItems[COLUMN_ID_STATUS].Text =
                        errorCode == HlsDumper.DUMPING_ERROR_CANCELED ? "Отменён" : "Завершён";
                    if (streamItem.Dumper != null && streamItem.Dumper.LostChunkCount > 0)
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_LOSTCHUNKS].Text =
                            streamItem.Dumper.LostChunkCount.ToString();
                    }
                }
            }
        }

        private void OnChannelCheckingCompleted(object sender, int errorCode)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnChannelCheckingCompleted(sender, errorCode); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    if (errorCode == 200)
                    {
                        streamItem.TimerRemaining = config.CheckingIntervalActive;
                    }
                    else
                    {
                        ResetItem(streamItem);
                        listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text =
                            errorCode != TwitchApi.ERROR_USER_OFFLINE ?
                            $"Ошибка! {TwitchApi.ErrorCodeToString(errorCode)}, {(sender as ChannelChecker).LastErrorMessage}" : null;
                        streamItem.TimerRemaining = errorCode == TwitchApi.ERROR_USER_NOT_FOUND ?
                            (int)numericUpDownTimerIntervalActive.Maximum : config.CheckingIntervalInactive;
                    }

                    listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text =
                        timerCheck.Enabled ? streamItem.TimerRemaining.ToString() : "Отключен!";
                }

                streamItem.IsChecking = false;
            }
        }

        private void OnLogMessage(object sender, string messageText)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { OnLogMessage(sender, messageText); });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                AddToLog(streamItem.ChannelName, messageText);
            }
        }

        private void OnUpdateErrors(object sender,
            int playlistErrorCountInRow, int playlistErrorCountInRowMax,
            int otherErrorCountInRow, int otherErrorCountInRowMax,
            int chunkDownloadErrorCount, int chunkAppendErrorCount,
            int lostChunkCount)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    OnUpdateErrors(sender, playlistErrorCountInRow, playlistErrorCountInRowMax,
                        otherErrorCountInRow, otherErrorCountInRowMax,
                        chunkDownloadErrorCount, chunkAppendErrorCount, lostChunkCount);
                });
            }
            else
            {
                StreamItem streamItem = (sender as ChannelChecker).StreamItem;
                int id = FindStreamItemInListView(streamItem, listViewStreams);
                if (id >= 0)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLISTERRORS].Text =
                        $"{playlistErrorCountInRow} / {playlistErrorCountInRowMax}";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKDOWNLOADERRORS].Text =
                        chunkDownloadErrorCount.ToString();
                    listViewStreams.Items[id].SubItems[COLUMN_ID_CHUNKAPPENDERRORS].Text =
                        chunkAppendErrorCount.ToString();
                    listViewStreams.Items[id].SubItems[COLUMN_ID_OTHERERRORS].Text =
                        $"{otherErrorCountInRow} / {otherErrorCountInRowMax}";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_LOSTCHUNKS].Text =
                        lostChunkCount.ToString();
                }
            }
        }

        private void StopAll()
        {
            foreach (ListViewItem listViewItem in listViewStreams.Items)
            {
                StreamItem streamItem = listViewItem.Tag as StreamItem;
                streamItem.Dumper?.StopDumping();
            }
        }

        private bool IsUnfinishedTaskPresent()
        {
            foreach (ListViewItem listViewItem in listViewStreams.Items)
            {
                StreamItem streamItem = listViewItem.Tag as StreamItem;
                if (streamItem.IsStreamActive) { return true; }
            }
            return false;
        }

        private void numericUpDownTimerIntervalInactive_ValueChanged(object sender, EventArgs e)
        {
            config.CheckingIntervalInactive = (int)numericUpDownTimerIntervalInactive.Value;
        }

        private void numericUpDownTimerIntervalActive_ValueChanged(object sender, EventArgs e)
        {
            config.CheckingIntervalActive = (int)numericUpDownTimerIntervalActive.Value;
        }

        private void btnSetDefaultFileNameFormat_Click(object sender, EventArgs e)
        {
            config.FileNameFormat = FILENAME_FORMAT_DEFAULT;
            textBoxFileNameFormat.Text = FILENAME_FORMAT_DEFAULT;
        }

        private void btnAddNewStream_Click(object sender, EventArgs e)
        {
            FormItemEditor itemEditor = new FormItemEditor(null);
            if (itemEditor.ShowDialog() == DialogResult.OK)
            {
                itemEditor.StreamItem.TimerRemaining = config.CheckingIntervalInactive;
                AddStream(itemEditor.StreamItem);
            }
        }

        private void AddToLog(string channelName, string eventText)
        {
            string dateTime = DateTime.Now.ToString("yyyy.MM.dd, HH:mm:ss");
            ListViewItem item = new ListViewItem(dateTime);
            item.SubItems.Add(channelName);
            item.SubItems.Add(eventText);

            listViewLog.Items.Add(item);

            if (MAX_LOG_COUNT > 1 && listViewLog.Items.Count > MAX_LOG_COUNT)
            {
                listViewLog.Items.RemoveAt(1);
            }

            listViewLog.EnsureVisible(listViewLog.Items.Count - 1);
        }

        private void listViewStreams_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listViewStreams.SelectedIndices.Count > 0)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void listViewStreams_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (columnHeaderChannelName.DisplayIndex != 0)
            {
                columnHeaderChannelName.DisplayIndex = 0;
            }

            try
            {
                using (StringFormat sf = new StringFormat())
                {
                    switch (e.Header.TextAlign)
                    {
                        case HorizontalAlignment.Center:
                            sf.Alignment = StringAlignment.Center;
                            break;

                        case HorizontalAlignment.Right:
                            sf.Alignment = StringAlignment.Far;
                            break;
                    }
                    sf.LineAlignment = StringAlignment.Center;
                    e.DrawBackground();

                    e.Graphics.DrawString(e.Header.Text, listViewStreams.Font, Brushes.Black, e.Bounds, sf);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private void listViewStreams_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(e.Bounds.Width, e.Bounds.Height))
                {
                    using (Graphics buffer = Graphics.FromImage(bitmap))
                    {
                        using (StringFormat sf = new StringFormat())
                        {
                            sf.FormatFlags = StringFormatFlags.NoWrap;
                            sf.LineAlignment = StringAlignment.Center;
                            switch (e.Header.TextAlign)
                            {
                                case HorizontalAlignment.Center:
                                    sf.Alignment = StringAlignment.Center;
                                    break;
                                case HorizontalAlignment.Right:
                                    sf.Alignment = StringAlignment.Far;
                                    break;
                            }

                            bool selected = listViewStreams.SelectedIndices.Count > 0 && listViewStreams.SelectedIndices[0] == e.ItemIndex;
                            Brush brushBkg = selected ? SystemBrushes.Highlight : SystemBrushes.Control;
                            Rectangle r = new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height);
                            buffer.FillRectangle(brushBkg, r);
                            StreamItem streamItem = listViewStreams.Items[e.ItemIndex].Tag as StreamItem;
                            Brush brushText;
                            if (selected)
                            {
                                brushText = streamItem.IsImportant && (e.ColumnIndex == 0 || e.ColumnIndex == 2) ? Brushes.Yellow : Brushes.White;
                            }
                            else
                            {
                                brushText = streamItem.IsImportant && (e.ColumnIndex == 0 || e.ColumnIndex == 2) ? Brushes.Red : Brushes.Black;
                            }
                            buffer.DrawString(e.SubItem.Text, listViewStreams.Font, brushText, r, sf);
                            e.Graphics.DrawImage(bitmap, e.Bounds.X, e.Bounds.Y);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                miImportantChannelToolStripMenuItem.Checked = streamItem.IsImportant;
            }
        }

        private void miImportantChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                miImportantChannelToolStripMenuItem.Checked = !miImportantChannelToolStripMenuItem.Checked;
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                streamItem.IsImportant = miImportantChannelToolStripMenuItem.Checked;

                listViewStreams.Refresh();
            }
        }

        private void miEditChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                FormItemEditor itemEditor = new FormItemEditor(streamItem);
                if (itemEditor.ShowDialog() == DialogResult.OK)
                {
                    streamItem.TimerRemaining =
                        streamItem.IsStreamActive ? config.CheckingIntervalActive : config.CheckingIntervalInactive;
                    listViewStreams.Refresh();
                }
            }
        }

        private void miCheckChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                CheckItem(id);
            }
        }

        private void miOpenChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                string url = $"https://twitch.tv/{streamItem.ChannelName}/videos";
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = url;
                process.Start();
            }
        }

        private void miCopyPlaylistUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                if (!string.IsNullOrEmpty(streamItem.PlaylistUrl))
                {
                    SetClipboardText(streamItem.PlaylistUrl);
                }
            }
        }

        private void miRemoveChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0 && id < listViewStreams.Items.Count)
            {
                string t = listViewStreams.Items[id].SubItems[0].Text;
                if (MessageBox.Show($"Удалить канал {t}?", "Удалить канал?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    channelNames.Remove(t);
                    listViewStreams.Items.RemoveAt(id);
                    int newId = id < listViewStreams.Items.Count ? id : listViewStreams.Items.Count - 1;
                    listViewStreams.SelectedIndices.Clear();
                    if (newId >= 0 && newId < listViewStreams.Items.Count)
                    {
                        listViewStreams.SelectedIndices.Add(newId);
                    }
                }
            }
        }

        private void miCopyChannelNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0 && id < listViewStreams.Items.Count)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                SetClipboardText(streamItem.ChannelName);
            }
        }

        private void miStopDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                streamItem.Dumper?.StopDumping();
            }
        }

        private void checkBoxTimerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTimerEnabled.Checked)
            {
                for (int i = 0; i < listViewStreams.Items.Count; ++i)
                {
                    StreamItem streamItem = listViewStreams.Items[i].Tag as StreamItem;
                    if (!streamItem.IsChecking)
                    {
                        streamItem.TimerRemaining = streamItem.IsStreamActive ? config.CheckingIntervalActive : config.CheckingIntervalInactive;
                        listViewStreams.Items[i].SubItems[COLUMN_ID_TIMER].Text = streamItem.TimerRemaining.ToString();
                    }
                }

                timerCheck.Enabled = true;
            }
            else
            {
                timerCheck.Enabled = false;
                for (int i = 0; i < listViewStreams.Items.Count; ++i)
                {
                    StreamItem streamItem = listViewStreams.Items[i].Tag as StreamItem;
                    if (!streamItem.IsChecking)
                    {
                        listViewStreams.Items[i].SubItems[COLUMN_ID_TIMER].Text = "Отключен!";
                        streamItem.TimerRemaining = streamItem.IsStreamActive ? config.CheckingIntervalActive : config.CheckingIntervalInactive;
                    }
                }
            }
        }

        private void checkBoxSaveStreamInfo_CheckedChanged(object sender, EventArgs e)
        {
            config.SaveStreamInfo = checkBoxSaveStreamInfo.Checked;
        }

        private void checkBoxSaveChunksInfo_CheckedChanged(object sender, EventArgs e)
        {
            config.SaveChunksInfo = checkBoxSaveChunksInfo.Checked;
        }

        private void checkBoxStopIfPlaylistLost_CheckedChanged(object sender, EventArgs e)
        {
            config.StopIfPlaylistLost = checkBoxStopIfPlaylistLost.Checked;
        }
    }
}
