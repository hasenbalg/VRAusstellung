using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ControlVideo : MonoBehaviour {
    // Use this for initialization
    public MovieTexture mt;
     bool isPlaying = false;
    public bool switchOn = false;
    void Start () {
        mt = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
	}
	
	// Update is called once per frame
	void Update () {
        if (mt != null)
        {
            if (switchOn && !mt.isPlaying)
            {
                PlayVideo();
            }
            if (!switchOn && mt.isPlaying)
            {
                PlayVideo();
            }
        }
      
	}

    public void PlayVideo() {
        var aud = GetComponent<AudioSource> ();
        aud.clip = mt.audioClip;
        if (isPlaying)
        {
            mt.Play();
            aud.Play();
        }
        else {
            mt.Pause();
            aud.Pause();
        }
        isPlaying = !isPlaying;
        
    }
}
