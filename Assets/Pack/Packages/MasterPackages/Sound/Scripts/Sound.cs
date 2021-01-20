using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public SoundType type;

    [Range(0f, 1f)]
    public float volume = .75f;
    [Range(0f, 1f)]
    public float maxVolume = .75f;

    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchVariance = .1f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class SoundTypeVolume
{

    [SerializeField] SoundType _type = SoundType.Effect;
    public SoundType type { get => _type; private set { _type = value; } }

    [Range(0f, 1f)] [SerializeField] float _volume = 1f;
    public float volume 
    {
        get => _volume;
        private set
        {
            _volume = value;
            if (_volume < 0f)
                _volume = 0f;
            else if (_volume > 1f)
                _volume = 1f;
            SetVolume();
        }
    }

    List<Sound> sounds;

    public void SetSound(Sound[] _sounds)
    {
        sounds = new List<Sound>(_sounds.Length);
        int l = _sounds.Length;
        for (int i = 0; i < l; i++)
        {
            if (_sounds[i].type == type)
                sounds.Add(_sounds[i]);
        }
    }

    public void SetVolume(float v)
    {
        volume = v;
    }

    public void SetVolume()
    {
        int l = sounds.Count;
        for (int i = 0; i < l; i++)
        {
            sounds[i].volume = volume * sounds[i].maxVolume;
        }
    }
}

public enum SoundType
{
    BGM,
    Effect,
}