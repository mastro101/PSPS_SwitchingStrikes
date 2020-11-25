using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDeviceController : MonoBehaviour
{
    public static void RotateVerticalScreen()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    
    public static void RotateHorizontalScreen()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }
}