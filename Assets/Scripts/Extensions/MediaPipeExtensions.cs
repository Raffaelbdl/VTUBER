using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.Holistic;
using UnityEngine;
using UnityEngine.Events;

public static class NormalizedLandmarkExtension
{
    public static Vector3 Vector3(this NormalizedLandmark landmark)
    {
        return new Vector3(landmark.X, landmark.Y, landmark.Z);
    }
}
