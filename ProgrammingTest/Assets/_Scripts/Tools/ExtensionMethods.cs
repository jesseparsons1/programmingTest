using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    public static bool IsApproximately(this Vector3 u, Vector3 v)
    {
        return Mathf.Approximately(u.magnitude, v.magnitude);      
    }

    public static bool IsApproximately(this Vector3 u, Vector3 v, float minError)
    {
        if (minError < 0)
        {
            Debug.LogWarning("Invalid error threshold given, using Mathf.Approximately instead.");
            return u.IsApproximately(v);
        }
        else
        {
            return (u - v).magnitude <= minError;
        }
    }

    public static string AddCommas(this int x)
    {
        string output = "";
        string number = x.ToString();

        int iMax = number.Length;
        for (int i = 1; i <= iMax; i++)
        {
            output = number[iMax - i] + output;

            if (i != iMax && i % 3 == 0)
                output = "," + output;
        }

        return output;
    }
}
