using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [Serializable]
    public class Sound
    {
        [HideInInspector] public AudioSource source;
        public AudioMixerGroup audioMixer;
        public AudioClip[] clips;
        public string name;
        public bool loop = false;
    }

    public Sound[] sounds;
    
    public static AudioManager instance;
    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayAudioOneShot(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"{sound.name} couldn't be found");
            return;
        }
        sound.source.clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
        sound.source.loop = sound.loop;
        sound.source.outputAudioMixerGroup = sound.audioMixer;
        sound.source.Play();
    }
}
