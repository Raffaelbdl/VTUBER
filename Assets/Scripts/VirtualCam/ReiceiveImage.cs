using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReiceiveImage : MonoBehaviour
{
    public RawImage rawImage;
    private TCPServer server;
    private TCPClient client;
    private bool isDataReceived;

    private void Start()
    {
        // server = new TCPServer(new TcpConfig());
        // server.Initialize();
        // server.onDataReceived += OnDataReceived;

        client = new TCPClient(new TcpConfig());
        StartCoroutine(SendCamera());
    }

    WaitForSeconds waitForSeconds = new WaitForSeconds(1f / 30f);
    private IEnumerator SendCamera()
    {
        while (true)
        {
            SendTxt();
            yield return waitForSeconds;
        }
    }

    private void Update()
    {
        if (isDataReceived) Load();
    }

    [ContextMenu("load")]
    private void OnDataReceived()
    {
        isDataReceived = true;
    }
    private void Load()
    {
        byte[] data = server.dataReceived;

        if (data == null) return;
        Debug.Log(data.Length);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(data);
        rawImage.texture = tex;

        isDataReceived = false;
    }

    [ContextMenu("send")]
    private void SendTxt()
    {
        client.SendData(TextureToBytes());
    }

    private byte[] TextureToBytes()
    {
        Texture tex = rawImage.texture;
        int w = tex.width;
        int h = tex.height;
        Texture2D tex2d = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture rT = new RenderTexture(h, w, 32);

        Graphics.Blit(tex, rT);
        RenderTexture.active = rT;

        tex2d.ReadPixels(new Rect(0, 0, rT.width, rT.height), 0, 0);
        tex2d.Apply();

        RenderTexture.active = currentRT;

        byte[] result = tex2d.EncodeToPNG();
        return result;
    }

}
