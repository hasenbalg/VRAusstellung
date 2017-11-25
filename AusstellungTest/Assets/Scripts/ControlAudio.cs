using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(AudioSource))]
public class ControlAudio : Control {

    AudioSource audioSource;

    void Start () {
        InitMarkers();
        audioSource = GetComponent<AudioSource>();
    }
	
	void Update () {
        if (audioSource != null && audioSource.clip != null)
        {
            if (switchOn && !audioSource.isPlaying)
            {
                Play();
            }
            if (!switchOn && audioSource.isPlaying)
            {
                Play();
            }
        SetMarkerPos(audioSource.time / audioSource.clip.length);
        }
        
    }

    public void Play() {
        if (isPlaying)
        {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }
        isPlaying = !isPlaying;
    }
}
