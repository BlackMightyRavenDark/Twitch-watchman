
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
            this.columnHeaderChannelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTimer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDumpingFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDelay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNewChunks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderProcessedChunks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFirstChunkId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLostChunks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDumpStartedDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPlaylistUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddNewStream = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.checkBoxTimerEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxStopIfPlaylistLost = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveChunksInfo = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveStreamInfo = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownTimerIntervalActive = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownTimerIntervalInactive = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetDefaultFileNameFormat = new System.Windows.Forms.Button();
            this.textBoxFileNameFormat = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseDownloadingDirectory = new System.Windows.Forms.Button();
            this.textBoxDownloadingDir = new System.Windows.Forms.TextBox();
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
            this.miRemoveChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyChannelNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyPlaylistUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miImportantChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miStopDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.tabPage1.Controls.Add(this.checkBoxTimerEnabled);
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
            this.columnHeaderChannelName,
            this.columnHeaderTimer,
            this.columnHeaderDumpingFile,
            this.columnHeaderFileSize,
            this.columnHeaderDelay,
            this.columnHeaderNewChunks,
            this.columnHeaderProcessedChunks,
            this.columnHeaderFirstChunkId,
            this.columnHeaderLostChunks,
            this.columnHeaderDumpStartedDate,
            this.columnHeaderPlaylistUrl});
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
            // columnHeaderChannelName
            // 
            this.columnHeaderChannelName.Text = "Канал";
            this.columnHeaderChannelName.Width = 100;
            // 
            // columnHeaderTimer
            // 
            this.columnHeaderTimer.Text = "Таймер";
            this.columnHeaderTimer.Width = 80;
            // 
            // columnHeaderDumpingFile
            // 
            this.columnHeaderDumpingFile.Text = "Дампинг файл";
            this.columnHeaderDumpingFile.Width = 250;
            // 
            // columnHeaderFileSize
            // 
            this.columnHeaderFileSize.Text = "Размер файла";
            this.columnHeaderFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileSize.Width = 100;
            // 
            // columnHeaderDelay
            // 
            this.columnHeaderDelay.Text = "Задержка";
            this.columnHeaderDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderDelay.Width = 100;
            // 
            // columnHeaderNewChunks
            // 
            this.columnHeaderNewChunks.Text = "Новые чанки";
            this.columnHeaderNewChunks.Width = 80;
            // 
            // columnHeaderProcessedChunks
            // 
            this.columnHeaderProcessedChunks.Text = "Обработано чанков";
            this.columnHeaderProcessedChunks.Width = 80;
            // 
            // columnHeaderFirstChunkId
            // 
            this.columnHeaderFirstChunkId.Text = "Первый чанк";
            this.columnHeaderFirstChunkId.Width = 80;
            // 
            // columnHeaderLostChunks
            // 
            this.columnHeaderLostChunks.Text = "Потеряно чанков";
            this.columnHeaderLostChunks.Width = 100;
            // 
            // columnHeaderDumpStartedDate
            // 
            this.columnHeaderDumpStartedDate.Text = "Дампинг начат";
            this.columnHeaderDumpStartedDate.Width = 120;
            // 
            // columnHeaderPlaylistUrl
            // 
            this.columnHeaderPlaylistUrl.Text = "Ссылка на плейлист";
            this.columnHeaderPlaylistUrl.Width = 400;
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
            // checkBoxTimerEnabled
            // 
            this.checkBoxTimerEnabled.AutoSize = true;
            this.checkBoxTimerEnabled.Location = new System.Drawing.Point(107, 10);
            this.checkBoxTimerEnabled.Name = "checkBoxTimerEnabled";
            this.checkBoxTimerEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxTimerEnabled.TabIndex = 2;
            this.checkBoxTimerEnabled.Text = "Таймер";
            this.checkBoxTimerEnabled.UseVisualStyleBackColor = true;
            this.checkBoxTimerEnabled.CheckedChanged += new System.EventHandler(this.checkBoxTimerEnabled_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.checkBoxStopIfPlaylistLost);
            this.tabPage2.Controls.Add(this.checkBoxSaveChunksInfo);
            this.tabPage2.Controls.Add(this.checkBoxSaveStreamInfo);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(899, 163);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Настройки";
            // 
            // checkBoxStopIfPlaylistLost
            // 
            this.checkBoxStopIfPlaylistLost.AutoSize = true;
            this.checkBoxStopIfPlaylistLost.Location = new System.Drawing.Point(420, 201);
            this.checkBoxStopIfPlaylistLost.Name = "checkBoxStopIfPlaylistLost";
            this.checkBoxStopIfPlaylistLost.Size = new System.Drawing.Size(314, 17);
            this.checkBoxStopIfPlaylistLost.TabIndex = 9;
            this.checkBoxStopIfPlaylistLost.Text = "Остановить дампинг, если плейлист больше недоступен";
            this.toolTip1.SetToolTip(this.checkBoxStopIfPlaylistLost, "Иначе, остановка после 5 ошибок. Нельзя изменить для активных трансляций!");
            this.checkBoxStopIfPlaylistLost.UseVisualStyleBackColor = true;
            this.checkBoxStopIfPlaylistLost.CheckedChanged += new System.EventHandler(this.checkBoxStopIfPlaylistLost_CheckedChanged);
            // 
            // checkBoxSaveChunksInfo
            // 
            this.checkBoxSaveChunksInfo.AutoSize = true;
            this.checkBoxSaveChunksInfo.Location = new System.Drawing.Point(211, 201);
            this.checkBoxSaveChunksInfo.Name = "checkBoxSaveChunksInfo";
            this.checkBoxSaveChunksInfo.Size = new System.Drawing.Size(194, 17);
            this.checkBoxSaveChunksInfo.TabIndex = 8;
            this.checkBoxSaveChunksInfo.Text = "Сохранять информацию о чанках";
            this.toolTip1.SetToolTip(this.checkBoxSaveChunksInfo, "Нельзя изменить для активных трансляций!");
            this.checkBoxSaveChunksInfo.UseVisualStyleBackColor = true;
            this.checkBoxSaveChunksInfo.CheckedChanged += new System.EventHandler(this.checkBoxSaveChunksInfo_CheckedChanged);
            // 
            // checkBoxSaveStreamInfo
            // 
            this.checkBoxSaveStreamInfo.AutoSize = true;
            this.checkBoxSaveStreamInfo.Location = new System.Drawing.Point(3, 201);
            this.checkBoxSaveStreamInfo.Name = "checkBoxSaveStreamInfo";
            this.checkBoxSaveStreamInfo.Size = new System.Drawing.Size(197, 17);
            this.checkBoxSaveStreamInfo.TabIndex = 7;
            this.checkBoxSaveStreamInfo.Text = "Сохранять информацию о стриме";
            this.toolTip1.SetToolTip(this.checkBoxSaveStreamInfo, "Нельзя изменить для активных трансляций!");
            this.checkBoxSaveStreamInfo.UseVisualStyleBackColor = true;
            this.checkBoxSaveStreamInfo.CheckedChanged += new System.EventHandler(this.checkBoxSaveStreamInfo_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.numericUpDownTimerIntervalActive);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numericUpDownTimerIntervalInactive);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(3, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(842, 81);
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
            this.groupBox2.Controls.Add(this.btnSetDefaultFileNameFormat);
            this.groupBox2.Controls.Add(this.textBoxFileNameFormat);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnBrowseDownloadingDirectory);
            this.groupBox2.Controls.Add(this.textBoxDownloadingDir);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(842, 105);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Файлы и папки";
            // 
            // btnSetDefaultFileNameFormat
            // 
            this.btnSetDefaultFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaultFileNameFormat.Location = new System.Drawing.Point(727, 74);
            this.btnSetDefaultFileNameFormat.Name = "btnSetDefaultFileNameFormat";
            this.btnSetDefaultFileNameFormat.Size = new System.Drawing.Size(109, 23);
            this.btnSetDefaultFileNameFormat.TabIndex = 8;
            this.btnSetDefaultFileNameFormat.Text = "Вернуть как было";
            this.btnSetDefaultFileNameFormat.UseVisualStyleBackColor = true;
            this.btnSetDefaultFileNameFormat.Click += new System.EventHandler(this.btnSetDefaultFileNameFormat_Click);
            // 
            // textBoxFileNameFormat
            // 
            this.textBoxFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileNameFormat.Location = new System.Drawing.Point(15, 76);
            this.textBoxFileNameFormat.Name = "textBoxFileNameFormat";
            this.textBoxFileNameFormat.Size = new System.Drawing.Size(706, 20);
            this.textBoxFileNameFormat.TabIndex = 7;
            this.textBoxFileNameFormat.Leave += new System.EventHandler(this.textBoxFileNameFormat_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Формат имени файла:";
            // 
            // btnBrowseDownloadingDirectory
            // 
            this.btnBrowseDownloadingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDownloadingDirectory.Location = new System.Drawing.Point(795, 35);
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
            this.textBoxDownloadingDir.Location = new System.Drawing.Point(16, 37);
            this.textBoxDownloadingDir.Name = "textBoxDownloadingDir";
            this.textBoxDownloadingDir.Size = new System.Drawing.Size(773, 20);
            this.textBoxDownloadingDir.TabIndex = 0;
            this.textBoxDownloadingDir.Leave += new System.EventHandler(this.textBoxDownloadingDir_Leave);
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
            this.miRemoveChannelToolStripMenuItem,
            this.miCopyChannelNameToolStripMenuItem,
            this.miCopyPlaylistUrlToolStripMenuItem,
            this.miOpenChannelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.miImportantChannelToolStripMenuItem,
            this.toolStripMenuItem2,
            this.miStopDumpToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(289, 208);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // miCheckChannelToolStripMenuItem
            // 
            this.miCheckChannelToolStripMenuItem.Name = "miCheckChannelToolStripMenuItem";
            this.miCheckChannelToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miCheckChannelToolStripMenuItem.Text = "Проверить";
            this.miCheckChannelToolStripMenuItem.Click += new System.EventHandler(this.miCheckChannelToolStripMenuItem_Click);
            // 
            // miEditChannelToolStripMenuItem
            // 
            this.miEditChannelToolStripMenuItem.Name = "miEditChannelToolStripMenuItem";
            this.miEditChannelToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miEditChannelToolStripMenuItem.Text = "Редактировать";
            this.miEditChannelToolStripMenuItem.Click += new System.EventHandler(this.miEditChannelToolStripMenuItem_Click);
            // 
            // miRemoveChannelToolStripMenuItem
            // 
            this.miRemoveChannelToolStripMenuItem.Name = "miRemoveChannelToolStripMenuItem";
            this.miRemoveChannelToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miRemoveChannelToolStripMenuItem.Text = "Удалить";
            this.miRemoveChannelToolStripMenuItem.Click += new System.EventHandler(this.miRemoveChannelToolStripMenuItem_Click);
            // 
            // miCopyChannelNameToolStripMenuItem
            // 
            this.miCopyChannelNameToolStripMenuItem.Name = "miCopyChannelNameToolStripMenuItem";
            this.miCopyChannelNameToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miCopyChannelNameToolStripMenuItem.Text = "Скопировать название канала";
            this.miCopyChannelNameToolStripMenuItem.Click += new System.EventHandler(this.miCopyChannelNameToolStripMenuItem_Click);
            // 
            // miCopyPlaylistUrlToolStripMenuItem
            // 
            this.miCopyPlaylistUrlToolStripMenuItem.Name = "miCopyPlaylistUrlToolStripMenuItem";
            this.miCopyPlaylistUrlToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miCopyPlaylistUrlToolStripMenuItem.Text = "Скопировать ссылку на плейлист";
            this.miCopyPlaylistUrlToolStripMenuItem.Click += new System.EventHandler(this.miCopyPlaylistUrlToolStripMenuItem_Click);
            // 
            // miOpenChannelToolStripMenuItem
            // 
            this.miOpenChannelToolStripMenuItem.Name = "miOpenChannelToolStripMenuItem";
            this.miOpenChannelToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miOpenChannelToolStripMenuItem.Text = "Открыть канал";
            this.miOpenChannelToolStripMenuItem.Click += new System.EventHandler(this.miOpenChannelToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(285, 6);
            // 
            // miImportantChannelToolStripMenuItem
            // 
            this.miImportantChannelToolStripMenuItem.Name = "miImportantChannelToolStripMenuItem";
            this.miImportantChannelToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miImportantChannelToolStripMenuItem.Text = "Важный канал";
            this.miImportantChannelToolStripMenuItem.Click += new System.EventHandler(this.miImportantChannelToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(285, 6);
            // 
            // miStopDumpToolStripMenuItem
            // 
            this.miStopDumpToolStripMenuItem.Name = "miStopDumpToolStripMenuItem";
            this.miStopDumpToolStripMenuItem.Size = new System.Drawing.Size(288, 24);
            this.miStopDumpToolStripMenuItem.Text = "Остановить дампинг";
            this.miStopDumpToolStripMenuItem.Click += new System.EventHandler(this.miStopDumpToolStripMenuItem_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDownloadingDir;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseDownloadingDirectory;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox checkBoxTimerEnabled;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDownTimerIntervalInactive;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFileNameFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSetDefaultFileNameFormat;
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
        private System.Windows.Forms.ColumnHeader columnHeaderChannelName;
        private System.Windows.Forms.ColumnHeader columnHeaderTimer;
        private System.Windows.Forms.ColumnHeader columnHeaderDumpingFile;
        private System.Windows.Forms.ColumnHeader columnHeaderFileSize;
        private System.Windows.Forms.ColumnHeader columnHeaderDelay;
        private System.Windows.Forms.ColumnHeader columnHeaderDumpStartedDate;
        private System.Windows.Forms.ColumnHeader columnHeaderPlaylistUrl;
        private System.Windows.Forms.ToolStripMenuItem miCopyPlaylistUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miRemoveChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miOpenChannelToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxSaveStreamInfo;
        private System.Windows.Forms.ToolStripMenuItem miCopyChannelNameToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderNewChunks;
        private System.Windows.Forms.ColumnHeader columnHeaderFirstChunkId;
        private System.Windows.Forms.ColumnHeader columnHeaderLostChunks;
        private System.Windows.Forms.ColumnHeader columnHeaderProcessedChunks;
        private System.Windows.Forms.ToolStripMenuItem miStopDumpToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxSaveChunksInfo;
        private System.Windows.Forms.CheckBox checkBoxStopIfPlaylistLost;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
