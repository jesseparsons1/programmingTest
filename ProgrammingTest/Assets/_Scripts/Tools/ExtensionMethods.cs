using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    //FLOAT EXTENSIONS

    public static float PositiveModulo(this float x, int modulus)
    {
        float newX = x % modulus;
        if (newX < 0)
            newX += modulus;
        return newX;
    }

    public static bool IsApproximately(this float x, float y, float minError)
    {
        if (minError < 0)
        {
            Debug.LogWarning("Invalid error threshold given, using Mathf.Approximately instead.");
            return false;
        }
        else
        {
            return Mathf.Abs(x - y) <= minError;
        }
    }





    //VECTOR3 EXTENSIONS

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





    //STRING EXTENSIONS

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
