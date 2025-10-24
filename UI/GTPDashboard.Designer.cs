namespace GTP.UI
{
    partial class GTPDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.RunNotepad = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.cbForceGCCollect = new System.Windows.Forms.CheckBox();
            this.cbMemory = new System.Windows.Forms.CheckBox();
            this.cbHighRefreshRate = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbComplexSearch = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbTolerance = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.udProgressInterval = new System.Windows.Forms.NumericUpDown();
            this.tbStop = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblncluded = new System.Windows.Forms.Label();
            this.includedList = new System.Windows.Forms.ListBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabRun = new System.Windows.Forms.TabPage();
            this.rtfExtra = new System.Windows.Forms.RichTextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.grid = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Elements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Memory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parameters = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleElementIDs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabs.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udProgressInterval)).BeginInit();
            this.tabRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabSettings);
            this.tabs.Controls.Add(this.tabRun);
            this.tabs.Location = new System.Drawing.Point(-6, -3);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1494, 570);
            this.tabs.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.Black;
            this.tabSettings.Controls.Add(this.RunNotepad);
            this.tabSettings.Controls.Add(this.panel3);
            this.tabSettings.Controls.Add(this.panel2);
            this.tabSettings.Controls.Add(this.btnStop);
            this.tabSettings.Controls.Add(this.panel1);
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.listBox1);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.lblncluded);
            this.tabSettings.Controls.Add(this.includedList);
            this.tabSettings.Controls.Add(this.btnRun);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1486, 544);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            // 
            // RunNotepad
            // 
            this.RunNotepad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RunNotepad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RunNotepad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.RunNotepad.Location = new System.Drawing.Point(36, 506);
            this.RunNotepad.Name = "RunNotepad";
            this.RunNotepad.Size = new System.Drawing.Size(156, 23);
            this.RunNotepad.TabIndex = 22;
            this.RunNotepad.Text = "Notepad Stratus Settings";
            this.RunNotepad.UseVisualStyleBackColor = true;
            this.RunNotepad.Click += new System.EventHandler(this.RunNotepad_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.cbForceGCCollect);
            this.panel3.Controls.Add(this.cbMemory);
            this.panel3.Controls.Add(this.cbHighRefreshRate);
            this.panel3.Location = new System.Drawing.Point(1163, 422);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(283, 71);
            this.panel3.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Lime;
            this.label13.Location = new System.Drawing.Point(4, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 20);
            this.label13.TabIndex = 13;
            this.label13.Text = "Run Options";
            // 
            // cbForceGCCollect
            // 
            this.cbForceGCCollect.AutoSize = true;
            this.cbForceGCCollect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbForceGCCollect.Location = new System.Drawing.Point(112, 13);
            this.cbForceGCCollect.Name = "cbForceGCCollect";
            this.cbForceGCCollect.Size = new System.Drawing.Size(146, 17);
            this.cbForceGCCollect.TabIndex = 22;
            this.cbForceGCCollect.Text = "Force Garbage Collection";
            this.cbForceGCCollect.UseVisualStyleBackColor = true;
            // 
            // cbMemory
            // 
            this.cbMemory.AutoSize = true;
            this.cbMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbMemory.Location = new System.Drawing.Point(12, 38);
            this.cbMemory.Name = "cbMemory";
            this.cbMemory.Size = new System.Drawing.Size(125, 17);
            this.cbMemory.TabIndex = 20;
            this.cbMemory.Text = "Gather Memory Stats";
            this.cbMemory.UseVisualStyleBackColor = true;
            // 
            // cbHighRefreshRate
            // 
            this.cbHighRefreshRate.AutoSize = true;
            this.cbHighRefreshRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbHighRefreshRate.Location = new System.Drawing.Point(143, 38);
            this.cbHighRefreshRate.Name = "cbHighRefreshRate";
            this.cbHighRefreshRate.Size = new System.Drawing.Size(123, 17);
            this.cbHighRefreshRate.TabIndex = 19;
            this.cbHighRefreshRate.Text = "More Responsive UI";
            this.cbHighRefreshRate.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbComplexSearch);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.tbTolerance);
            this.panel2.Location = new System.Drawing.Point(1163, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 184);
            this.panel2.TabIndex = 19;
            // 
            // cbComplexSearch
            // 
            this.cbComplexSearch.AutoSize = true;
            this.cbComplexSearch.ForeColor = System.Drawing.Color.GreenYellow;
            this.cbComplexSearch.Location = new System.Drawing.Point(33, 154);
            this.cbComplexSearch.Name = "cbComplexSearch";
            this.cbComplexSearch.Size = new System.Drawing.Size(217, 17);
            this.cbComplexSearch.TabIndex = 20;
            this.cbComplexSearch.Text = "Search Only for Complex Parts/Elements";
            this.cbComplexSearch.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label12.Location = new System.Drawing.Point(4, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(261, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "You specify the # of materials that constitute complex.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label11.Location = new System.Drawing.Point(4, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(271, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "A complex object is any element made of many materials";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Lime;
            this.label8.Location = new System.Drawing.Point(4, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "Complex Objects";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Lime;
            this.label10.Location = new System.Drawing.Point(30, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(212, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "# of Materials for an Element to be Complex";
            // 
            // tbTolerance
            // 
            this.tbTolerance.Location = new System.Drawing.Point(33, 114);
            this.tbTolerance.Name = "tbTolerance";
            this.tbTolerance.Size = new System.Drawing.Size(75, 20);
            this.tbTolerance.TabIndex = 14;
            this.tbTolerance.Text = "15";
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnStop.Location = new System.Drawing.Point(1381, 506);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 21;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.udProgressInterval);
            this.panel1.Controls.Add(this.tbStop);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tbStart);
            this.panel1.Location = new System.Drawing.Point(1163, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 184);
            this.panel1.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(4, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(229, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Range of Elements to Examine";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Lime;
            this.label7.Location = new System.Drawing.Point(172, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Stop (-1 == max)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Lime;
            this.label6.Location = new System.Drawing.Point(30, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Start (-1 == min)";
            // 
            // udProgressInterval
            // 
            this.udProgressInterval.BackColor = System.Drawing.Color.Black;
            this.udProgressInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.udProgressInterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.udProgressInterval.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udProgressInterval.Location = new System.Drawing.Point(33, 139);
            this.udProgressInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udProgressInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udProgressInterval.Name = "udProgressInterval";
            this.udProgressInterval.Size = new System.Drawing.Size(217, 20);
            this.udProgressInterval.TabIndex = 11;
            this.udProgressInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // tbStop
            // 
            this.tbStop.Location = new System.Drawing.Point(175, 72);
            this.tbStop.Name = "tbStop";
            this.tbStop.Size = new System.Drawing.Size(75, 20);
            this.tbStop.TabIndex = 15;
            this.tbStop.Text = "-1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(30, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Step Size";
            // 
            // tbStart
            // 
            this.tbStart.Location = new System.Drawing.Point(33, 72);
            this.tbStart.Name = "tbStart";
            this.tbStart.Size = new System.Drawing.Size(75, 20);
            this.tbStart.TabIndex = 14;
            this.tbStart.Text = "-1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(473, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Package Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(893, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Item Number";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(459, 30);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(997, 470);
            this.listBox1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(441, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Excluded Properties";
            // 
            // lblncluded
            // 
            this.lblncluded.AutoSize = true;
            this.lblncluded.ForeColor = System.Drawing.Color.Lime;
            this.lblncluded.Location = new System.Drawing.Point(17, 8);
            this.lblncluded.Name = "lblncluded";
            this.lblncluded.Size = new System.Drawing.Size(98, 13);
            this.lblncluded.TabIndex = 6;
            this.lblncluded.Text = "Included Properties";
            // 
            // includedList
            // 
            this.includedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.includedList.BackColor = System.Drawing.SystemColors.InfoText;
            this.includedList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.includedList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.includedList.FormattingEnabled = true;
            this.includedList.Location = new System.Drawing.Point(36, 30);
            this.includedList.Name = "includedList";
            this.includedList.Size = new System.Drawing.Size(723, 470);
            this.includedList.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.ForeColor = System.Drawing.Color.Lime;
            this.btnRun.Location = new System.Drawing.Point(1300, 506);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tabRun
            // 
            this.tabRun.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabRun.Controls.Add(this.rtfExtra);
            this.tabRun.Controls.Add(this.lblProgress);
            this.tabRun.Controls.Add(this.progress);
            this.tabRun.Controls.Add(this.grid);
            this.tabRun.Location = new System.Drawing.Point(4, 22);
            this.tabRun.Name = "tabRun";
            this.tabRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabRun.Size = new System.Drawing.Size(1486, 544);
            this.tabRun.TabIndex = 1;
            this.tabRun.Text = "Run";
            // 
            // rtfExtra
            // 
            this.rtfExtra.BackColor = System.Drawing.SystemColors.MenuText;
            this.rtfExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfExtra.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfExtra.ForeColor = System.Drawing.Color.Lime;
            this.rtfExtra.Location = new System.Drawing.Point(1161, 0);
            this.rtfExtra.Name = "rtfExtra";
            this.rtfExtra.Size = new System.Drawing.Size(322, 541);
            this.rtfExtra.TabIndex = 4;
            this.rtfExtra.Text = "";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.White;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblProgress.Location = new System.Drawing.Point(5, 504);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(10, 16);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = ".";
            // 
            // progress
            // 
            this.progress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progress.Location = new System.Drawing.Point(-4, 523);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(1166, 18);
            this.progress.TabIndex = 1;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Elements,
            this.Memory,
            this.Parameters,
            this.SampleElementIDs,
            this.TemplateId});
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.Size = new System.Drawing.Size(1162, 544);
            this.grid.TabIndex = 3;
            this.grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
            // 
            // Time
            // 
            this.Time.HeaderText = "Seconds Processing";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Elements
            // 
            this.Elements.HeaderText = "Elements Processed";
            this.Elements.Name = "Elements";
            this.Elements.ReadOnly = true;
            // 
            // Memory
            // 
            this.Memory.HeaderText = "Memory";
            this.Memory.Name = "Memory";
            this.Memory.ReadOnly = true;
            // 
            // Parameters
            // 
            this.Parameters.HeaderText = "Avg # Parameters per Element";
            this.Parameters.Name = "Parameters";
            this.Parameters.ReadOnly = true;
            // 
            // SampleElementIDs
            // 
            this.SampleElementIDs.HeaderText = "Element Revit Ids";
            this.SampleElementIDs.Name = "SampleElementIDs";
            this.SampleElementIDs.ReadOnly = true;
            // 
            // TemplateId
            // 
            this.TemplateId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TemplateId.HeaderText = "Element TemplateId";
            this.TemplateId.Name = "TemplateId";
            this.TemplateId.ReadOnly = true;
            // 
            // GTPDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1482, 560);
            this.Controls.Add(this.tabs);
            this.Name = "GTPDashboard";
            this.Text = "GTPDashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.tabs.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udProgressInterval)).EndInit();
            this.tabRun.ResumeLayout(false);
            this.tabRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TabPage tabRun;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblncluded;
        private System.Windows.Forms.ListBox includedList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown udProgressInterval;
        private System.Windows.Forms.TextBox tbStop;
        private System.Windows.Forms.TextBox tbStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbHighRefreshRate;
        private System.Windows.Forms.CheckBox cbMemory;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox cbForceGCCollect;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Elements;
        private System.Windows.Forms.DataGridViewTextBoxColumn Memory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameters;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleElementIDs;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateId;
        private System.Windows.Forms.RichTextBox rtfExtra;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbTolerance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbComplexSearch;
        private System.Windows.Forms.Button RunNotepad;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
    }
}