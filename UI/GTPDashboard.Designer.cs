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
            this.btnStop = new System.Windows.Forms.Button();
            this.cbMemory = new System.Windows.Forms.CheckBox();
            this.cbHighRefreshRate = new System.Windows.Forms.CheckBox();
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
            this.version = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabRun = new System.Windows.Forms.TabPage();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.grid = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Elements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Memory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabs.SuspendLayout();
            this.tabSettings.SuspendLayout();
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
            this.tabs.Size = new System.Drawing.Size(1170, 570);
            this.tabs.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.Black;
            this.tabSettings.Controls.Add(this.btnStop);
            this.tabSettings.Controls.Add(this.cbMemory);
            this.tabSettings.Controls.Add(this.cbHighRefreshRate);
            this.tabSettings.Controls.Add(this.panel1);
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.listBox1);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.lblncluded);
            this.tabSettings.Controls.Add(this.includedList);
            this.tabSettings.Controls.Add(this.version);
            this.tabSettings.Controls.Add(this.btnRun);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1162, 544);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnStop.Location = new System.Drawing.Point(1072, 508);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 21;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cbMemory
            // 
            this.cbMemory.AutoSize = true;
            this.cbMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbMemory.Location = new System.Drawing.Point(639, 510);
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
            this.cbHighRefreshRate.Location = new System.Drawing.Point(770, 510);
            this.cbHighRefreshRate.Name = "cbHighRefreshRate";
            this.cbHighRefreshRate.Size = new System.Drawing.Size(88, 17);
            this.cbHighRefreshRate.TabIndex = 19;
            this.cbHighRefreshRate.Text = "High Refresh";
            this.cbHighRefreshRate.UseVisualStyleBackColor = true;
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
            this.panel1.Location = new System.Drawing.Point(877, 316);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 184);
            this.panel1.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(22, 12);
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
            this.label3.Location = new System.Drawing.Point(893, 95);
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
            this.listBox1.Size = new System.Drawing.Size(399, 470);
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
            this.includedList.Size = new System.Drawing.Size(399, 470);
            this.includedList.TabIndex = 5;
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.ForeColor = System.Drawing.Color.Lime;
            this.version.Location = new System.Drawing.Point(14, 519);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(55, 13);
            this.version.TabIndex = 4;
            this.version.Text = "v2023.9.4";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.ForeColor = System.Drawing.Color.Lime;
            this.btnRun.Location = new System.Drawing.Point(977, 508);
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
            this.tabRun.Controls.Add(this.lblProgress);
            this.tabRun.Controls.Add(this.progress);
            this.tabRun.Controls.Add(this.grid);
            this.tabRun.Location = new System.Drawing.Point(4, 22);
            this.tabRun.Name = "tabRun";
            this.tabRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabRun.Size = new System.Drawing.Size(1162, 544);
            this.tabRun.TabIndex = 1;
            this.tabRun.Text = "Run";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.White;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblProgress.Location = new System.Drawing.Point(3, 503);
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
            this.TemplateId});
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.Size = new System.Drawing.Size(1162, 544);
            this.grid.TabIndex = 3;
            // 
            // Time
            // 
            this.Time.HeaderText = "Seconds";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Elements
            // 
            this.Elements.HeaderText = "Elements";
            this.Elements.Name = "Elements";
            this.Elements.ReadOnly = true;
            // 
            // Memory
            // 
            this.Memory.HeaderText = "Memory";
            this.Memory.Name = "Memory";
            this.Memory.ReadOnly = true;
            // 
            // TemplateId
            // 
            this.TemplateId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TemplateId.HeaderText = "TemplateId";
            this.TemplateId.Name = "TemplateId";
            this.TemplateId.ReadOnly = true;
            // 
            // GTPDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1158, 560);
            this.Controls.Add(this.tabs);
            this.Name = "GTPDashboard";
            this.Text = "GTPDashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.tabs.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
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
        private System.Windows.Forms.Label version;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Elements;
        private System.Windows.Forms.DataGridViewTextBoxColumn Memory;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateId;
    }
}