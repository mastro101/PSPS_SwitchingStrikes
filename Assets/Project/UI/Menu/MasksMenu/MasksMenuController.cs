using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasksMenuController : MonoBehaviour
{
    [SerializeField] MaskVar equippedMask = null;
    [SerializeField] PlayerMaskScriptable[] masks = null;

    public System.Action<int> OnChangeRight;
    public System.Action<int> OnChangeLeft;
    public System.Action<PlayerMaskScriptable> OnEquip;

    PlayerMaskScriptable currentMask;

    int _index = 0;
    public int index
    {
        get => _index;
        private set
        {
            _index = value;
            if (_index >= masks.Length)
                _index = 0;
            else if (_index < 0)
                _index = masks.Length - 1;
            currentMask = masks[_index];
        }
    }

    public PlayerMaskScriptable[] GetMasks()
    {
        return masks;
    }

    public PlayerMaskScriptable GetMask(int i)
    {
        return masks[i];
    }

    public int GetNext(int n = 1)
    {
        int i = index + n;
        if (i >= masks.Length)
        {
            i -= masks.Length;
        }
        else if (i < 0)
        {
            i += masks.Length;
        }
        return i;
    }

    public PlayerMaskScriptable GoRight()
    {
        index++;
        OnChangeRight?.Invoke(index);
        return currentMask;
    }

    public PlayerMaskScriptable GoLeft()
    {
        index--;
        OnChangeLeft?.Invoke(index);
        return currentMask;
    }

    public PlayerMaskScriptable Equip()
    {
        if (currentMask.IsUnlock())
            return equippedMask.SetValue(currentMask);
        return equippedMask.value;
    }
}