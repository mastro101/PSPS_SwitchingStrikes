using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasksMenuView : MonoBehaviour
{
    [SerializeField] MasksMenuController masksMenuController = null;
    [SerializeField] Image maskImagePrefab = null;
    [SerializeField] Color lockColor = default;
    [SerializeField] float animationDuration;
    [SerializeField] Transform marginLeftPos = null, leftMaskPos = null, currentMaskPos = null, rightMaskPos = null, marginRightPos = null;

    Image[] images;
    int l;

    int _index = 0;
    public int index
    {
        get => _index;
        private set
        {
            _index = value;
            if (_index >= images.Length)
                _index = 0;
            else if (_index < 0)
                _index = images.Length - 1;
        }
    }

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        masksMenuController.OnChangeRight += ChangeViewRight;
        masksMenuController.OnChangeLeft += ChangeViewLeft;
    }

    private void OnDisable()
    {
        masksMenuController.OnChangeRight -= ChangeViewRight;
        masksMenuController.OnChangeLeft -= ChangeViewLeft;
    }

    void Setup()
    {
        l = masksMenuController.GetMasks().Length;
        if (l <= 3)
        {
            images = new Image[l * 2];
        }
        else
        {
            images = new Image[l];
        }

        for (int i = 0; i < images.Length; i++)
        {
            InstantiateMask(i);
        }

        index = masksMenuController.index;

        images[index].transform.position = currentMaskPos.position;
        images[GetNext(1)].transform.position = rightMaskPos.position;
        images[GetNext(2)].transform.position = marginRightPos.position;
        images[GetNext(-1)].transform.position = leftMaskPos.position;
        images[GetNext(-2)].transform.position = marginLeftPos.position;

    }

    void InstantiateMask(int index)
    {
        int l = masksMenuController.GetMasks().Length;
        int n = index >= l ? l : 0;

        PlayerMaskScriptable mask = masksMenuController.GetMask(index - n);

        images[index] = Instantiate(maskImagePrefab, Vector3.right * 10000, Quaternion.identity, transform);
        Image tempImage1 = images[index];
        tempImage1.sprite = mask.GetMaskSprite(0);
        if (!mask.IsUnlock())
            tempImage1.color = lockColor;
    }

    void ChangeViewRight(int MaskIndex)
    {
        index++;
        images[index].transform.DOMove(currentMaskPos.position, animationDuration);
        images[GetNext(1)].transform.position = marginRightPos.position;
        images[GetNext(1)].transform.DOMove(rightMaskPos.position, animationDuration);
        images[GetNext(2)].transform.position = marginRightPos.position;
        images[GetNext(-1)].transform.DOMove(leftMaskPos.position, animationDuration);
        images[GetNext(-2)].transform.DOMove(marginLeftPos.position, animationDuration);
    }
    
    void ChangeViewLeft(int MaskIndex)
    {
        index--;
        images[index].transform.DOMove(currentMaskPos.position, animationDuration);
        images[GetNext(1)].transform.DOMove(rightMaskPos.position, animationDuration);
        images[GetNext(2)].transform.DOMove(marginRightPos.position, animationDuration);
        images[GetNext(-1)].transform.position = marginLeftPos.position;
        images[GetNext(-1)].transform.DOMove(leftMaskPos.position, animationDuration);
        images[GetNext(-2)].transform.position = marginLeftPos.position;
    }

    int GetNext(int n = 1)
    {
        int i = index + n;
        if (i >= images.Length)
        {
            i -= images.Length;
        }
        else if (i < 0)
        {
            i += images.Length;
        }
        return i;
    }
}