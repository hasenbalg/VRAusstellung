using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(AudioSource))]
public class ControlVideo : Control {

    VideoPlayer vp;
 
    void Start () {
        InitMarkers();
        vp = (VideoPlayer)GetComponent<VideoPlayer>();
    }

    void Update () {
        if (vp != null)
        {
            if (switchOn && !vp.isPlaying)
            {
                Play();
            }
            if (!switchOn && vp.isPlaying)
            {
                Play();
            }
        SetMarkerPos((float)vp.frame / vp.frameCount);
        }
        
    }

    public void Play() {
        if (isPlaying)
        {
            vp.Play();
        }
        else {
            vp.Pause();
        }
        isPlaying = !isPlaying;
    }
}
