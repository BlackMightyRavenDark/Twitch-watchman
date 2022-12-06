
namespace Twitch_watchman
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listViewStreams = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddNewStream = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.chkTimerEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkSaveStreamInfo = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownTimerIntervalActive = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownTimerIntervalInactive = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetDefaultFilNameFormat = new System.Windows.Forms.Button();
            this.textBoxFileNameFormat = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseFfmpeg = new System.Windows.Forms.Button();
            this.btnBrowseDownloadingDirectory = new System.Windows.Forms.Button();
            this.textBoxDownloadingDir = new System.Windows.Forms.TextBox();
            this.textBoxFfmpegPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCheckChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRhemoveChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyChannelNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miImportantChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miClearDumpInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miKeepAliveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miBrowseForKeepAliveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerIntervalActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerIntervalInactive)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(907, 189);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.listViewStreams);
            this.tabPage1.Controls.Add(this.btnAddNewStream);
            this.tabPage1.Controls.Add(this.btnCheckAll);
            this.tabPage1.Controls.Add(this.chkTimerEnabled);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(899, 163);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Главная";
            // 
            // listViewStreams
            // 
            this.listViewStreams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewStreams.BackColor = System.Drawing.SystemColors.Control;
            this.listViewStreams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader10});
            this.listViewStreams.FullRowSelect = true;
            this.listViewStreams.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreams.HideSelection = false;
            this.listViewStreams.LabelWrap = false;
            this.listViewStreams.Location = new System.Drawing.Point(6, 35);
            this.listViewStreams.MultiSelect = false;
            this.listViewStreams.Name = "listViewStreams";
            this.listViewStreams.OwnerDraw = true;
            this.listViewStreams.Size = new System.Drawing.Size(887, 120);
            this.listViewStreams.TabIndex = 5;
            this.listViewStreams.UseCompatibleStateImageBehavior = false;
            this.listViewStreams.View = System.Windows.Forms.View.Details;
            this.listViewStreams.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewStreams_DrawColumnHeader);
            this.listViewStreams.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewStreams_DrawSubItem);
            this.listViewStreams.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewStreams_KeyDown);
            this.listViewStreams.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewStreams_MouseDoubleClick);
            this.listViewStreams.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewStreams_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Канал";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Таймер";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Дампинг файл";
            this.columnHeader3.Width = 250;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Размер файла";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Копии";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Дата батона";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Ссылка";
            this.columnHeader10.Width = 400;
            // 
            // btnAddNewStream
            // 
            this.btnAddNewStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewStream.Location = new System.Drawing.Point(818, 6);
            this.btnAddNewStream.Name = "btnAddNewStream";
            this.btnAddNewStream.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewStream.TabIndex = 4;
            this.btnAddNewStream.Text = "Добавить";
            this.btnAddNewStream.UseVisualStyleBackColor = true;
            this.btnAddNewStream.Click += new System.EventHandler(this.btnAddNewStream_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(6, 6);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(95, 23);
            this.btnCheckAll.TabIndex = 3;
            this.btnCheckAll.Text = "Проверить все";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // chkTimerEnabled
            // 
            this.chkTimerEnabled.AutoSize = true;
            this.chkTimerEnabled.Location = new System.Drawing.Point(107, 10);
            this.chkTimerEnabled.Name = "chkTimerEnabled";
            this.chkTimerEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkTimerEnabled.TabIndex = 2;
            this.chkTimerEnabled.Text = "Таймер";
            this.chkTimerEnabled.UseVisualStyleBackColor = true;
            this.chkTimerEnabled.CheckedChanged += new System.EventHandler(this.chkTimerEnabled_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.chkSaveStreamInfo);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(899, 163);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Настройки";
            // 
            // chkSaveStreamInfo
            // 
            this.chkSaveStreamInfo.AutoSize = true;
            this.chkSaveStreamInfo.Location = new System.Drawing.Point(21, 252);
            this.chkSaveStreamInfo.Name = "chkSaveStreamInfo";
            this.chkSaveStreamInfo.Size = new System.Drawing.Size(202, 17);
            this.chkSaveStreamInfo.TabIndex = 7;
            this.chkSaveStreamInfo.Text = "Сохранять информацию о сттриме";
            this.chkSaveStreamInfo.UseVisualStyleBackColor = true;
            this.chkSaveStreamInfo.CheckedChanged += new System.EventHandler(this.chkSaveStreamInfo_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.numericUpDownTimerIntervalActive);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numericUpDownTimerIntervalInactive);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(6, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(836, 81);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Частота проверки";
            // 
            // numericUpDownTimerIntervalActive
            // 
            this.numericUpDownTimerIntervalActive.Location = new System.Drawing.Point(277, 50);
            this.numericUpDownTimerIntervalActive.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalActive.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalActive.Name = "numericUpDownTimerIntervalActive";
            this.numericUpDownTimerIntervalActive.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownTimerIntervalActive.TabIndex = 3;
            this.numericUpDownTimerIntervalActive.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalActive.ValueChanged += new System.EventHandler(this.numericUpDownTimerIntervalActive_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Интервал таймера в секундах, когда стрим есть:";
            // 
            // numericUpDownTimerIntervalInactive
            // 
            this.numericUpDownTimerIntervalInactive.Location = new System.Drawing.Point(277, 22);
            this.numericUpDownTimerIntervalInactive.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalInactive.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalInactive.Name = "numericUpDownTimerIntervalInactive";
            this.numericUpDownTimerIntervalInactive.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownTimerIntervalInactive.TabIndex = 1;
            this.numericUpDownTimerIntervalInactive.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownTimerIntervalInactive.ValueChanged += new System.EventHandler(this.numericUpDownTimerIntervalInactive_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Интервал таймера в секундах, когда стрима нет:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSetDefaultFilNameFormat);
            this.groupBox2.Controls.Add(this.textBoxFileNameFormat);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnBrowseFfmpeg);
            this.groupBox2.Controls.Add(this.btnBrowseDownloadingDirectory);
            this.groupBox2.Controls.Add(this.textBoxDownloadingDir);
            this.groupBox2.Controls.Add(this.textBoxFfmpegPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(836, 153);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Файлы и папки";
            // 
            // btnSetDefaultFilNameFormat
            // 
            this.btnSetDefaultFilNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaultFilNameFormat.Location = new System.Drawing.Point(721, 123);
            this.btnSetDefaultFilNameFormat.Name = "btnSetDefaultFilNameFormat";
            this.btnSetDefaultFilNameFormat.Size = new System.Drawing.Size(109, 23);
            this.btnSetDefaultFilNameFormat.TabIndex = 8;
            this.btnSetDefaultFilNameFormat.Text = "Вернуть как было";
            this.btnSetDefaultFilNameFormat.UseVisualStyleBackColor = true;
            this.btnSetDefaultFilNameFormat.Click += new System.EventHandler(this.btnSetDefaultFilNameFormat_Click);
            // 
            // textBoxFileNameFormat
            // 
            this.textBoxFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileNameFormat.Location = new System.Drawing.Point(15, 125);
            this.textBoxFileNameFormat.Name = "textBoxFileNameFormat";
            this.textBoxFileNameFormat.Size = new System.Drawing.Size(700, 20);
            this.textBoxFileNameFormat.TabIndex = 7;
            this.textBoxFileNameFormat.Leave += new System.EventHandler(this.textBoxFileNameFormat_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Формат имени файла:";
            // 
            // btnBrowseFfmpeg
            // 
            this.btnBrowseFfmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFfmpeg.Location = new System.Drawing.Point(789, 77);
            this.btnBrowseFfmpeg.Name = "btnBrowseFfmpeg";
            this.btnBrowseFfmpeg.Size = new System.Drawing.Size(41, 23);
            this.btnBrowseFfmpeg.TabIndex = 5;
            this.btnBrowseFfmpeg.Text = "...";
            this.btnBrowseFfmpeg.UseVisualStyleBackColor = true;
            this.btnBrowseFfmpeg.Click += new System.EventHandler(this.btnBrowseFfmpeg_Click);
            // 
            // btnBrowseDownloadingDirectory
            // 
            this.btnBrowseDownloadingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDownloadingDirectory.Location = new System.Drawing.Point(789, 33);
            this.btnBrowseDownloadingDirectory.Name = "btnBrowseDownloadingDirectory";
            this.btnBrowseDownloadingDirectory.Size = new System.Drawing.Size(41, 23);
            this.btnBrowseDownloadingDirectory.TabIndex = 4;
            this.btnBrowseDownloadingDirectory.Text = "...";
            this.btnBrowseDownloadingDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseDownloadingDirectory.Click += new System.EventHandler(this.btnBrowseDownloadingDirectory_Click);
            // 
            // textBoxDownloadingDir
            // 
            this.textBoxDownloadingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDownloadingDir.Location = new System.Drawing.Point(16, 35);
            this.textBoxDownloadingDir.Name = "textBoxDownloadingDir";
            this.textBoxDownloadingDir.Size = new System.Drawing.Size(767, 20);
            this.textBoxDownloadingDir.TabIndex = 0;
            this.textBoxDownloadingDir.Leave += new System.EventHandler(this.textBoxDownloadingDir_Leave);
            // 
            // textBoxFfmpegPath
            // 
            this.textBoxFfmpegPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFfmpegPath.Location = new System.Drawing.Point(15, 79);
            this.textBoxFfmpegPath.Name = "textBoxFfmpegPath";
            this.textBoxFfmpegPath.Size = new System.Drawing.Size(768, 20);
            this.textBoxFfmpegPath.TabIndex = 1;
            this.textBoxFfmpegPath.Leave += new System.EventHandler(this.textBoxFfmpegPath_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Путь до FFMPEG.EXE:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Папка для скачивания:";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.listViewLog);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(899, 163);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Лог";
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.BackColor = System.Drawing.SystemColors.Control;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Location = new System.Drawing.Point(0, 0);
            this.listViewLog.MultiSelect = false;
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(896, 158);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Дата батона";
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Канал";
            this.columnHeader8.Width = 120;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Событие";
            this.columnHeader9.Width = 300;
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 1000;
            this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Twitch watchman";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCheckChannelToolStripMenuItem,
            this.miEditChannelToolStripMenuItem,
            this.miRhemoveChannelToolStripMenuItem,
            this.miCopyChannelNameToolStripMenuItem,
            this.miOpenChannelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.miImportantChannelToolStripMenuItem,
            this.miClearDumpInfoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.miKeepAliveToolStripMenuItem,
            this.miBrowseForKeepAliveFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(271, 232);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // miCheckChannelToolStripMenuItem
            // 
            this.miCheckChannelToolStripMenuItem.Name = "miCheckChannelToolStripMenuItem";
            this.miCheckChannelToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miCheckChannelToolStripMenuItem.Text = "Проверить";
            this.miCheckChannelToolStripMenuItem.Click += new System.EventHandler(this.miCheckChannelToolStripMenuItem_Click);
            // 
            // miEditChannelToolStripMenuItem
            // 
            this.miEditChannelToolStripMenuItem.Name = "miEditChannelToolStripMenuItem";
            this.miEditChannelToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miEditChannelToolStripMenuItem.Text = "Редактировать";
            this.miEditChannelToolStripMenuItem.Click += new System.EventHandler(this.miEditChannelToolStripMenuItem_Click);
            // 
            // miRhemoveChannelToolStripMenuItem
            // 
            this.miRhemoveChannelToolStripMenuItem.Name = "miRhemoveChannelToolStripMenuItem";
            this.miRhemoveChannelToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miRhemoveChannelToolStripMenuItem.Text = "Удалить";
            this.miRhemoveChannelToolStripMenuItem.Click += new System.EventHandler(this.miRhemoveChannelToolStripMenuItem_Click);
            // 
            // miCopyChannelNameToolStripMenuItem
            // 
            this.miCopyChannelNameToolStripMenuItem.Name = "miCopyChannelNameToolStripMenuItem";
            this.miCopyChannelNameToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miCopyChannelNameToolStripMenuItem.Text = "Скопировать название канала";
            this.miCopyChannelNameToolStripMenuItem.Click += new System.EventHandler(this.miCopyChannelNameToolStripMenuItem_Click);
            // 
            // miOpenChannelToolStripMenuItem
            // 
            this.miOpenChannelToolStripMenuItem.Name = "miOpenChannelToolStripMenuItem";
            this.miOpenChannelToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miOpenChannelToolStripMenuItem.Text = "Открыть канал";
            this.miOpenChannelToolStripMenuItem.Click += new System.EventHandler(this.miOpenChannelToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(267, 6);
            // 
            // miImportantChannelToolStripMenuItem
            // 
            this.miImportantChannelToolStripMenuItem.Name = "miImportantChannelToolStripMenuItem";
            this.miImportantChannelToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miImportantChannelToolStripMenuItem.Text = "Важный канал";
            this.miImportantChannelToolStripMenuItem.Click += new System.EventHandler(this.miImportantChannelToolStripMenuItem_Click);
            // 
            // miClearDumpInfoToolStripMenuItem
            // 
            this.miClearDumpInfoToolStripMenuItem.Name = "miClearDumpInfoToolStripMenuItem";
            this.miClearDumpInfoToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miClearDumpInfoToolStripMenuItem.Text = "Забыть стрим";
            this.miClearDumpInfoToolStripMenuItem.Click += new System.EventHandler(this.miClearDumpInfoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(267, 6);
            // 
            // miKeepAliveToolStripMenuItem
            // 
            this.miKeepAliveToolStripMenuItem.Name = "miKeepAliveToolStripMenuItem";
            this.miKeepAliveToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miKeepAliveToolStripMenuItem.Text = "Антизависатор";
            this.miKeepAliveToolStripMenuItem.Click += new System.EventHandler(this.miKeepAliveToolStripMenuItem_Click);
            // 
            // miBrowseForKeepAliveFileToolStripMenuItem
            // 
            this.miBrowseForKeepAliveFileToolStripMenuItem.Name = "miBrowseForKeepAliveFileToolStripMenuItem";
            this.miBrowseForKeepAliveFileToolStripMenuItem.Size = new System.Drawing.Size(270, 24);
            this.miBrowseForKeepAliveFileToolStripMenuItem.Text = "Выбрать файл для слежения...";
            this.miBrowseForKeepAliveFileToolStripMenuItem.Click += new System.EventHandler(this.miBrowseForKeepAliveFileToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 213);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(580, 250);
            this.Name = "Form1";
            this.Text = "Twitch watchman";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerIntervalActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimerIntervalInactive)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFfmpegPath;
        private System.Windows.Forms.TextBox textBoxDownloadingDir;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseFfmpeg;
        private System.Windows.Forms.Button btnBrowseDownloadingDirectory;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox chkTimerEnabled;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDownTimerIntervalInactive;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFileNameFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSetDefaultFilNameFormat;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnAddNewStream;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miEditChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miCheckChannelToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numericUpDownTimerIntervalActive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miImportantChannelToolStripMenuItem;
        private System.Windows.Forms.ListView listViewStreams;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ToolStripMenuItem miClearDumpInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miBrowseForKeepAliveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miKeepAliveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRhemoveChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miOpenChannelToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSaveStreamInfo;
        private System.Windows.Forms.ToolStripMenuItem miCopyChannelNameToolStripMenuItem;
    }
}
