using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class cv : MonoBehaviour
{
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    public int port = 5068;
    public RawImage rawImage;
    private byte[] tryTextureBytes;
    private bool copiedData = false;

    private void Start()
    {
        InitTCP();
    }

    private void Update()
    {
        if (copiedData)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(tryTextureBytes);
            rawImage.texture = tex;
            copiedData = false;
        }
    }

    private void InitTCP()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        try
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            byte[] bytes = new byte[1024];

            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        // IFormatter formatter = new BinaryFormatter();
                        // byte[] data = (byte[])formatter.Deserialize(stream);
                        // Debug.Log(data.Length);

                        // IFormatter formatter = new BinaryFormatter();
                        // while (true)
                        // {
                        //     var data = (byte[])formatter.Deserialize(stream);
                        //     Debug.Log(data.Length);
                        // }
                        List<byte[]> result = new List<byte[]>();
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            result.Add(incomingData);

                            // tryTextureBytes = incomingData;

                            Debug.Log("done");
                            // string clientMessage = Encoding.ASCII.GetString(incomingData);
                            // Debug.Log(clientMessage);
                        }

                        int totalLength = 0;
                        foreach (byte[] block in result)
                        {
                            totalLength += block.Length;
                        }
                        byte[] resresult = new byte[totalLength];
                        int i = 0;
                        int offset = 0;
                        foreach (byte[] block in result)
                        {
                            Debug.Log(i++);
                            System.Buffer.BlockCopy(block, 0, resresult, offset, block.Length);
                            offset += block.Length;
                        }

                        tryTextureBytes = resresult;
                        copiedData = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}