using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.Holistic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The VTuberSolution relies on the HolisticTrackingGraph
/// 
/// This class only collects the results from the HolisticTrackingGraph
/// </summary>
public class VTuberSolution : ImageSourceSolution<HolisticTrackingGraph>
{
    public static UnityAction<NormalizedLandmarkList> onPoseLandmarksOutputSync;
    public static UnityAction<NormalizedLandmarkList> onPoseLandmarksOutputAsync;

    public static UnityAction<NormalizedLandmarkList> onFaceLandmarksOutputSync;
    public static UnityAction<NormalizedLandmarkList> onFaceLandmarksOutputAsync;


    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
        graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override void OnStartRun()
    {
        if (!runningMode.IsSynchronous())
        {
            graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
            graphRunner.OnFaceLandmarksOutput += OnFaceLandmarksOutput;
        }
    }


    protected override IEnumerator WaitForNextValue()
    {
        var task = graphRunner.WaitNextAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        var result = task.Result;
        onPoseLandmarksOutputSync?.Invoke(result.poseLandmarks);
        onFaceLandmarksOutputSync?.Invoke(result.faceLandmarks);
    }

    private void OnPoseLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
        var packet = eventArgs.packet;
        var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
        onPoseLandmarksOutputAsync?.Invoke(value);
    }


    private void OnFaceLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
        var packet = eventArgs.packet;
        var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
        onFaceLandmarksOutputAsync?.Invoke(value);
    }

}
