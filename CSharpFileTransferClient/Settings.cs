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

    public partial class Settings : Form {

        public static int MAX_DOWNLOAD = 5;

        public static int MAX_SIZE_ALLOWED = 5000;

        public static string DOWNLOADS_DIRECTORY = "C:\\Users\\cucio\\Documents\\#PERSONAL\\ebooks\\Downloads";

        public Settings() {
            InitializeComponent();
            InitializeDefaultSettings();
        }

        private void InitializeDefaultSettings() {
            numMaxSimDownload.Value = Convert.ToDecimal(MAX_DOWNLOAD);
            numMaxSizeAllowed.Value = Convert.ToDecimal(MAX_SIZE_ALLOWED);
            txtDownloadsDirectory.Text = DOWNLOADS_DIRECTORY;
        }

        private void btnApplySettings_Click(object sender, EventArgs e) {
            MAX_DOWNLOAD = Convert.ToInt32(numMaxSimDownload.Value);
            MAX_SIZE_ALLOWED = Convert.ToInt32(numMaxSizeAllowed.Value);
            DOWNLOADS_DIRECTORY = txtDownloadsDirectory.Text;

            this.Close();
        }
    }
}
