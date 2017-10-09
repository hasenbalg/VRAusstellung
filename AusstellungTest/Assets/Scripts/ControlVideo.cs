using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[RequireComponent(typeof(AudioSource))]
public class ControlVideo : MonoBehaviour {

    VideoPlayer vp;
    bool isPlaying = false;
    public bool switchOn = false;

    GameObject timeLine, timeMarker;

    void Start () {
        vp = (VideoPlayer)GetComponent<VideoPlayer>();
        timeLine = transform.parent.Find("TimeLine").gameObject;
        timeMarker = timeLine.transform.Find("TimeMarker").gameObject;
    }
	
	void Update () {
        if (vp != null)
        {
            if (switchOn && !vp.isPlaying)
            {
                PlayVideo();
            }
            if (!switchOn && vp.isPlaying)
            {
                PlayVideo();
            }
        }
        SetMarkerPos();
        
    }

    public void PlayVideo() {
        if (isPlaying)
        {
            vp.Play();
        }
        else {
            vp.Pause();
        }
        isPlaying = !isPlaying;
        
    }

    public void SetMarkerPos(){
        float playProgress = (float)vp.frame / vp.frameCount;
        Vector3 markerPos = timeLine.transform.TransformPoint(Vector3.up * (playProgress * 2 - 1));//https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html
        timeMarker.transform.position = markerPos;
    }
}
