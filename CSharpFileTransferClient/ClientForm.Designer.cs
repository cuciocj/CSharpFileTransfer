
namespace CSharpFileTransferClient {
    partial class CSFTClient {
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
            this.btnStartClient = new System.Windows.Forms.Button();
            this.lblConnected = new System.Windows.Forms.Label();
            this.btnTerminate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartClient
            // 
            this.btnStartClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartClient.Location = new System.Drawing.Point(101, 182);
            this.btnStartClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(201, 49);
            this.btnStartClient.TabIndex = 2;
            this.btnStartClient.Text = "Connect";
            this.btnStartClient.UseVisualStyleBackColor = true;
            this.btnStartClient.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = true;
            this.lblConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnected.Location = new System.Drawing.Point(96, 154);
            this.lblConnected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(218, 25);
            this.lblConnected.TabIndex = 3;
            this.lblConnected.Text = "Not connected to server";
            // 
            // btnTerminate
            // 
            this.btnTerminate.Location = new System.Drawing.Point(337, 203);
            this.btnTerminate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTerminate.Name = "btnTerminate";
            this.btnTerminate.Size = new System.Drawing.Size(100, 28);
            this.btnTerminate.TabIndex = 4;
            this.btnTerminate.Text = "Terminate";
            this.btnTerminate.UseVisualStyleBackColor = true;
            this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
            // 
            // CSFTClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 629);
            this.Controls.Add(this.btnTerminate);
            this.Controls.Add(this.lblConnected);
            this.Controls.Add(this.btnStartClient);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CSFTClient";
            this.Text = "Client Form";
            this.Load += new System.EventHandler(this.CSFTClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.Label lblConnected;
        private System.Windows.Forms.Button btnTerminate;
    }
}

