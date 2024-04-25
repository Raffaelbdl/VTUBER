using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.Holistic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using Mediapipe.Unity.CoordinateSystem;


public class Bibendum : MonoBehaviour
{
    [SerializeField] private PoseAnnotations poseAnnotations;
    private List<Transform> spheres;

    private void Start()
    {
        spheres = new List<Transform>();
        for (int i = 0; i < poseAnnotations.transform.childCount; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(transform);
            sphere.transform.localScale = 100 * Vector3.one;
            spheres.Add(sphere.transform);
        }
    }

    private void Update()
    {
        for (int i = 0; i < spheres.Count; i++)
        {
            spheres[i].localPosition = poseAnnotations.transform.GetChild(i).localPosition;
        }
    }
}
