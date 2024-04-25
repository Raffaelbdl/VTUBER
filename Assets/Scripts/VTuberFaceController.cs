using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VTuberFaceController : MonoBehaviour
{
    // [SerializeField] private FaceAnnotations faceAnnotations;
    [SerializeField] private BlendshapeAnnotations faceAnnotations;
    [SerializeField] private SkinnedMeshRenderer faceRenderer;
    [SerializeField] private float mouthGapMultiplier = 1f;
    [SerializeField] private float maxMouth, maxRight, maxLeft;
    [SerializeField] private float minMouth, minRight, minLeft;

    private void Update()
    {
        faceRenderer.SetBlendShapeWeight(21, CalculateMouth());
        faceRenderer.SetBlendShapeWeight(14, CalculateRightEye());
        faceRenderer.SetBlendShapeWeight(15, CalculateLeftEye());
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
