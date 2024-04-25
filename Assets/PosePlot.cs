using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;


public class PosePlot : MonoBehaviour
{
    public MoveBody moveBody;

    private IReadOnlyList<NormalizedLandmark> _currentPoseLandmarkList;
    [SerializeField] private PointListAnnotation _landmarkListAnnotation;
    protected bool isStale = false;

    private void OnEnable()
    {
        VTuberSolution.onPoseLandmarksOutputSync += DrawNow;
        VTuberSolution.onPoseLandmarksOutputAsync += DrawPoseLandmarkListLater;
    }

    private void Start()
    {
        _landmarkListAnnotation.Fill(33);
        _landmarkListAnnotation.isMirrored = true;
    }

    private void LateUpdate()
    {
        if (isStale)
        {
            UpdatePos();
        }
    }

    public void UpdatePos()
    {
        if (_currentPoseLandmarkList == null) return;
        _landmarkListAnnotation.Draw(_currentPoseLandmarkList, false);

        moveBody.RotateBust(
            _currentPoseLandmarkList[11].Vector3(),
            _currentPoseLandmarkList[12].Vector3()
        );
    }

    public void DrawNow(IReadOnlyList<NormalizedLandmark> target)
    {
        _currentPoseLandmarkList = target;
        UpdatePos();
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
        Debug.Log("here");
        DrawPoseLandmarkListLater(poseLandmarkList?.Landmark);
    }

}
