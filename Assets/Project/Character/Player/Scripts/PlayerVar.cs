using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInstance", menuName = "ScriptableVar/PlayerInstance")]
public class PlayerVar : ScriptableVar<Player>
{
    private void OnDestroy()
    {
        value = null;
    }
}