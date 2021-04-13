using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilityLibrary;
using Message = UtilityLibrary.Message;

namespace FileTransferClient {

    public partial class Login : Form {
        const string SERVER_IP = "127.0.0.1";
        const int PORT = 5000;
        public Dictionary<int, string> dirFiles = new Dictionary<int, string>();
        Socket client = null;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        public Login() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        // Client will start connecting to the server using the ip and port provided
        private void StartClient() {
            try {
                IPAddress ipAddress = IPAddress.Parse(SERVER_IP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, PORT);
                client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // send a message that will tell the server that this is a login request
                String message = ":log:";
                Send(client, message);
                sendDone.WaitOne();

                ReceiveDirectoryFiles(client);
                receiveDone.WaitOne();

                Console.WriteLine("done waiting: dirFiles size: {0}", dirFiles.Count);

                // hide the login then show the filepicker after successful login
                MethodInvoker inv = delegate {
                    this.Hide();
                    FilePicker filepicker = new FilePicker(dirFiles);
                    filepicker.Show();
                };
                Invoke(inv);

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, String data) {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket client = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Send {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveDirectoryFiles(Socket client) {
            try {
                StateObject state = new StateObject {
                    workSocket = client
                };

                // Begin receiving the data from the server.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveDirectoryFilesCallback), state);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveDirectoryFilesCallback(IAsyncResult ar) {
            try {
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);
                Console.WriteLine("bytesRead size {0}", bytesRead);

                if (bytesRead > 0) {
                    // There might be more data, so store the data received so far.
                    state.ms.Write(state.buffer, 0, state.buffer.Length);
                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, 
                        new AsyncCallback(ReceiveDirectoryFilesCallback), state);
                } else {
                    // all the data has arrived
                    Console.WriteLine("done receiving: ms size {0}", state.ms.Length);
                    //dirFiles = ConvertFromByteArray(state.ms.ToArray());
                    Message message = new UtilityLibrary.Message { Data = state.ms.ToArray() };
                    dirFiles = (Dictionary<int, string>) SerialLibrary.Deserialize(message);

                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }

                Console.WriteLine("ms.length {0} : {1}", state.ms.Length, state.buffer.Length);
            } catch (Exception e) {
                Console.WriteLine("ReceiveDirectoryFilesCallback: {0}", e.ToString());
            }
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
                Thread clientThread = new Thread(new ThreadStart(StartClient));
                clientThread.Start();
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

    /*
     * State object for receiving data from remote device
     */
    public class StateObject {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
        public MemoryStream ms = new MemoryStream();

        public byte[] bytesDownloaded;
        public string currentIdDownload;
    }
}
