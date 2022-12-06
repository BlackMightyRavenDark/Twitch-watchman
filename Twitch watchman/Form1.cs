using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Twitch_watchman.Utils;

namespace Twitch_watchman
{
    public partial class Form1 : Form
    {
        private const int COLUMN_ID_CHANNEL_NAME = 0;
        private const int COLUMN_ID_TIMER = 1;
        private const int COLUMN_ID_FILEPATH = 2;
        private const int COLUMN_ID_FILESIZE = 3;
        private const int COLUMN_ID_COPIES_COUNT = 4;
        private const int COLUMN_ID_DATE = 5;
        private const int COLUMN_ID_PLAYLIST_URL = 6;

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
                json["ffmpegPath"] = config.FfmpegPath;
                json["fileNameFormat"] = config.FileNameFormat;
                json["checkingIntervalInactive"] = config.CheckingIntervalInactive;
                json["checkingIntervalActive"] = config.CheckingIntervalActive;
                json["saveStreamInfo"] = config.SaveStreamInfo;
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

                jt = json.Value<JToken>("ffmpegPath");
                if (jt != null)
                {
                    config.FfmpegPath = jt.Value<string>();
                }
                textBoxFfmpegPath.Text = config.FfmpegPath;

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
            };
            config.Load();
            textBoxFileNameFormat.Text = config.FileNameFormat;
            numericUpDownTimerIntervalInactive.Value = config.CheckingIntervalInactive;
            numericUpDownTimerIntervalActive.Value = config.CheckingIntervalActive;
            chkSaveStreamInfo.Checked = config.SaveStreamInfo;

            listViewStreams.Columns[COLUMN_ID_CHANNEL_NAME].Width += 1;
            if (File.Exists(config.StreamListFilePath))
            {
                LoadList(config.StreamListFilePath);
            }

            string[] args = Environment.GetCommandLineArgs();
            foreach (string s in args)
            {
                if (s.ToLower().Equals("/enabled"))
                {
                    chkTimerEnabled.Checked = true;
                }
                else if (s.ToLower().Equals("/check"))
                {
                    CheckAllItems();
                }
            }

            listViewStreams.SetDoubleBuffered(true);

            AddToLog(null, "Программа запущена");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                config.Save();
                SaveList(config.StreamListFilePath);
            }
        }

        private void SaveList(string filePath)
        {
            JArray jArray = new JArray();
            foreach (ListViewItem item in listViewStreams.Items)
            {
                StreamItem streamItem = item.Tag as StreamItem;
                JObject jItem = new JObject();
                jItem["channelName"] = streamItem.ChannelName;
                jItem["copiesCount"] = streamItem.CopiesCount;
                jItem["important"] = streamItem.IsImportant;
                jArray.Add(jItem);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, jArray.ToString());
        }

        private void LoadList(string filePath)
        {
            JArray jArray = JArray.Parse(File.ReadAllText(filePath));
            foreach (JObject j in jArray)
            {
                string channelName = j.Value<string>("channelName");
                StreamItem streamItem = new StreamItem(channelName);
                streamItem.CopiesCount = j.Value<int>("copiesCount");
                if (streamItem.CopiesCount > 3)
                {
                    streamItem.CopiesCount = 3;
                }
                else if (streamItem.CopiesCount < 0)
                {
                    streamItem.CopiesCount = 0;
                }
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

        private void btnBrowseFfmpeg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите файл FFMPEG.EXE";
            ofd.Filter = "*.EXE|*.exe";
            string t = Path.GetDirectoryName(config.FfmpegPath);
            ofd.InitialDirectory = t != null && t.Equals(string.Empty) ? config.SelfDirPath : t;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                config.FfmpegPath = ofd.FileName;
                textBoxFfmpegPath.Text = ofd.FileName;
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

        private void textBoxFfmpegPath_Leave(object sender, EventArgs e)
        {
            config.FfmpegPath = textBoxFfmpegPath.Text;
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
            string sizeString = streamItem.DumpingFileSize >= 0L ? streamItem.DumpingFileSize.ToString() : string.Empty;
            item.SubItems.Add(sizeString);
            item.SubItems.Add(streamItem.CopiesCount.ToString());
            item.SubItems.Add(string.Empty);
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
                listViewStreams.Items[id].SubItems[COLUMN_ID_DATE].Text = null;
                listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLIST_URL].Text = null;
            }
        }

        private void CheckItem(StreamItem streamItem, int itemIndex)
        {
            if (!streamItem.IsChecking)
            {
                streamItem.IsChecking = true;
                listViewStreams.Items[itemIndex].SubItems[COLUMN_ID_TIMER].Text = "Запуск проверки...";
                if (!streamItem.IsStreamActive)
                {
                    listViewStreams.Items[itemIndex].SubItems[COLUMN_ID_FILEPATH].Text = null;
                }
                MakeThread(streamItem).Start(SynchronizationContext.Current);
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
                try
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private Thread MakeThread(StreamItem streamItem)
        {
            ThreadChecker threadChecker = new ThreadChecker(streamItem, config.SaveStreamInfo);
            threadChecker.CheckingStarted += OnCheckingStarted;
            threadChecker.NewLiveDetected += OnLiveDetected;
            threadChecker.TitleDetected += OnTitleDetected;
            threadChecker.PlaylistUrlDetected += OnPlaylistUrlDetected;
            threadChecker.DumpingStarted += OnDumpingStarted;
            threadChecker.FileSizeChanged += OnFileSizeChanged;
            threadChecker.DumpingHalted += OnDumpingHalted;
            threadChecker.TitleChanged += OnTitleChanged;
            threadChecker.Completed += OnThreadCheckingCompleted;
            threadChecker.LogAdding += OnLogAdding;

            return new Thread(threadChecker.Work);
        }

        private void OnCheckingStarted(object sender)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text = "Проверка...";
            }
        }

        private void OnLiveDetected(object sender)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            notifyIcon1.BalloonTipTitle = "СТРИИИИИИМ!!!!!!!";
            notifyIcon1.BalloonTipText = $"Канал {streamItem.ChannelName} начал трансляцию!";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(5000);

            AddToLog(streamItem.ChannelName, "Новая трансляция!");
            if (streamItem.CopiesCount == 0)
            {
                AddToLog(streamItem.ChannelName, "<Dumping is disabled!>");
            }
        }

        private void OnTitleDetected(object sender, string title)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            AddToLog(streamItem.ChannelName, $"Название стрима: {title}");
        }

        private void OnPlaylistUrlDetected(object sender, string playlistUrl)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLIST_URL].Text = playlistUrl;
            }
        }

        private void OnTitleChanged(object sender)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            notifyIcon1.BalloonTipTitle = streamItem.ChannelName;
            notifyIcon1.BalloonTipText = $"Изменилось название: {streamItem.Title}";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(5000);

            AddToLog(streamItem.ChannelName, $"Новое название стрима: {streamItem.Title}");
        }

        private void OnDumpingStarted(object sender, int errorCode)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                if (errorCode == 200)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text = streamItem.DumpingFilePath;
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = "0";
                    listViewStreams.Items[id].SubItems[COLUMN_ID_DATE].Text = streamItem.DateLocal.ToString();
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

        private void OnFileSizeChanged(object sender, long oldFileSize)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = FormatSize(streamItem.DumpingFileSize);
            }
        }

        private void OnDumpingHalted(object sender)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                notifyIcon1.BalloonTipTitle = streamItem.ChannelName;
                notifyIcon1.BalloonTipText = "ЗАВИСЛО!!!";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon1.ShowBalloonTip(5000);

                AddToLog(streamItem.ChannelName, "Зависло нафиг!");

                ResetItem(streamItem);

                MakeThread(streamItem).Start(SynchronizationContext.Current);
            }
        }

        private void OnThreadCheckingCompleted(object sender, int errorCode)
        {
            StreamItem streamItem = (sender as ThreadChecker).StreamItem;
            int id = FindStreamItemInListView(streamItem, listViewStreams);
            if (id >= 0)
            {
                if (errorCode == 200)
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text =
                        streamItem.CopiesCount > 0 ? streamItem.DumpingFilePath : "<Dumping is disabled>";

                    streamItem.TimerRemaining = config.CheckingIntervalActive;
                }
                else
                {
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text =
                        errorCode != TwitchApi.ERROR_USER_OFFLINE ?
                        $"Ошибка! {TwitchApi.ErrorCodeToString(errorCode)}, {(sender as ThreadChecker).LastErrorMessage}" : null;
                    listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = null;
                    listViewStreams.Items[id].SubItems[COLUMN_ID_DATE].Text = null;
                    listViewStreams.Items[id].SubItems[COLUMN_ID_PLAYLIST_URL].Text = null;

                    streamItem.TimerRemaining = errorCode == TwitchApi.ERROR_USER_NOT_FOUND ?
                        (int)numericUpDownTimerIntervalActive.Maximum : config.CheckingIntervalInactive;
                }
                listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text =
                    timerCheck.Enabled ? streamItem.TimerRemaining.ToString() : "Отключен!";
            }
            streamItem.IsChecking = false;
        }

        private void OnLogAdding(object sender, string logText)
        {
            AddToLog((sender as ThreadChecker).StreamItem.ChannelName, logText);
        }

        private void chkTimerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTimerEnabled.Checked)
            {
                if (string.IsNullOrEmpty(config.FfmpegPath) || string.IsNullOrWhiteSpace(config.FfmpegPath) ||
                    !File.Exists(config.FfmpegPath))
                {
                    notifyIcon1.BalloonTipTitle = "О горе!";
                    notifyIcon1.BalloonTipText = "FFMPEG.EXE не найден!";
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                    notifyIcon1.ShowBalloonTip(5000);
                    chkTimerEnabled.Checked = false;
                    return;
                }

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

        private void numericUpDownTimerIntervalInactive_ValueChanged(object sender, EventArgs e)
        {
            config.CheckingIntervalInactive = (int)numericUpDownTimerIntervalInactive.Value;
        }

        private void numericUpDownTimerIntervalActive_ValueChanged(object sender, EventArgs e)
        {
            config.CheckingIntervalActive = (int)numericUpDownTimerIntervalActive.Value;
        }

        private void btnSetDefaultFilNameFormat_Click(object sender, EventArgs e)
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

        private void listViewStreams_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
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

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                miImportantChannelToolStripMenuItem.Checked = streamItem.IsImportant;
                miKeepAliveToolStripMenuItem.Checked = streamItem.KeepAlive;
            }
        }

        private void miEditChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                int prevCopiesCount = streamItem.CopiesCount;
                FormItemEditor itemEditor = new FormItemEditor(streamItem);
                if (itemEditor.ShowDialog() == DialogResult.OK)
                {
                    streamItem.TimerRemaining =
                        streamItem.IsStreamActive ? config.CheckingIntervalActive : config.CheckingIntervalInactive;
                    listViewStreams.Items[id].SubItems[COLUMN_ID_COPIES_COUNT].Text =
                        streamItem.CopiesCount.ToString();
                    listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text =
                        timerCheck.Enabled ? streamItem.TimerRemaining.ToString() : "Отключен!";
                    if (streamItem.IsStreamActive)
                    {
                        listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text = streamItem.DumpingFilePath;
                        streamItem.DumpingFileSize = GetFileSize(streamItem.DumpingFilePath);
                        if (prevCopiesCount == 0 && streamItem.CopiesCount > 0 && streamItem.DumpingFileSize == -1L)
                        {
                            LaunchFfmpeg(streamItem.PlaylistUrl, streamItem.DumpingFilePath);
                            streamItem.DumpingFileSize = 0L;
                            listViewStreams.Items[id].SubItems[COLUMN_ID_FILESIZE].Text = "0";
                        }
                    }
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

        private void miClearDumpInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                ResetItem(streamItem);
            }
        }

        private void miKeepAliveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                streamItem.KeepAlive = !streamItem.KeepAlive;
            }
        }

        private void miRhemoveChannelToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void miBrowseForKeepAliveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = listViewStreams.SelectedIndex();
            if (id >= 0)
            {
                StreamItem streamItem = listViewStreams.Items[id].Tag as StreamItem;
                if (string.IsNullOrEmpty(streamItem.DumpingFilePath))
                {
                    MessageBox.Show("Стрима нет!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Title = "Select a file";
                        if (!string.IsNullOrEmpty(streamItem.DumpingFilePath))
                        {
                            ofd.InitialDirectory = Path.GetDirectoryName(streamItem.DumpingFilePath);
                        }
                        ofd.Filter = "*.TS-files|*.ts";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            streamItem.DumpingFilePath = ofd.FileName;
                            streamItem.DumpingFileSize = GetFileSize(ofd.FileName);
                            streamItem.TimerRemaining = config.CheckingIntervalActive;

                            listViewStreams.Items[id].SubItems[COLUMN_ID_TIMER].Text = streamItem.TimerRemaining.ToString();
                            listViewStreams.Items[id].SubItems[COLUMN_ID_FILEPATH].Text = ofd.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
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

        private void chkSaveStreamInfo_CheckedChanged(object sender, EventArgs e)
        {
            config.SaveStreamInfo = chkSaveStreamInfo.Checked;
        }
    }
}
