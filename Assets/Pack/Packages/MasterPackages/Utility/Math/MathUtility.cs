using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static int TakeADecimal(float f, int index)
    {
        //if (f % 1 == 0)
        //    return 0;
        return (int)((f * Mathf.Pow(10f, index)) % 10f);
    }

    public static float Approximation(float f, int numberOfDecimal)
    {
        if (f % 1 == 0)
            return f;
        if (numberOfDecimal < 0)
        {
            Debug.LogError("number of decimal must be positive");
            return f;
        }
        float pow = Mathf.Pow(10f, (float)numberOfDecimal);
        float fInt = Mathf.Round(f * pow);
        float newF = ((fInt % pow) / pow) + (int)f;
        return newF;
    }
}