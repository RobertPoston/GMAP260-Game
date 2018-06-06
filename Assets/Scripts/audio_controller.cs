using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audio_controller : MonoBehaviour {

    public AudioMixer music;
    public AudioMixer sfx;
    public bool setMusic = false;

    public void SetSound(float soundLevel)
    {
        if (setMusic)
        {
            music.SetFloat("music_volume", soundLevel);
        }
        else
        {
            sfx.SetFloat("sfx_volume", soundLevel);
        }
    }

}
