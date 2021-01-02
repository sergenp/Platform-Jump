using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Serializable]
    public class Sound
    {
        [HideInInspector] public AudioSource source;
        public AudioClip[] clips;
        public string name;
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
        sound.source.Play();
    }
}
