using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class VTubingConnect : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;

    private TCPClient client;
    private Process process;
    private Coroutine coroutine;
    private bool running = false;

    public void ToggleConnect(bool state)
    {
        if (state) StartConnect();
        else StopConnect();
    }

    public void StartConnect()
    {
        IEnumerator Initialize()
        {
#if UNITY_EDITOR
            string dir = Application.persistentDataPath;
#else
        string dir = Application.dataPath;
#endif
            string pythonPath = Path.Combine(dir, "dist/server/server");
            UnityEngine.Debug.Log(pythonPath);
            process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = pythonPath;
            process.StartInfo.Arguments = MakeArgs();
            process.Start();

            yield return new WaitForSeconds(5.0f);

            client = new TCPClient(new TcpConfig());

            running = true;
            yield return SendRenderTexture();
        }
        coroutine = StartCoroutine(Initialize());
    }

    public void StopConnect()
    {
        running = false;
        StopCoroutine(coroutine);
        if (Process.GetProcessesByName(process.ProcessName).Length > 0) process.Kill();
    }

    private void OnApplicationQuit() => StopConnect();

    WaitForSeconds waitForSeconds = new WaitForSeconds(1f / 50f);
    private IEnumerator SendRenderTexture()
    {
        while (running)
        {
            client.SendData(RenderTexture2Bytes());
            yield return waitForSeconds;
        }
    }

    private byte[] RenderTexture2Bytes()
    {

        int w = renderTexture.width;
        int h = renderTexture.height;
        Texture2D bufferTexture = new Texture2D(w, h, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture.active = renderTexture;

        bufferTexture.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        bufferTexture.Apply();

        RenderTexture.active = currentRT;

        byte[] result = bufferTexture.EncodeToPNG();
        return result;
    }

    private string MakeArgs()
    {
        string args = "";
        args += " --port " + VTubingUI.port.ToString();
        if (VTubingUI.vOutput == VOutput.OpenCV)
            args += " --no-virtual_cam";
        args += " --size " + renderTexture.width.ToString() + "x" + renderTexture.height.ToString();
        UnityEngine.Debug.Log(args);
        return args;
    }
}
