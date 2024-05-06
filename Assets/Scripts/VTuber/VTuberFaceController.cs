using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VTuberFaceController : MonoBehaviour
{
    [SerializeField] private BlendshapeAnnotations faceAnnotations;
    [SerializeField] private VTubingModel model;
    [SerializeField] private float mouthGapMultiplier = 1f;
    [SerializeField] private float maxMouth, maxRight, maxLeft;
    [SerializeField] private float minMouth, minRight, minLeft;

    private void Update()
    {
        model.SetMouthWeight(CalculateMouth());
        model.SetRightEyeWeight(CalculateRightEye());
        model.SetLeftEyeWeight(CalculateLeftEye());
    }

    private float CalculateMouth()
    {
        float gap = faceAnnotations.blendShapes.OpenMouth;
        return 100f * Mathf.InverseLerp(minMouth, maxMouth, gap);
    }
    private float CalculateRightEye()
    {
        float gap = faceAnnotations.blendShapes.RightBlink;
        return 100f * Mathf.InverseLerp(minRight, maxRight, gap);
    }
    private float CalculateLeftEye()
    {
        float gap = faceAnnotations.blendShapes.LeftBlink;
        return 100f * Mathf.InverseLerp(minLeft, maxLeft, gap);
    }
}
