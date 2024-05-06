using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanModel : VTubingModel
{
    [field: SerializeField] public SkinnedMeshRenderer eyebrowsRenderer { get; private set; }
    [field: SerializeField] public int rightEyebrowsBlendshapeId { get; private set; }

    // Cannot control eyes separately 
    public override void SetRightEyeWeight(float value)
    {
        base.SetRightEyeWeight(value);
        eyebrowsRenderer.SetBlendShapeWeight(rightEyebrowsBlendshapeId, value);
    }

    public override void SetLeftEyeWeight(float value) { }
}
