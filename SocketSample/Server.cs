using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketSample {
    class Server {
        const int PORT = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args) {
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener tcpListener = new TcpListener(localAdd, PORT);
            Console.WriteLine("Listening...");
            tcpListener.Start();

            Byte[] bytes = new Byte[256];
            String data = null;

            bool flag = true;
            while (flag) {
                //---incoming client connected---
                TcpClient client = tcpListener.AcceptTcpClient();   // <-- thread will wait here until client connects
                NetworkStream nwStream = client.GetStream();

                int i;
                while((i = nwStream.Read(bytes, 0, bytes.Length)) != 0) {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);
                    if(data == "!q") {
                        flag = false;
                        break;
                    }
                }

                client.Close();
            }

            Console.WriteLine("[s]: Connection terminated by client");
        }
    }
}
