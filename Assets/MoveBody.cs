using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveBody : MonoBehaviour
{
    public Transform bustTransform;
    public Transform viewMiddle;
    public float targetAngle = 0f;

    public void RotateBust(Vector3 rightShoulder, Vector3 leftShoulder)
    {
        Vector3 middle = (rightShoulder - leftShoulder) / 2f;
        middle = Quaternion.Euler(0f, 0f, 90f) * middle;
        viewMiddle.localPosition = new Vector3(
            middle.x,
            middle.y,
            viewMiddle.localPosition.z);

        targetAngle = Vector3.SignedAngle(Vector3.up, middle, Vector3.back);
        // bustTransform.localEulerAngles = new Vector3(
        //     bustTransform.localEulerAngles.x,
        //     targetAngle,
        //     bustTransform.localEulerAngles.z
        // );
    }

    private IEnumerator Start()
    {
        while (true)
        {
            bustTransform.localEulerAngles = Vector3.RotateTowards(
                bustTransform.localEulerAngles,
                new Vector3(
                    bustTransform.localEulerAngles.x,
                    targetAngle,
                    bustTransform.localEulerAngles.z
                ),
                0.1f,
                0.1f
            );
            yield return new WaitForSeconds(0.1f);
        }
    }


}
