﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UtilityLibrary;

namespace FileTransferServer {
    public partial class CSFTServer : Form {
        const string DIRECTORY_PATH = "D:\\ebook\\";
        const string SERVER_IP = "127.0.0.1";
        const int PORT = 5000;

        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public CSFTServer() {
            InitializeComponent();
        }

        public class StateObject {
            public const int BufferSize = 1024; // Size of receive buffer.
            public byte[] buffer = new byte[BufferSize]; // Receive buffer.
            public StringBuilder sb = new StringBuilder(); // Received data string.
            public Socket workSocket = null; // Client socket.
        }

        public void StartListening() {
            IPAddress ipAddress = IPAddress.Parse(SERVER_IP);
            IPEndPoint localEndpoint = new IPEndPoint(ipAddress, PORT);

            // Create a TCP/IP socket.
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try {
                listener.Bind(localEndpoint);
                listener.Listen(100);

                while (true) {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("[s]: Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            } catch (Exception e) {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar) {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar) {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0) {
                String data = Encoding.ASCII.GetString(state.buffer, 0,
                    bytesRead);

                if (data != "!quit") {
                    Console.WriteLine("[s] Data : {0}", data);

                    // After logging in, 
                    // send list of file inside the DIRECTORY_PATH
                    if (data.StartsWith(":log:")) {

                        if (Directory.Exists(DIRECTORY_PATH)) {
                            Dictionary<int, string> dirFiles = new Dictionary<int, string>();

                            int count = 1;
                            foreach (string filename in Directory.GetFiles(DIRECTORY_PATH)) {
                                FileInfo fileInfo = new FileInfo(filename);
                                CSharpFileTransfer.File file = new CSharpFileTransfer.File {
                                    name = Path.GetFileName(filename),
                                    size = fileInfo.Length
                                };
                                dirFiles.Add(count, file.name + ":" + file.size);
                                count++;
                            }

                            UtilityLibrary.Message message = SerialLibrary.Serialize(dirFiles);
                            Send(handler, message);
                        }

                    }

                    //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReadCallback), state);
                } else {
                    Console.WriteLine("[s] Quit Signal : {0}", data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
        }

        public void Send(Socket handler, UtilityLibrary.Message message) {
            // Begin sending the data to the remote device.
            handler.BeginSend(message.Data, 0, message.Data.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("SendCallback: Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            } catch (Exception e) {
                Console.WriteLine("SendCallback: {0}", e.ToString());
            }
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

                        //if (data == "ping") {
                        //    byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes("pong");
                        //    nwStream.Write(inputBytes, 0, inputBytes.Length);
                        //} else if (data == "!q") {
                        //    tcpListener.Stop();
                        //    client.Close();
                        //    flag = false;
                        //}
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

        private byte[] ConvertToByteArray(Object obj) {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, obj);

            return mStream.ToArray();
        }

        private Dictionary<int, CSharpFileTransfer.File> ConvertFromByteArray(byte[] byteArray) {
            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();

            mStream.Write(byteArray, 0, byteArray.Length);
            mStream.Position = 0;

            return binFormatter.Deserialize(mStream) as Dictionary<int, CSharpFileTransfer.File>;
        }

        private void btnStart_Click(object sender, EventArgs e) {
            btnStart.Enabled = false;
            btnStart.Text = "Listening...";
            // Thread serverThread = new Thread(new ThreadStart(StartServer));
            Thread serverThread = new Thread(new ThreadStart(StartListening));
            serverThread.Start();
        }
    }
}
