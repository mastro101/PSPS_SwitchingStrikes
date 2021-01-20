using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    //public AudioMixerGroup mixerGroup;
    [SerializeField] SoundTypeVolume[] soundTypes = null;
    [SerializeField] Sound[] sounds = null;
    int l;

    protected override SoundManager Setup()
    {
        //instance.soundTypes = prefab.soundTypes;
        //instance.sounds = prefab.sounds;

        l = GetInstance().sounds.Length;
        for (int i = 0; i < l; i++)
        {
            Sound s = GetInstance().sounds[i];
            //GameObject go = new GameObject(s.name);
            //go.transform.SetParent(this.transform);
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
        for (int i = 0; i < GetInstance().soundTypes.Length; i++)
        {
            GetInstance().soundTypes[i].SetSound(sounds);
            GetInstance().soundTypes[i].SetVolume();
        }
        return this;
    }

    public void Play(AudioClip clip)
    {
        Sound s;

        if (clip == null)
        {
            Debug.Log("Sound: clip == null");
            return;
        }

        for (int i = 0; i < l; i++)
        {
            s = GetInstance().sounds[i];
            if (s.clip == clip)
            {
                s.source.volume = s.volume;// * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
                s.source.pitch = s.pitch;// * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

                s.source.Play();
                return;
            }
        }

        Debug.LogWarning("Sound: " + clip.name + " not found!");
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(GetInstance().sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.volume = s.volume;// * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch;// * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Pause(AudioClip clip)
    {
        Sound s;

        if (clip == null)
            return;

        for (int i = 0; i < l; i++)
        {
            s = GetInstance().sounds[i];
            if (s.clip == clip)
            {
                s.source.Pause();
                return;
            }
        }

        Debug.LogWarning("Sound: " + clip.name + " not found!");
    }

    public void Pause(string sound)
    {
        Sound s = Array.Find(GetInstance().sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
        }

        s.source.Pause();
    }

    public void Pause()
    {
        foreach (Sound sound in GetInstance().sounds)
        {
            sound.source.Stop();
        }
    }

    public void SetVolume(float _volume, SoundType _type)
    {
        for (int i = 0; i < soundTypes.Length; i++)
        {
            if (soundTypes[i].type == _type)
                soundTypes[i].SetVolume(_volume);
        }
    }

    public float GetVolume(SoundType _type)
    {
        for (int i = 0; i < soundTypes.Length; i++)
        {
            if (soundTypes[i].type == _type)
                return soundTypes[i].volume;
        }

        Debug.LogError(_type + " is not present in the sound manager");
        return 0f;
    }
}