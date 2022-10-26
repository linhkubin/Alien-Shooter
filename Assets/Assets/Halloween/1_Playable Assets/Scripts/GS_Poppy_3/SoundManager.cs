using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [LunaPlaygroundFieldStep(0.1f)]
    [LunaPlaygroundField("SFX Volume", 2, "Game Settings")]
    [Range(0, 1)]
    public float SFXVolume = 1f;

    [LunaPlaygroundField("Background Music", 2, "Game Settings")]
    public bool BGM = true;

    public AudioSource BackgroundMusic;
    public AudioSource[] AudioSources;

    // Start is called before the first frame update
    void Start()
    {
        foreach(AudioSource eachAudioSource in AudioSources)
        {
            eachAudioSource.volume = SFXVolume;
        }

        BackgroundMusic.mute = !BGM;
    }
}
