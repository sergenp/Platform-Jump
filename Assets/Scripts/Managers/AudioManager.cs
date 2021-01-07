using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource;

    float? backgroundMusicTime;

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

    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            backgroundMusicTime = SaveSystem<float>.LoadData("musicTime");
            if (backgroundMusicTime == null)
            {
                backgroundMusicTime = 0f;
            }
            backgroundMusicSource.time = (float) backgroundMusicTime;
        }
        else { 
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
        }
    }
    #endregion

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

    // save the current background time, this way 
    // background music won't start from the beginning each time you open up the game
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveSystem<float>.SaveData(backgroundMusicSource.time, "musicTime");
        } 
    }

    private void OnApplicationQuit()
    {
        SaveSystem<float>.SaveData(backgroundMusicSource.time, "musicTime");
    }
}
