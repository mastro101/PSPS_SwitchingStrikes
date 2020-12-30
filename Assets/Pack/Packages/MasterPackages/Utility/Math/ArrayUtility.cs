using UnityEngine;
using System.Collections;

public static class ArrayUtility
{
    public static float ArraySum(this float[] fs)
    {
        int l = fs.Length;
        float sum = 0;
        for (int i = 0; i < l; i++)
        {
            sum += fs[i];
        }
        return sum;
    }

    public static void DivideArray(this float[] fs, float div)
    {
        int l = fs.Length;
        for (int i = 0; i < l; i++)
        {
            fs[i] /= div;
        }
    }
}