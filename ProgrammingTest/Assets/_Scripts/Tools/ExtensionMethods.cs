using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    public static bool IsApproximately(this Vector3 u, Vector3 v)
    {
        return Mathf.Approximately(u.magnitude, v.magnitude);
    }
}
