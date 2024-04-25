using System.Collections;
using System.Collections.Generic;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.FaceDetection;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;
using Mediapipe.Tasks;
using UnityEngine.Events;
using Mediapipe;
using System;

public class BlendshapeSolution : VisionTaskApiRunner<FaceLandmarker>
{
    public static UnityAction<FaceLandmarkerResult> onFaceLandmarkerOutputSync;
    public static UnityAction<FaceLandmarkerResult> onFaceLandmarkerOutputAsync;

    private Mediapipe.Unity.Experimental.TextureFramePool _textureFramePool;
    public readonly FaceLandmarkDetectionConfig config = new FaceLandmarkDetectionConfig();

    public override void Stop()
    {
        base.Stop();
        _textureFramePool?.Dispose();
        _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
        yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

        var options = config.GetFaceLandmarkerOptions(config.RunningMode == Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnFaceLandmarkDetectionOutput : null);
        taskApi = FaceLandmarker.CreateFromOptions(options);
        var imageSource = ImageSourceProvider.ImageSource;

        yield return imageSource.Play();

        if (!imageSource.isPrepared)
        {
            Debug.LogError("Failed to start ImageSource, exiting...");
            yield break;
        }

        // Use RGBA32 as the input format.
        // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
        _textureFramePool = new Mediapipe.Unity.Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

        // NOTE: The screen will be resized later, keeping the aspect ratio.
        screen.Initialize(imageSource);

        var transformationOptions = imageSource.GetTransformationOptions();
        var flipHorizontally = transformationOptions.flipHorizontally;
        var flipVertically = transformationOptions.flipVertically;
        var imageProcessingOptions = new Mediapipe.Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

        UnityEngine.Rendering.AsyncGPUReadbackRequest req = default;
        var waitUntilReqDone = new WaitUntil(() => req.done);
        var result = FaceLandmarkerResult.Alloc(options.numFaces);

        while (true)
        {
            if (isPaused)
            {
                yield return new WaitWhile(() => isPaused);
            }

            if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            // Copy current image to TextureFrame
            req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
            yield return waitUntilReqDone;

            if (req.hasError)
            {
                Debug.LogError($"Failed to read texture from the image source, exiting...");
                break;
            }

            var image = textureFrame.BuildCPUImage();
            switch (taskApi.runningMode)
            {
                case Mediapipe.Tasks.Vision.Core.RunningMode.IMAGE:
                    if (taskApi.TryDetect(image, imageProcessingOptions, ref result))
                    {
                        onFaceLandmarkerOutputSync?.Invoke(result);
                    }
                    else
                    {
                        onFaceLandmarkerOutputSync?.Invoke(default);
                    }
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.VIDEO:
                    if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
                    {
                        onFaceLandmarkerOutputSync?.Invoke(result);
                    }
                    else
                    {
                        onFaceLandmarkerOutputSync?.Invoke(default);
                    }
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM:
                    taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
                    break;
            }

            textureFrame.Release();
        }
    }

    private void OnFaceLandmarkDetectionOutput(FaceLandmarkerResult result, Image image, long timestamp)
    {
        onFaceLandmarkerOutputAsync?.Invoke(result);
    }
}
