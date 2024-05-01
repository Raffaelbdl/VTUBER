using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Vector3Extension
{
    public static Vector3 ChangeX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }
    public static Vector3 ChangeY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }
    public static Vector3 ChangeZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
}
