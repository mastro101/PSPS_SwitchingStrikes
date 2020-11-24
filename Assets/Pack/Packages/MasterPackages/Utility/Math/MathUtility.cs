using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static int TakeADecimal(float f, int index)
    {
        return (int)((index * Mathf.Pow(10f, index)) % 10f);
    }

    public static float Approximation(float f, int numberOfDecimal)
    {
        if (f == 0)
            return 0;
        float pow = Mathf.Pow(10, (float)numberOfDecimal);
        float fNoComma = f * pow;
        float round = Mathf.Round(fNoComma);
        float newF = ((round % pow) / pow) + (int)f;
        Debug.LogFormat("approximetion of {0} with {1} decimal = {2}", f, numberOfDecimal, newF);
        return newF;
    }
}