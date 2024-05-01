using System.Net.Sockets;
using UnityEngine;

public class TCPClient
{
    private TcpConfig tcpConfig;
    public TCPClient(TcpConfig tcpConfig) => this.tcpConfig = tcpConfig;

    private TcpClient client;
    private TcpListener listener;

    public void SendData(byte[] data)
    {
        client = new TcpClient(tcpConfig.IPAddress, tcpConfig.port);

        using (NetworkStream stream = client.GetStream())
        {
            int offset = 0;
            while (offset < data.Length)
            {
                int length = Mathf.Min(1024, data.Length - offset);
                stream.Write(data, offset, length);
                offset += length;
            }
        }
    }
}
