using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIView : MonoBehaviour
{
    [SerializeField] IntData life = null;
    [Space]
    [SerializeField] Transform heartsTransformParent = null;
    [SerializeField] PoolManager HeartsPool = null;

    List<IPoolable> hearts;

    private void OnEnable()
    {
        for (int i = heartsTransformParent.childCount - 1; i >= 0; i--)
        {
            IPoolable tempP = heartsTransformParent.GetChild(i).GetComponent<IPoolable>();
            if (tempP != null)
            {
                tempP.Destroy();
            }
            else
                Destroy(heartsTransformParent.GetChild(i).gameObject);
        }

        HeartsPool.SetPoolable().SpawnObjs();
        hearts = new List<IPoolable>(HeartsPool.Count());
        if (life)
        {
            AddHeart(life.value);
            life.OnChangeValue += UpdateView;
        }
    }
    
    private void OnDisable()
    {
        if (life)
            life.OnChangeValue -= UpdateView;
    }

    public void UpdateView(int _life)
    {
        int diff = hearts.Count - _life;
        if (diff > 0)
        {
            RemoveHeart(diff);
        }
        else
        {
            AddHeart(-diff);
        }
    }

    void AddHeart(int l)
    {
        for (int i = 0; i < l; i++)
        {
            IPoolable tempP = HeartsPool.GetPoolablePrefab().Take(Vector3.zero, Quaternion.identity, heartsTransformParent);
            hearts.Add(tempP);
        }
    }

    void RemoveHeart(int n)
    {
        int l = hearts.Count;
        for (int i = l - 1; i >= l - n; i--)
        {
            hearts[i].Destroy();
            hearts.RemoveAt(i);
        }
    }
}