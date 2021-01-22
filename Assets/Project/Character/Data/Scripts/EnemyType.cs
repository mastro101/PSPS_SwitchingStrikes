using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable/Enemy/Type")]
public class EnemyType : ScriptableObject
{
    public Color color = new Color(1,1,1,1);
    public VFXAttack VFXAttack;
    PlayerMaskScriptable maskData;

    public void Setup(PlayerMaskScriptable _maskData)
    {
        if (VFXAttack != null)
        {
            maskData = _maskData;
        }
    }

    public Sprite GetMaskSprite()
    {
        return maskData.GetMaskSprite(this);
    }
}

[System.Serializable]
public class VFXAttack
{
    [SerializeField] ParticleSystem generateAttackSerialized = null;
    [SerializeField] ParticleSystem attackSerialized = null;
    [SerializeField] ParticleSystem destroiedAttackSerialized = null;

    [HideInInspector] public ParticleSystem generateAttack = null;
    [HideInInspector] public ParticleSystem attack = null;
    [HideInInspector] public ParticleSystem destroiedAttack = null;

    public void ResetPosition(Transform t)
    {
        generateAttack.transform.position = t.position;
        attack.transform.position = t.position;
        destroiedAttack.transform.position = t.position;
    }

    public void Generate()
    {
        generateAttack = GameObject.Instantiate(generateAttackSerialized);
        attack = GameObject.Instantiate(attackSerialized);
        destroiedAttack = GameObject.Instantiate(destroiedAttackSerialized);

        generateAttack.Stop();
        attack.Stop();
        destroiedAttack.Stop();

        generateAttack.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        destroiedAttack.gameObject.SetActive(false);
    }

    public void Play(ParticleSystem particleSystem)
    {
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();
    }

    public void Stop(ParticleSystem particleSystem)
    {
        particleSystem.gameObject.SetActive(false);
        particleSystem.Stop();
    }
}