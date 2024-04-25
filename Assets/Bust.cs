using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.Holistic;
using UnityEngine;

public class Bust : ImageSourceSolution<HolisticTrackingGraph>
{
    public PosePlot posePlot;

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
        graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override void OnStartRun()
    {
        if (!runningMode.IsSynchronous())
        {
            // graphRunner.OnFaceLandmarksOutput += OnFaceLandmarksOutput;
            graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
        }
    }

    protected override IEnumerator WaitForNextValue()
    {
        var task = graphRunner.WaitNextAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        var result = task.Result;
        posePlot.DrawNow(result.poseLandmarks);
        Debug.Log(result.poseLandmarks);
    }

    private void OnPoseLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
        var packet = eventArgs.packet;
        var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
        posePlot.DrawPoseLandmarkListLater(value);
    }
}
