using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {

    public GameObject index, timeLine;
    public AudioSource audiosource;
    private bool clipIsLoaded = false;

    //public bool shouldPlay = true;



    // Update is called once per frame
    void Update()
    {
        if (audiosource.clip != null && !audiosource.isPlaying && !clipIsLoaded)
        {
            audiosource.Play();
            clipIsLoaded = true;
        }
        if (audiosource.clip != null)
        {
            index.transform.localPosition = new Vector3(
                0,
                map(audiosource.time / audiosource.clip.length, 0, 1, -1, 1),
                0
                );
        }


    }

    private void OnTriggerEnter(Collider col)
    {
        audiosource.Pause();
    }
    void OnTriggerStay(Collider other)
    {
        Vector3 colpos = transform.InverseTransformPoint(other.transform.position);
        audiosource.time = Mathf.Clamp(
            map(colpos.y, -1, 1, 0, audiosource.clip.length)
            , 0
            , audiosource.clip.length
            );
    }

    private void OnTriggerExit(Collider other)
    {
        audiosource.UnPause();
    }



    public static float map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}
