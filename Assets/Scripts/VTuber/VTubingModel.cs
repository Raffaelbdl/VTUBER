using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public enum Axis { X, Y, Z }

public class VTubingModel : MonoBehaviour
{
    [field: SerializeField] public Transform spine { get; private set; }
    [field: SerializeField] public Transform neck { get; private set; }
    [field: SerializeField] public SkinnedMeshRenderer mouthRenderer { get; private set; }
    [field: SerializeField] public int mouthBlendshapeId { get; private set; }
    [field: SerializeField] public SkinnedMeshRenderer eyesRenderer { get; private set; }
    [field: SerializeField] public int rightEyeBlendshapeId { get; private set; }
    [field: SerializeField] public int leftEyeBlendshapeId { get; private set; }

    [field: SerializeField] public Axis spineAxis { get; private set; }
    [field: SerializeField] public Axis neckAxis { get; private set; }

    virtual public void SetMouthWeight(float value)
    {
        mouthRenderer.SetBlendShapeWeight(mouthBlendshapeId, value);
    }

    virtual public void SetRightEyeWeight(float value)
    {
        eyesRenderer.SetBlendShapeWeight(rightEyeBlendshapeId, value);
    }

    virtual public void SetLeftEyeWeight(float value)
    {
        eyesRenderer.SetBlendShapeWeight(leftEyeBlendshapeId, value);
    }

    virtual public void SetSpineRotation(float value) => SetTransformLocalRotation(spine, value, spineAxis);
    virtual public void SetNeckRotation(float value) => SetTransformLocalRotation(neck, value, neckAxis);

    private void SetTransformLocalRotation(Transform _transform, float value, Axis axis)
    {
        Vector3 newRot;

        switch (axis)
        {
            default:
                newRot = _transform.localEulerAngles.ChangeX(value); break;
            case Axis.Y:
                newRot = _transform.localEulerAngles.ChangeY(value); break;
            case Axis.Z:
                newRot = _transform.localEulerAngles.ChangeZ(value); break;
        }
        _transform.localEulerAngles = newRot;
    }
}
