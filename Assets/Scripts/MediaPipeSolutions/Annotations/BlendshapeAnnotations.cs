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
using UnityEngine.Assertions.Must;

public struct BlendShapes
{
    public float LeftBlink;
    public float RightBlink;
    public float OpenMouth;
}

public class BlendshapeAnnotations : MonoBehaviour
{
    [SerializeField] private MultiFaceLandmarkListAnnotation annotation;
    public BlendShapes blendShapes { private set; get; } = new BlendShapes();

    [SerializeField] private bool isMirrored = true;
    [SerializeField] private bool visualizeZ = true;
    private FaceLandmarkerResult _currentTarget;
    private bool isStale = false;

    private void OnEnable()
    {
        BlendshapeSolution.onFaceLandmarkerOutputSync += DrawNow;
        BlendshapeSolution.onFaceLandmarkerOutputAsync += DrawLater;
    }
    private void OnDisable()
    {
        BlendshapeSolution.onFaceLandmarkerOutputSync -= DrawNow;
        BlendshapeSolution.onFaceLandmarkerOutputAsync -= DrawLater;
    }

    private void Awake()
    {
        annotation.Fill(478);
        annotation.isMirrored = isMirrored;
    }

    private void LateUpdate()
    {
        if (isStale) SyncNow();
    }

    public void DrawNow(FaceLandmarkerResult target)
    {
        target.CloneTo(ref _currentTarget);
        SyncNow();
    }

    public void DrawLater(FaceLandmarkerResult target) => UpdateCurrentTarget(target);

    protected void UpdateCurrentTarget(FaceLandmarkerResult newTarget)
    {
        if (IsTargetChanged(newTarget, _currentTarget))
        {
            newTarget.CloneTo(ref _currentTarget);
            isStale = true;
        }
    }
    protected bool IsTargetChanged<TValue>(TValue newTarget, TValue currentTarget)
    {
        // It's assumed that target has not changed iff previous target and new target are both null.
        return currentTarget != null || newTarget != null;
    }

    protected void SyncNow()
    {
        isStale = false;
        try
        {
            if (_currentTarget.faceBlendshapes != null && _currentTarget.faceBlendshapes.Count > 0)
            {
                var classification = _currentTarget.faceBlendshapes[0];
                blendShapes = new BlendShapes()
                {
                    LeftBlink = classification.categories[9].score,
                    RightBlink = classification.categories[10].score,
                    OpenMouth = classification.categories[25].score
                };
            }
        }
        catch { }
        // annotation.Draw(_currentTarget.faceLandmarks, visualizeZ);
    }
}
