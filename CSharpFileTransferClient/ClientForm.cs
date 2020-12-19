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

namespace CSharpFileTransferClient {
    public partial class CSFTClient : Form {
        TcpClient client = null;
        const string SERVER_IP = "127.0.0.1";
        const int PORT = 5000;
        
        public CSFTClient() {
            InitializeComponent();
        }

        private void StartClient() {
            //TcpClient client = null;
            NetworkStream nwStream = null;
            
            do {
                try {
                    client = new TcpClient(SERVER_IP, PORT);

                    MethodInvoker inv = delegate {
                        this.btnStartClient.Enabled = true;
                        this.btnStartClient.Text = "Connected";
                        this.lblConnected.Text = "Connected to server";
                    };
                    Invoke(inv);

                    nwStream = client.GetStream();

                    String input = "ping";
                    do {
                        Console.WriteLine("[c]: {0}", input);
                        byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(input);
                        nwStream.Write(inputBytes, 0, inputBytes.Length);
                        Thread.Sleep(30000);
                    } while (true);
                    
                } catch (SocketException ex) {
                    Console.WriteLine("SocketException: {0}", ex);
                }
            } while (client == null);
        }

        private void sendMessage(String message) {
            NetworkStream nwStream = client.GetStream();
            Console.WriteLine("[c]: {0}", message);
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(message);
            nwStream.Write(inputBytes, 0, inputBytes.Length);
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            btnStartClient.Enabled = false;
            btnStartClient.Text = "Connecting...";
            Thread clientThread = new Thread(new ThreadStart(StartClient));
            clientThread.Start();
        }

        private void btnTerminate_Click(object sender, EventArgs e) {
            btnStartClient.Enabled = true;
            btnStartClient.Text = "Connect";
            sendMessage("!q");
        }
    }
}
