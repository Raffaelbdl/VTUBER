using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

[Serializable]
public class TcpConfig
{
    public string IPAddress = "127.0.0.1";
    public int port = 5065;
}

public class TCPServer
{
    private TcpConfig tcpConfig;
    public TCPServer(TcpConfig tcpConfig) => this.tcpConfig = tcpConfig;

    private TcpClient client;
    private TcpListener listener;

    private Thread receiveThread;

    private byte[] resultBytes;
    public byte[] dataReceived { get => resultBytes; }
    public Action onDataReceived;

    public void Initialize()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        listener = new TcpListener(IPAddress.Parse(tcpConfig.IPAddress), tcpConfig.port);
        listener.Start();

        byte[] data = new byte[1024];

        while (true)
        {
            using (client = listener.AcceptTcpClient())
            {
                using (NetworkStream stream = client.GetStream())
                {
                    List<byte[]> result = new List<byte[]>();

                    int length;
                    while ((length = stream.Read(data, 0, data.Length)) != 0)
                    {
                        byte[] incomingData = new byte[length];
                        Array.Copy(data, 0, incomingData, 0, length);
                        result.Add(incomingData);
                    }

                    resultBytes = Combine(result.ToArray());
                    onDataReceived?.Invoke();
                }
            }
        }
    }

    private byte[] Combine(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }
}
