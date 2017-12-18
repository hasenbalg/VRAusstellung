using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour {
    public GameObject canvas, timeLine, index;
    //public RawImage image;

    //public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;
    private bool clipIsLoaded = false;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        Play();
    }

    private void Play()
    {

        videoPlayer = canvas.GetComponent<VideoPlayer>();
        audioSource = canvas.GetComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        audioSource.volume = 1.0f;
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1);
        

        Debug.Log("Done Preparing Video");

        videoPlayer.Play();
        audioSource.Play();

        
    }

    void Update()
    {
        if (!float.IsNaN(videoPlayer.frame / (float)videoPlayer.frameCount))
        {
            index.transform.localPosition = new Vector3(
                0,
                map(videoPlayer.frame / (float)videoPlayer.frameCount, 0, 1, -1, 1),
                0
                );
        }
            
    }

    private void OnTriggerEnter(Collider col)
    {
        videoPlayer.Pause();
        audioSource.Pause();
    }
    void OnTriggerStay(Collider other)
    {
        Vector3 colpos = transform.InverseTransformPoint(other.transform.position);
        videoPlayer.frame = (int)Mathf.Clamp(
            map(colpos.y, -1, 1, 0, videoPlayer.frameCount)
            , 0
            , videoPlayer.frameCount
            );
    }

    private void OnTriggerExit(Collider other)
    {
        videoPlayer.Play();
        audioSource.Play();
    }

    private void OnApplicationQuit()
    {
        Destroy(videoPlayer);
    }


    public static float map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}
