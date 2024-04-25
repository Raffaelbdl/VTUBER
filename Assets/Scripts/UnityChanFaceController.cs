using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanFace : MonoBehaviour
{
    [SerializeField] private FaceAnnotations faceAnnotations;
    [SerializeField] private SkinnedMeshRenderer faceRenderer;
    [SerializeField] private float mouthGapMultiplier = 1f;
    [SerializeField] private float eyeGapMin, eyeGapMax;

    private Transform mouthTop, mouthBottom;
    private Transform leftEyeTop, leftEyeBottom;
    private Transform rightEyeTop, rightEyeBottom;

    private void Start()
    {
        Debug.Log(faceAnnotations.transform.childCount);
        mouthTop = faceAnnotations.transform.GetChild(12);
        mouthBottom = faceAnnotations.transform.GetChild(15);

        leftEyeTop = faceAnnotations.transform.GetChild(470);
        leftEyeBottom = faceAnnotations.transform.GetChild(472);

        rightEyeTop = faceAnnotations.transform.GetChild(475);
        rightEyeBottom = faceAnnotations.transform.GetChild(477);
    }

    private void Update()
    {
        faceRenderer.SetBlendShapeWeight(21, CalculateMouthGap());
        faceRenderer.SetBlendShapeWeight(14, CalculateLeftEyeGap());
        faceRenderer.SetBlendShapeWeight(15, CalculateRightEyeGap());
    }

    private float CalculateMouthGap()
    {
        float gap = mouthGapMultiplier * (mouthTop.position - mouthBottom.position).y;
        return Mathf.Clamp(gap, 0f, 100f);
    }

    private float CalculateLeftEyeGap()
    {
        float gap = (leftEyeTop.position - leftEyeBottom.position).y;
        Debug.Log(gap);
        gap = 100 * Mathf.InverseLerp(eyeGapMin, eyeGapMax, gap);
        return Mathf.Clamp(gap, 0f, 100f);
    }

    private float CalculateRightEyeGap()
    {
        float gap = (rightEyeTop.position - rightEyeBottom.position).y;
        gap = 100 * Mathf.InverseLerp(eyeGapMin, eyeGapMax, gap);
        return Mathf.Clamp(gap, 0f, 100f);
    }
}
