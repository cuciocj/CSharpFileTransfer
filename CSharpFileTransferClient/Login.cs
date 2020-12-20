using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferClient {
    public partial class Login : Form {
        TcpClient client = null;
        const string SERVER_IP = "127.0.0.1";
        const int PORT = 5000;
        public Login() {
            InitializeComponent();
        }

        /* 
         * Connect and send login request to server
         */
        private void StartClient() {
            //TcpClient client = null;
            NetworkStream nwStream = null;

            do {
                try {
                    client = new TcpClient(SERVER_IP, PORT);

                    MethodInvoker inv = delegate {
                    };
                    Invoke(inv);

                    nwStream = client.GetStream();

                    String input = "ping";
                    String credentials = txtUserName.Text + ',' + txtPassword.Text;
                    do {
                        Console.WriteLine("[c]: {0}", input);
                        // TODO: send username and password to server
                        byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(credentials);
                        nwStream.Write(inputBytes, 0, inputBytes.Length);
                        Thread.Sleep(30000);
                    } while (true);

                } catch (SocketException ex) {
                    Console.WriteLine("SocketException: {0}", ex);
                }
            } while (client == null);
        }

        /*
         * Process login
         */
        private void ProcessLogin() {
            int result;

            this.disableControl();

            // validate inputs
            result = this.ValidateInputs();

            if (result != 1) {
                this.enableControl();
            } else {
                this.StartClient();
            }
        }

        /*
         * Validate username and input, and display error if empty
         */
        private int ValidateInputs() {
            if (txtUserName.TextLength == 0) {
                errInput.SetError(txtUserName, "Please input username.");
                txtUserName.Focus();
                return - 1;
            } else if (txtPassword.TextLength == 0) {
                errInput.SetError(txtPassword, "Please input password.");
                txtPassword.Focus();
                return -1;
            }
            else {
                return 1;
            }
        }

        /*
         * Login button click
         */
        private void btnLogin_Click(object sender, EventArgs e) {
            this.ProcessLogin();
        }

        /*
         * Set control properties to default
         */
        private void enableControl() {
            this.btnLogin.Enabled = true;
            this.btnLogin.Text = "Login";
        }

        /*
         * Disable controls
         */
        private void disableControl() {
            this.btnLogin.Enabled = false;
            this.btnLogin.Text = "Logging in";
            errInput.Clear();
        }
    }
}
