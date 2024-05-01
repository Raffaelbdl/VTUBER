using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum VOutput { OpenCV, VirtualCam }

public class VTubingUI : MonoBehaviour
{
    public static VOutput vOutput { get; private set; } = VOutput.OpenCV;
    [SerializeField] TMP_Dropdown outputDropdown;
    public static Vector2 resolution { get; private set; } = new Vector2(512, 512);
    public static int port { get; private set; } = 5065;

    private void Start()
    {
        outputDropdown.ClearOptions();
        outputDropdown.AddOptions(Enum.GetNames(typeof(VOutput)).ToList());
    }

    public void OnOutputChanged(int value) => vOutput = (VOutput)Enum.ToObject(typeof(VOutput), value);
    public void OnWidthChanged(string width) => resolution = new Vector2(int.Parse(width), resolution.y);
    public void OnHeightChanged(string height) => resolution = new Vector2(resolution.x, int.Parse(height));
    public void OnPortChanged(string value) => port = int.Parse(value);
}
