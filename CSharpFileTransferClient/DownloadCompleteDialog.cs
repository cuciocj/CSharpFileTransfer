using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpFileTransferClient {
    public partial class DownloadCompleteDialog : Form {
        public DownloadCompleteDialog() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnOpenDownloads_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(Settings.DOWNLOADS_DIRECTORY);
        }
    }
}
