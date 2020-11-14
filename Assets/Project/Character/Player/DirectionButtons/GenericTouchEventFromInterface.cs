using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTouchEventFromInterface : MonoBehaviour , TouchBehaviour
{
    Collider2D TouchBehaviour.collider => _collider;
    [SerializeField] Collider2D _collider = null;
    [Space]
    [SerializeField] UnityEvent OnStartTouchUE = null;
    [SerializeField] UnityEvent OnEndTouchUE = null;
    [SerializeField] UnityEvent OnTapUE = null;
    [SerializeField] UnityEvent OnStartOverlapTouchUE = null;
    [SerializeField] UnityEvent OnEndOverlapTouchUE = null;
    [SerializeField] UnityEvent OnOverlapTouchUE = null;

    public bool startTouch { get; set; }
    public bool oldOverlapTouch { get; set; }
    public bool overlapTouch { get; set; }

    public void OnStartTouch(int _touchIndex)
    {
        OnStartTouchUE?.Invoke();
    }

    public void OnEndTouch(int _touchIndex)
    {
        OnEndTouchUE?.Invoke();
    }

    public void OnTap(int _touchIndex)
    {
        OnTapUE?.Invoke();
    }

    public void OnStartOverlapTouch(int _touchIndex)
    {
        OnStartOverlapTouchUE?.Invoke();
    }

    public void OnOverlapTouch(int _touchIndex)
    {
        OnOverlapTouchUE?.Invoke();
    }

    public void OnEndOverlapTouch(int _touchIndex)
    {
        OnEndOverlapTouchUE?.Invoke();
    }

    private void Update()
    {
        if (oldOverlapTouch == true && overlapTouch == false)
        {
            OnEndOverlapTouch(0);
        }

        if (overlapTouch)
            overlapTouch = false;
    }
}