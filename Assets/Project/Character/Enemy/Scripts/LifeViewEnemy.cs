using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeViewEnemy : MonoBehaviour
{
    [SerializeField] Enemy enemy = null;
    [Space]
    [SerializeField] Transform heartsTransformParent = null;
    [SerializeField] PoolManager heartsPool = null;

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

        heartsPool.SetPoolable().SpawnObjs();
        hearts = new List<IPoolable>(heartsPool.Count());
        if (enemy)
        {
            AddHeart(enemy.currentLife);
            enemy.OnDamage += UpdateView;
            enemy.OnSpawn += UpdateView;
        }
    }

    private void OnDisable()
    {
        if (enemy)
        {
            enemy.OnDamage -= UpdateView;
            enemy.OnSpawn += UpdateView;
        }
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
    
    public void UpdateView()
    {
        int diff = hearts.Count - enemy.currentLife;
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
            IPoolable tempP = heartsPool.GetPoolablePrefab().Take(Vector3.zero, Quaternion.identity, heartsTransformParent);
            tempP.gameObject.transform.localScale = Vector3.one;
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