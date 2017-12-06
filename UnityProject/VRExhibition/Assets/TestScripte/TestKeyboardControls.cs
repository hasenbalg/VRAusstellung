using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestKeyboardControls : MonoBehaviour
{

    GameObject exhibitionPiece;
    BuildShow buildShow;

    // Use this for initialization
    void Start()
    {
        this.exhibitionPiece = null;
        this.buildShow = GetComponent<BuildShow>();
        Debug.Log("BuildShow : " + this.buildShow);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("i"))
        {
            buildShow.MoveRoom(BuildShow.MoveDirection.MPOS);
        }

        if (Input.GetKeyDown("k"))
        {
            buildShow.MoveRoom(BuildShow.MoveDirection.MNEG);
        }

        if (Input.GetKeyDown("o"))
        {
            buildShow.MoveRoom(BuildShow.MoveDirection.NPOS);
        }

        if (Input.GetKeyDown("l"))
        {
            buildShow.MoveRoom(BuildShow.MoveDirection.NNEG);
        }


        if (this.exhibitionPiece == null)
        {
            this.exhibitionPiece = GameObject.FindGameObjectWithTag("CanvasVideo");
            if (this.exhibitionPiece == null)
            {
                this.exhibitionPiece = GameObject.FindGameObjectWithTag("CanvasAudio");
            }
            //Debug.Log("Exhibition Piece Assign Attempt");
        }

        if (Input.GetKeyDown("e") && this.exhibitionPiece != null)
        {
            AudioSource srcAud = null;
            VideoPlayer srcVid = null;

            if (this.exhibitionPiece.CompareTag("CanvasAudio"))
            {
                srcAud = this.exhibitionPiece.GetComponent<AudioSource>();
            }
            else
            {
                srcVid = this.exhibitionPiece.GetComponent<VideoPlayer>();
            }
            if (srcAud != null)
            {
                if (!srcAud.isPlaying)
                {
                    Debug.Log("Audio Source Play : " + srcAud.clip.ToString());
                    this.exhibitionPiece.GetComponent<ControlAudio>().switchOn = true;
                }
                else
                {
                    Debug.Log("Audio Source Pause : " + srcAud.clip.ToString());
                    this.exhibitionPiece.GetComponent<ControlAudio>().switchOn = false;
                }
            }
            if (srcVid != null)
            {
                if (!srcVid.isPlaying)
                {
                    Debug.Log("Video Source Play : " + srcVid.ToString());
                    this.exhibitionPiece.GetComponent<ControlVideo>().switchOn = true;
                }
                else
                {
                    Debug.Log("Video Source Pause : " + srcVid.ToString());
                    this.exhibitionPiece.GetComponent<ControlVideo>().switchOn = false;
                }
            }
        }
        else if (Input.GetKeyDown("e") && this.exhibitionPiece == null)
        {
            Debug.Log("Exhibition Piece = Null");
        }
    }
}
