using System;
using System.Net.Sockets;
using System.Text;

namespace SocketClient {
    class Client {
        const int PORT = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args) {
            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient(SERVER_IP, PORT);
            NetworkStream nwStream = client.GetStream();
            String input = "";
            Console.WriteLine("Send Message");
            while(input != "!q") {
                Console.Write(": ");
                input = Console.ReadLine();
                byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(input);
                nwStream.Write(inputBytes, 0, inputBytes.Length);
            }
            nwStream.Close();
            client.Close();
        }
    }
}
