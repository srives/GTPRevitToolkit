namespace GTP.UI
{
    partial class ConfirmationWindow
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
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ConfirmationDialog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.Location = new System.Drawing.Point(93, 74);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(174, 74);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // ConfirmationDialog
            // 
            this.ConfirmationDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ConfirmationDialog.Location = new System.Drawing.Point(0, 9);
            this.ConfirmationDialog.Name = "ConfirmationDialog";
            this.ConfirmationDialog.Size = new System.Drawing.Size(344, 52);
            this.ConfirmationDialog.TabIndex = 2;
            this.ConfirmationDialog.Text = "label1";
            this.ConfirmationDialog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ConfirmationDialog.Click += new System.EventHandler(this.ConfirmationDialog_Click);
            // 
            // ConfirmationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 104);
            this.ControlBox = false;
            this.Controls.Add(this.ConfirmationDialog);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Name = "ConfirmationWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfirmationWindow";
            this.Load += new System.EventHandler(this.ConfirmationWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label ConfirmationDialog;
    }
}