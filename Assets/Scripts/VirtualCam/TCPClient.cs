using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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
        // byte[] msg = Encoding.ASCII.GetBytes("Hello there!");

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
