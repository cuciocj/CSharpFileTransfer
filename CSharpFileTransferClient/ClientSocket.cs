using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class StateObject {
    public Socket workSocket = null;
    public const int BufferSize = 1024;
    public byte[] buffer = new byte[BufferSize];
    public StringBuilder sb = new StringBuilder();
    public MemoryStream ms = new MemoryStream();
}

public class ClientSocket {

    public StateObject state { get; set; }

    public Socket socket;

    public string serverIp = String.Empty;

    public int port;

    public ClientSocket(string strAddress, int port) {
        IPAddress iPAddress = IPAddress.Parse(strAddress);
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
        socket = new Socket(iPAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
    }

}
