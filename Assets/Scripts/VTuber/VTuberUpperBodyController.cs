using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class VTuberUpperBody : MonoBehaviour
{
    [SerializeField] private PoseAnnotations poseAnnotations;

    [SerializeField] VTubingModel model;
    private Transform rightShoulder, leftShoulder, face;

    private void Start()
    {
        rightShoulder = poseAnnotations.transform.GetChild(11);
        leftShoulder = poseAnnotations.transform.GetChild(12);
        face = poseAnnotations.transform.GetChild(0);
    }

    private void Update()
    {
        model.spine.localEulerAngles = CalculateSpineRotation(); // with Z fixed
        model.neck.localEulerAngles = CalculateNeckRotation(); // with Z fixed
    }

    private Vector3 CalculateSpineRotation()
    {
        Vector3 shoulderLine = rightShoulder.position - leftShoulder.position;
        Vector3 shoulderOrth = new Vector3(shoulderLine.y, -shoulderLine.x, 0f);
        float angle = Vector3.SignedAngle(Vector3.up, shoulderOrth, Vector3.back);
        return model.spine.localEulerAngles.ChangeZ(angle);
    }

    private Vector3 CalculateNeckRotation()
    {
        Vector3 middleShoulder = (rightShoulder.position + leftShoulder.position) / 2f;
        Vector3 neckDirection = face.position - middleShoulder;
        float angle = Vector3.SignedAngle(Vector3.up, neckDirection, Vector3.back);
        return model.neck.localEulerAngles.ChangeZ(angle);
    }
}
