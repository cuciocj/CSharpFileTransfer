using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSharpFileTransfer {
    public partial class CSFTServer : Form {
        const string SERVER_IP = "127.0.0.1";
        const int PORT = 5000;

        public CSFTServer() {
            InitializeComponent();
        }

        private void StartServer() {
            IPAddress ipAddress = IPAddress.Parse(SERVER_IP);
            TcpListener tcpListener = new TcpListener(ipAddress, PORT);
            tcpListener.Start();

            Byte[] bytes = new byte[256];
            String data = null;

            bool flag = true;
            while (flag) {
                Console.Write("[s]: Waiting for a connection... ");
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("[s]: Connected!");

                MethodInvoker inv = delegate {
                    this.lblClientConnected.Text = "Client connected";
                };
                Invoke(inv);

                NetworkStream nwStream = client.GetStream();

                int i;
                try {
                    while ((i = nwStream.Read(bytes, 0, bytes.Length)) != 0) {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("[s]: {0}", data);

                        if (data == "ping") {
                            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes("pong");
                            nwStream.Write(inputBytes, 0, inputBytes.Length);
                        } else if (data == "!q") {
                            tcpListener.Stop();
                            client.Close();
                            flag = false;
                        }
                    }
                } catch (ObjectDisposedException ex) {
                    Console.WriteLine("ObjectDisposedException: {0}", ex);
                } finally {
                    tcpListener.Stop();
                    MethodInvoker invf = delegate {
                        btnStart.Enabled = true;
                        btnStart.Text = "Start";
                    };
                    Invoke(invf);
                }
                
            }
            Console.WriteLine("[s]: Connection terminated by client");
        }

        private void btnStart_Click(object sender, EventArgs e) {
            btnStart.Enabled = false;
            btnStart.Text = "Listening...";
            Thread serverThread = new Thread(new ThreadStart(StartServer));
            serverThread.Start();
        }
    }
}
