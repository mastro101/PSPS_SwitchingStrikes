using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Color newColor = Color.white;

    Color oldColor;

    private void Awake()
    {
        if (spriteRenderer)
        {
            oldColor = spriteRenderer.color;
        }
    }

    public void ChangeColor()
    {
        if (spriteRenderer)
            spriteRenderer.color = newColor;
    }

    public void ResumeColor()
    {
        if (spriteRenderer)
            spriteRenderer.color = oldColor;
    }
}