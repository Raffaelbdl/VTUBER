using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VTubingModel : MonoBehaviour
{
    [field: SerializeField] public Transform spine { get; private set; }
    [field: SerializeField] public Transform neck { get; private set; }
    [field: SerializeField] public SkinnedMeshRenderer meshRenderer { get; private set; }
    [field: SerializeField] public int mouthBlendshapeId { get; private set; }
    [field: SerializeField] public int rightEyeBlendshapeId { get; private set; }
    [field: SerializeField] public int leftEyeBlendshapeId { get; private set; }
}
