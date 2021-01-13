using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FileTransferClient {
    
    public partial class FilePicker : Form {
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private DownloadableFiles files;

        const string DOWNLOADED_FILES_PATH = "D:\\ebook\\downloads\\";

        public FilePicker(Dictionary<int, string> dirFiles) {
            InitializeComponent();
            SetupDataGridView();
            InitializeTable(dirFiles);
        }

        public void SetupDataGridView() {
            filesDataGridView.ColumnCount = 3;
            // filesDataGridView.Size = new Size(350, 100);
            filesDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            filesDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            filesDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            filesDataGridView.GridColor = Color.Black;
            filesDataGridView.RowHeadersVisible = false;
            filesDataGridView.AllowUserToAddRows = false;

            filesDataGridView.Columns[0].Name = "No";
            filesDataGridView.Columns[0].ReadOnly = true;

            filesDataGridView.Columns[1].Name = "Filename";
            filesDataGridView.Columns[1].ReadOnly = true;

            filesDataGridView.Columns[2].Name = "File Size";
            filesDataGridView.Columns[2].ReadOnly = true;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn {
                Name = "action",
                HeaderText = "action",
                Width = 50,
                ReadOnly = false,
                TrueValue = true
            };
            filesDataGridView.Columns.Add(checkColumn);
            
        }

        private void AdjustDataGridViewHeight() {
            var height = 50;
            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                height += row.Height;
            }

            filesDataGridView.Height = height;
        }

        private void AdjustDataGridViewWidth() {
            filesDataGridView.AutoSizeColumnsMode = 
                DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private List<DataGridViewRow> GetCheckedRows() {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in filesDataGridView.Rows) {
                if (Convert.ToBoolean(row.Cells["action"].Value) == true) {
                    rows.Add(row);
                }
            }

            return rows;
        }

        private Socket InitializeClientSocket() {
            Socket client = null;
            try {
                IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 5000);
                client = new Socket(iPAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(iPEndPoint,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

            } catch (Exception e) {
                Console.WriteLine("RequestDownload error: {0}", e.ToString());
            }

            return client;
        }

        private void ConnectCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket) ar.AsyncState;
                client.EndConnect(ar);
                Console.WriteLine("[c] Socket connected to {0}",
                    client.RemoteEndPoint.ToString());
                connectDone.Set();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, String data) {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket) ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                sendDone.Set();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void Receive(Socket client, String data) {
            StateObject state = null;
            try {
                state = new StateObject {
                    currentIdDownload = data,
                    workSocket = client
                };

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            } catch (Exception e) {
                Console.WriteLine("Receive: error: {0}", e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar) {
            try {
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0) {
                    state.ms.Write(state.buffer, 0, state.buffer.Length);
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                } else {
                    state.bytesDownloaded = state.ms.ToArray();
                    receiveDone.Set();

                    // process bytesDownloaded
                    Console.WriteLine("ReceiveCallback: bytesDownloaded size: {0}", state.bytesDownloaded.Length);
                    Console.WriteLine("ReceiveCallback: currentDownload: {0}", state.currentIdDownload);
                    File.WriteAllBytes(DOWNLOADED_FILES_PATH + files.Get(state.currentIdDownload).name, state.bytesDownloaded);
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void InitializeTable(Dictionary<int, string> dirFiles) {
            files = new DownloadableFiles();

            foreach (var file in dirFiles) {
                string[] values = file.Value.Split(':');
                string filename = values[0];
                string size = values[1];

                ServerFile serverfile = new ServerFile {
                    id = file.Key,
                    name = filename,
                    size = long.Parse(size)
                };
                files.Add(serverfile);

                string[] row = {
                    file.Key + "",
                    serverfile.name,
                    ServerFile.GetReadableSize(serverfile.size)
                };
                filesDataGridView.Rows.Add(row);
            }
            AdjustDataGridViewHeight();
            AdjustDataGridViewWidth();
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            List<DataGridViewRow> checkedRows = GetCheckedRows();
            String ids = String.Empty;
            ids = string.Join("|", checkedRows.Select(row => row.Cells[0].Value.ToString()));
            Console.WriteLine("ids: {0}", ids);

            // loop for each checked row and send download request
            foreach (DataGridViewRow row in checkedRows) {
                string id = row.Cells[0].Value.ToString();
                Console.WriteLine("id to send to server {0}", id);

                Socket socket = InitializeClientSocket();
                Send(socket, id);
                sendDone.WaitOne();

                Receive(socket, id);
                receiveDone.WaitOne();
                Thread.Sleep(5000);
            }
        }
    }
}
