using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource source;
    public AudioClip button,writing,doorbell,doorClose;


    public void Play(string sound)
    {
        if (sound.Equals("button"))
            source.clip = button;
        else if (sound.Equals("writing"))
            source.clip = writing;
        else if (sound.Equals("doorbell"))
            source.clip = doorbell;
        else if (sound.Equals("doorClose"))
            source.clip = doorClose;

        source.Play();
    }
}
