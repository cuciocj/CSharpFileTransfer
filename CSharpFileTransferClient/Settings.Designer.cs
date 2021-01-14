
namespace CSharpFileTransferClient {
    partial class Settings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numMaxSimDownload = new System.Windows.Forms.NumericUpDown();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.numMaxSizeAllowed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDownloadsDirectory = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSimDownload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSizeAllowed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max simultaneous download:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Max size allowed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Downloads folder:";
            // 
            // numMaxSimDownload
            // 
            this.numMaxSimDownload.Location = new System.Drawing.Point(237, 30);
            this.numMaxSimDownload.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMaxSimDownload.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxSimDownload.Name = "numMaxSimDownload";
            this.numMaxSimDownload.Size = new System.Drawing.Size(77, 20);
            this.numMaxSimDownload.TabIndex = 6;
            this.numMaxSimDownload.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Location = new System.Drawing.Point(237, 136);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(77, 23);
            this.btnApplySettings.TabIndex = 7;
            this.btnApplySettings.Text = "Apply";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // numMaxSizeAllowed
            // 
            this.numMaxSizeAllowed.Location = new System.Drawing.Point(237, 64);
            this.numMaxSizeAllowed.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.numMaxSizeAllowed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxSizeAllowed.Name = "numMaxSizeAllowed";
            this.numMaxSizeAllowed.Size = new System.Drawing.Size(48, 20);
            this.numMaxSizeAllowed.TabIndex = 8;
            this.numMaxSizeAllowed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(291, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "MB";
            // 
            // txtDownloadsDirectory
            // 
            this.txtDownloadsDirectory.Location = new System.Drawing.Point(139, 98);
            this.txtDownloadsDirectory.Name = "txtDownloadsDirectory";
            this.txtDownloadsDirectory.Size = new System.Drawing.Size(175, 20);
            this.txtDownloadsDirectory.TabIndex = 10;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 175);
            this.Controls.Add(this.txtDownloadsDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numMaxSizeAllowed);
            this.Controls.Add(this.btnApplySettings);
            this.Controls.Add(this.numMaxSimDownload);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "Settings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSimDownload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSizeAllowed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMaxSimDownload;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.NumericUpDown numMaxSizeAllowed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDownloadsDirectory;
    }
}