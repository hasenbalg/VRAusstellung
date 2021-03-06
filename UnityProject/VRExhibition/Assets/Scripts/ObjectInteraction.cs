﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{

    GameObject hitGameObject;
    //float lastTrackpadPosition = 0;
    float rightTrackpadAxis;
    //float scrubbingFactor = 2;
    bool triggerActivated = false;
    //bool mediaSwitchedOnBeforeTrackpad = false;
    ControlAudio ctrlAudio = null;
    ControlVideo ctrlVideo = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(raycast, out hit, 100f);

        if (raycastHit)
        {
            hitGameObject = hit.collider.gameObject;
            if (this.CheckMediaAudio(hitGameObject))
            {
                this.ctrlAudio = hitGameObject.GetComponent<ControlAudio>();
                this.ctrlVideo = null;
            } else if (this.CheckMediaVideo(hitGameObject))
            {
                this.ctrlVideo = hitGameObject.GetComponent<ControlVideo>();
                this.ctrlAudio = null;
            }
        }
        else
        {
            hitGameObject = null;
            this.ctrlAudio = null;
            this.ctrlVideo = null;
        }

        if (Input.GetAxis("RightTrigger") > 0.8f)
        {
            if (this.ctrlAudio != null && !triggerActivated)
            {
                this.ctrlAudio.switchOn = !this.ctrlAudio.switchOn;
            }
            else if (this.ctrlVideo != null && !triggerActivated)
            {
                this.ctrlVideo.switchOn = !this.ctrlVideo.switchOn;
            }
            triggerActivated = true;
        }
        else
        {
            triggerActivated = false;
        }

        this.rightTrackpadAxis = Input.GetAxis("RightTrackpadHorizontalMovement");

        if (rightTrackpadAxis != 0)
        {
            if (this.ctrlAudio != null)
            {
                this.ctrlAudio.switchOn = false;
                this.ctrlAudio.audioSource.time = ((this.rightTrackpadAxis + 1) / 2) * this.ctrlAudio.audioSource.clip.length; //Make Left Edge = 0 and Right Edge = 1
            } else if (this.ctrlVideo != null)
            {
                this.ctrlVideo.switchOn = false;
                this.ctrlVideo.vp.frame = (long)(((this.rightTrackpadAxis + 1) / 2) * this.ctrlVideo.vp.frameCount);
            }

            //Not working well, maybe implement later
            #region scrub_by_adding_time
            /*
            if (this.CheckMediaAudio(hitGameObject))
            {

            }

            if (this.lastTrackpadPosition == 0)
            {
                this.lastTrackpadPosition = rightTrackpadAxis;
            }
            else
            {
                if (this.CheckMediaAudio(hitGameObject))
                {
                    if (this.rightTrackpadAxis > 0)
                    {
                        //Add absolute distance travelled to audioSource.time
                        hitGameObject.GetComponent<ControlAudio>().audioSource.time = Mathf.Min(hitGameObject.GetComponent<ControlAudio>().audioSource.time + this.scrubbingFactor * (Mathf.Max(this.rightTrackpadAxis, this.lastTrackpadPosition) - Mathf.Min(this.rightTrackpadAxis, this.lastTrackpadPosition)), hitGameObject.GetComponent<ControlAudio>().audioSource.clip.length);
                    }
                    else
                    {
                        //Subtract absolute distance travelled from audioSource.time
                        hitGameObject.GetComponent<ControlAudio>().audioSource.time = Mathf.Max(hitGameObject.GetComponent<ControlAudio>().audioSource.time - this.scrubbingFactor * (Mathf.Max(this.rightTrackpadAxis, this.lastTrackpadPosition) - Mathf.Min(this.rightTrackpadAxis, this.lastTrackpadPosition)), 0);
                    }
                }
                else if (this.CheckMediaVideo(hitGameObject))
                {
                    //hitGameObject.GetComponent<ControlVideo>().;
                }

            }
            */
            #endregion scrub_by_adding_time
        }
        else
        {
            //this.lastTrackpadPosition = 0;
        }
    }

    bool CheckMediaAudio(GameObject go)
    {
        if (go != null)
        {
            if (go.GetComponent<ControlAudio>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckMediaVideo(GameObject go)
    {
        if (go != null)
        {
            if (go.GetComponent<ControlVideo>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
