using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class VTuberUpperBody : MonoBehaviour
{
    [SerializeField] private PoseAnnotations poseAnnotations;

    // bust movements
    [SerializeField] private Transform spine;
    [SerializeField] private Transform neck;
    private Transform rightShoulder, leftShoulder, face;

    private void Start()
    {
        rightShoulder = poseAnnotations.transform.GetChild(11);
        leftShoulder = poseAnnotations.transform.GetChild(12);
        face = poseAnnotations.transform.GetChild(0);
    }

    private void Update()
    {
        spine.localEulerAngles = CalculateSpineRotation(); // with Z fixed
        neck.localEulerAngles = CalculateNeckRotation(); // with Z fixed
    }

    private Vector3 CalculateSpineRotation()
    {
        Vector3 middleShoulder = (rightShoulder.position + leftShoulder.position) / 2f;
        float angle = Vector3.SignedAngle(Vector3.right, middleShoulder, Vector3.forward);
        return spine.localEulerAngles.ChangeZ(angle + 90f);
    }

    private Vector3 CalculateNeckRotation()
    {
        Vector3 middleShoulder = (rightShoulder.position + leftShoulder.position) / 2f;
        Vector3 neckDirection = face.position - middleShoulder;
        float angle = Vector3.SignedAngle(Vector3.up, neckDirection, Vector3.back);
        return neck.localEulerAngles.ChangeZ(angle);
    }
}
