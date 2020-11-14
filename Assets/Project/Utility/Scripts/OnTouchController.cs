using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTouchController : MonoBehaviour
{
    [SerializeField] bool OnUpdate = false;
    [Space]
    [Header("Tap Event")]
    [SerializeField] UnityEvent OnStartTouchUE = null;
    [SerializeField] UnityEvent OnEndTouchUE = null;
    [SerializeField] UnityEvent OnTouchUE = null;
    [SerializeField] UnityEvent OnStartOverlapTouchUE = null;
    [SerializeField] UnityEvent OnOverlapTouchUE = null;
    [SerializeField] UnityEvent OnEndOverlapTouchUE = null;
    
    public Action OnStartTouch;
    public Action OnEndTouch;
    public Action OnTouch;
    public Action OnStartOverlapTouch;
    public Action OnOverlapTouch;
    public Action OnEndOverlapTouch;

    RaycastHit hit;
    bool startTouch = false;
    bool wasOverlap = false;

    private void Update()
    {
        if (OnUpdate)
            TouchHandler();
    }

    public void TouchHandler()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(t.position), out hit))
                {
                    OnTouchController genericTouchEvent = hit.collider.GetComponent<OnTouchController>();

                    if (genericTouchEvent == this)
                    {
                        if (t.phase == TouchPhase.Began)
                        {
                            OnStartTouchUE?.Invoke();
                            OnStartTouch?.Invoke();
                            startTouch = true;
                        }
                        else if (t.phase == TouchPhase.Ended)
                        {
                            OnEndTouchUE?.Invoke();
                            OnEndTouch?.Invoke();
                            OnEndOverlapTouchUE?.Invoke();
                            OnEndOverlapTouch?.Invoke();
                            if (startTouch)
                            {
                                OnTouchUE?.Invoke();
                                OnTouch?.Invoke();
                            }
                            startTouch = false;
                            wasOverlap = false;
                        }

                        if (!wasOverlap)
                        {
                            wasOverlap = true;
                            OnStartOverlapTouchUE?.Invoke();
                            OnStartOverlapTouch?.Invoke();
                        }
                        else
                            OnOverlapTouchUE?.Invoke();
                            OnOverlapTouch?.Invoke();
                    }
                }
                else if (wasOverlap)
                {
                    wasOverlap = false;
                    OnEndOverlapTouchUE?.Invoke();
                    OnEndOverlapTouch?.Invoke();
                }
            }
        }
        else if (wasOverlap)
        {
            wasOverlap = false;
            OnEndOverlapTouchUE?.Invoke();
            OnEndOverlapTouch?.Invoke();
        }
    }

    public void CustomDebug(string s)
    {
        Debug.Log(s + name);
    }
}