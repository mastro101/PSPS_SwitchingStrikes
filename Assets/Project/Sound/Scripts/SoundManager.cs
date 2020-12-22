using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //public AudioMixerGroup mixerGroup;

    public Sound[] sounds;
    int l;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            l = instance.sounds.Length;
            for (int i = 0; i < l; i++)
            {
                Sound s = instance.sounds[i];
                //GameObject go = new GameObject(s.name);
                //go.transform.SetParent(this.transform);
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.playOnAwake = false;
            }
        }
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
            s = instance.sounds[i];
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
        Sound s = Array.Find(instance.sounds, item => item.name == sound);

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
            s = instance.sounds[i];
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
        Sound s = Array.Find(instance.sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
        }

        s.source.Pause();
    }

    public void Pause()
    {
        foreach (Sound sound in instance.sounds)
        {
            sound.source.Stop();
        }
    }
}