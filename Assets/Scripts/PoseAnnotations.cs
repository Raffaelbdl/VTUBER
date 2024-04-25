using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.Holistic;
using UnityEngine;
using UnityEngine.Events;

public class PoseAnnotations : MonoBehaviour
{
    private IReadOnlyList<NormalizedLandmark> _currentPoseLandmarkList;

    [SerializeField] private PointListAnnotation _landmarkListAnnotation;
    [SerializeField] private bool isMirrored = true;
    [SerializeField] private bool visualizeZ = true;
    private bool isStale = false;

    private void OnEnable()
    {
        VTuberSolution.onPoseLandmarksOutputSync += DrawNow;
        VTuberSolution.onPoseLandmarksOutputAsync += DrawPoseLandmarkListLater;
    }
    private void OnDisable()
    {
        VTuberSolution.onPoseLandmarksOutputSync -= DrawNow;
        VTuberSolution.onPoseLandmarksOutputAsync -= DrawPoseLandmarkListLater;
    }

    private void Awake()
    {
        _landmarkListAnnotation.Fill(33);
        _landmarkListAnnotation.isMirrored = isMirrored;
    }

    private void LateUpdate()
    {
        if (isStale) SyncNow();
    }

    public void SyncNow()
    {
        if (_currentPoseLandmarkList == null) return;
        _landmarkListAnnotation.Draw(_currentPoseLandmarkList, visualizeZ);
    }

    public void DrawNow(IReadOnlyList<NormalizedLandmark> target)
    {
        _currentPoseLandmarkList = target;
        SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList target)
    {
        DrawNow(target?.Landmark);
    }

    protected void UpdateCurrentTarget<TValue>(TValue newTarget, ref TValue currentTarget)
    {
        if (IsTargetChanged(newTarget, currentTarget))
        {
            currentTarget = newTarget;
            isStale = true;
        }
    }

    protected bool IsTargetChanged<TValue>(TValue newTarget, TValue currentTarget)
    {
        // It's assumed that target has not changed iff previous target and new target are both null.
        return currentTarget != null || newTarget != null;
    }

    public void DrawPoseLandmarkListLater(IReadOnlyList<NormalizedLandmark> poseLandmarkList)
    {
        UpdateCurrentTarget(poseLandmarkList, ref _currentPoseLandmarkList);
    }

    public void DrawPoseLandmarkListLater(NormalizedLandmarkList poseLandmarkList)
    {
        DrawPoseLandmarkListLater(poseLandmarkList?.Landmark);
    }
}
