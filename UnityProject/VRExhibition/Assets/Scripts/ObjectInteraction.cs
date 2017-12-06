using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

public class ObjectInteraction : MonoBehaviour
{

    GameObject hitGameObject, handObject, currentGrabbableObject, grabbedObject;
    Transform grabbedObjectInitialParent;
    Hand handScript;
    //float lastTrackpadPosition = 0;
    float rightTrackpadAxis;
    //float scrubbingFactor = 2;
    bool leftTriggerActivated = false;
    bool rightTriggerActivated = false;
    //bool mediaSwitchedOnBeforeTrackpad = false;
    ControlAudio ctrlAudio = null;
    ControlVideo ctrlVideo = null;
    public AudioSource srcAudio = null;
    public VideoPlayer srcVideo = null;
    GameObject door = null;
    BuildShow bs;
    Hand.HandType handType;
    float lastFrameControllerRotation, lastFrameKnobRotation;

    // Use this for initialization
    void Start()
    {
        this.lastFrameControllerRotation = 0f;
        this.lastFrameKnobRotation = 0f;
        this.bs = GameObject.Find("GameManager").GetComponent<BuildShow>();
        this.handObject = this.gameObject;
        this.handScript = this.handObject.GetComponent<Hand>();
        this.handType = this.handScript.startingHandType;
    }

    // Update is called once per frame
    void Update()
    {

        #region raycast
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
                this.door = null;
            }
            else if (this.CheckMediaVideo(hitGameObject))
            {
                this.ctrlVideo = hitGameObject.GetComponent<ControlVideo>();
                this.ctrlAudio = null;
                this.door = null;
            }
            else if (hitGameObject.CompareTag("Door"))
            {
                this.door = hitGameObject;
                this.ctrlAudio = null;
                this.ctrlVideo = null;
            }
            else
            {
                this.door = null;
                this.ctrlAudio = null;
                this.ctrlVideo = null;
            }
        }
        else
        {
            this.door = null;
            this.ctrlAudio = null;
            this.ctrlVideo = null;
        }
        #endregion raycast

        #region grabObject
        if (this.handType == Hand.HandType.Right)
        {
            if (this.currentGrabbableObject != null && Input.GetAxis("RightTrigger") > 0.8f && !rightTriggerActivated)
            {
                this.rightTriggerActivated = true;
                this.OnGrab();
            }
            else if (this.grabbedObject != null && Input.GetAxis("RightTrigger") < 0.4f && rightTriggerActivated)
            {
                this.rightTriggerActivated = false;
                this.OnRelease();
            }
        }
        else if (this.handType == Hand.HandType.Left)
        {
            if (this.currentGrabbableObject != null && Input.GetAxis("LeftTrigger") > 0.8f && !leftTriggerActivated)
            {
                this.OnGrab();
            }
            else if (this.grabbedObject != null && Input.GetAxis("LeftTrigger") < 0.4f && leftTriggerActivated)
            {
                this.OnRelease();
            }
        }

        /*
        if (Input.GetAxis("RightTrigger") > 0.8f && this.currentGrabbableObject == null)
        {
            this.rightTriggerActivated = true;
        }
        else if (Input.GetAxis("RightTrigger") < 0.8f && this.currentGrabbableObject == null)
        {
            this.rightTriggerActivated = false;
        }
        */
        #endregion grabObject

        #region LimitMarkerMovement
        /*
        //Checks if currently grabbed object is a TimeMarker and, if so, clamps its X-Position to restrain it to the TimeLine.
        if(this.grabbedObject != null)
        {
            if (this.grabbedObject.name == "TimeMarker")
            {
                GameObject timeLine = grabbedObject.transform.parent.gameObject;
                Mathf.Clamp(
                    this.grabbedObject.transform.position.x,
                    timeLine.transform.TransformPoint(Vector3.up * (0 * 2 - 1)).x,
                    timeLine.transform.TransformPoint(Vector3.up * (1 * 2 - 1)).x
                    );
            }
        }
        */
        #endregion LimitMarkerMovement

        #region triggerEvents
        /*
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
            else if (this.door != null && !triggerActivated)
            {
                switch (this.door.name)
                {
                    case "mPositive":
                        this.bs.MoveRoom(BuildShow.MoveDirection.MPOS);
                        break;
                    case "mNegative":
                        this.bs.MoveRoom(BuildShow.MoveDirection.MNEG);
                        break;
                    case "nPositive":
                        this.bs.MoveRoom(BuildShow.MoveDirection.NPOS);
                        break;
                    case "nNegative":
                        this.bs.MoveRoom(BuildShow.MoveDirection.NNEG);
                        break;
                    default:
                        break;
                }
            }
            triggerActivated = true;
        }
        else
        {
            triggerActivated = false;
        }
        */
        #endregion triggerEvents

        #region mediaScrubbing
        /*
        this.rightTrackpadAxis = Input.GetAxis("RightTrackpadHorizontalMovement");

        if (rightTrackpadAxis != 0)
        {
            if (this.ctrlAudio != null)
            {
                this.ctrlAudio.switchOn = false;
                this.ctrlAudio.audioSource.time = ((this.rightTrackpadAxis + 1) / 2) * this.ctrlAudio.audioSource.clip.length; //Make Left Edge = 0 and Right Edge = 1
            }
            else if (this.ctrlVideo != null)
            {
                this.ctrlVideo.switchOn = false;
                this.ctrlVideo.vp.frame = (long)(((this.rightTrackpadAxis + 1) / 2) * this.ctrlVideo.vp.frameCount);
            }
            */
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
        /*
        }
        else
        {
            //this.lastTrackpadPosition = 0;
        }
        */
        #endregion mediaScrubbing

        if (this.grabbedObject != null)
        {
            switch (this.grabbedObject.name)
            {
                case "VolumeKnobAudio":
                case "VolumeKnobVideo":
                    this.grabbedObject.transform.rotation = Quaternion.Euler(this.grabbedObject.transform.rotation.eulerAngles.x, this.grabbedObject.transform.rotation.eulerAngles.y,
                        this.grabbedObject.transform.rotation.eulerAngles.z + (this.transform.rotation.eulerAngles.z - this.lastFrameControllerRotation));
                    this.lastFrameControllerRotation = this.transform.rotation.eulerAngles.z;
                    break;
                case "ScrubbingKnobAudio":
                    this.grabbedObject.transform.rotation = Quaternion.Euler(this.grabbedObject.transform.rotation.eulerAngles.x, this.grabbedObject.transform.rotation.eulerAngles.y,
                        this.grabbedObject.transform.rotation.eulerAngles.z + (this.transform.rotation.eulerAngles.z - this.lastFrameControllerRotation));
                    this.lastFrameControllerRotation = this.transform.rotation.eulerAngles.z;
                    this.srcAudio.time = Mathf.Clamp((this.srcAudio.time - (this.grabbedObject.transform.rotation.eulerAngles.z - lastFrameKnobRotation) / 15), 0, srcAudio.clip.length);
                    this.lastFrameKnobRotation = this.grabbedObject.transform.rotation.eulerAngles.z;
                    break;
                case "ScrubbingKnobVideo":
                    this.grabbedObject.transform.rotation = Quaternion.Euler(this.grabbedObject.transform.rotation.eulerAngles.x, this.grabbedObject.transform.rotation.eulerAngles.y,
                        this.grabbedObject.transform.rotation.eulerAngles.z + (this.transform.rotation.eulerAngles.z - this.lastFrameControllerRotation));
                    this.lastFrameControllerRotation = this.transform.rotation.eulerAngles.z;
                    this.srcVideo.frame = (long)Mathf.Clamp((this.srcVideo.frame - ((this.grabbedObject.transform.rotation.eulerAngles.z - lastFrameKnobRotation) * 10)), 0, this.srcVideo.frameCount - 1);
                    this.lastFrameKnobRotation = this.grabbedObject.transform.rotation.eulerAngles.z;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Something is grabbable! : " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Grabbable") || collision.gameObject.CompareTag("Door") && this.grabbedObject == null)
        {
            this.currentGrabbableObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("No longer grabbable! : " + collision.gameObject.name);
        this.currentGrabbableObject = null;
    }

    private void OnGrab()
    {
        this.grabbedObjectInitialParent = this.currentGrabbableObject.transform.parent;
        //this.currentGrabbableObject.transform.SetParent(this.transform);
        this.grabbedObject = currentGrabbableObject;
        Debug.Log(grabbedObject.name);

        /*
         * Pauses/Plays Media and assigns ctrlAudio/ctrlVideo and srcAudio/srcVideo for scrubbing and adjusting volume
         * Also manages interaction with doors
        */
        #region immediateInteraction
        switch (this.grabbedObject.name)
        {
            case "PlayButtonAudio":
                ControlAudio ctrlAudio = this.grabbedObjectInitialParent.GetComponent<ControlAudio>();
                if (ctrlAudio.switchOn)
                {
                    this.grabbedObject.transform.position = new Vector3(this.grabbedObject.transform.position.x, this.grabbedObject.transform.position.y, this.grabbedObject.transform.position.z + 0.01f);
                }
                else
                {
                    this.grabbedObject.transform.position = new Vector3(this.grabbedObject.transform.position.x, this.grabbedObject.transform.position.y, this.grabbedObject.transform.position.z - 0.01f);
                }
                ctrlAudio.switchOn = !ctrlAudio.switchOn;
                break;

            case "PlayButtonVideo":
                ControlVideo ctrlVideo = this.grabbedObjectInitialParent.Find("Screen").GetComponent<ControlVideo>();
                if (ctrlVideo.switchOn)
                {
                    this.grabbedObject.transform.position = new Vector3(this.grabbedObject.transform.position.x, this.grabbedObject.transform.position.y, this.grabbedObject.transform.position.z + 0.01f);
                }
                else
                {
                    this.grabbedObject.transform.position = new Vector3(this.grabbedObject.transform.position.x, this.grabbedObject.transform.position.y, this.grabbedObject.transform.position.z - 0.01f);
                }
                ctrlVideo.switchOn = !ctrlVideo.switchOn;
                break;

            case "ScrubbingKnobAudio":
                ControlAudio ctrlAudioScrub = this.grabbedObjectInitialParent.GetComponent<ControlAudio>();
                if (ctrlAudioScrub.switchOn)
                {
                    GameObject playButtonAudio = GameObject.Find("PlayButtonAudio");
                    playButtonAudio.transform.position = new Vector3(playButtonAudio.transform.position.x, playButtonAudio.transform.position.y, playButtonAudio.transform.position.z + 0.01f);
                }
                ctrlAudioScrub.switchOn = false;
                this.lastFrameControllerRotation = this.transform.rotation.z;
                this.lastFrameKnobRotation = this.grabbedObject.transform.rotation.eulerAngles.z;
                //this.srcAudio = grabbedObject.transform.parent.GetComponent<AudioSource>();
                this.srcAudio = grabbedObject.transform.parent.GetComponent<ControlAudio>().audioSource;
                break;

            case "ScrubbingKnobVideo":
                ControlVideo ctrlVideoScrub = this.grabbedObjectInitialParent.Find("Screen").GetComponent<ControlVideo>();
                if (ctrlVideoScrub.switchOn)
                {
                    GameObject playButtonVideo = GameObject.Find("PlayButtonVideo");
                    playButtonVideo.transform.position = new Vector3(playButtonVideo.transform.position.x, playButtonVideo.transform.position.y, playButtonVideo.transform.position.z + 0.01f);
                }
                ctrlVideoScrub.switchOn = false;
                this.lastFrameControllerRotation = this.transform.rotation.z;
                this.lastFrameKnobRotation = this.grabbedObject.transform.rotation.eulerAngles.z;
                this.srcVideo = grabbedObject.transform.parent.Find("Screen").GetComponent<VideoPlayer>();
                break;

            case "mPositive":
                this.bs.MoveRoom(BuildShow.MoveDirection.MPOS);
                OnRelease();
                break;
            case "mNegative":
                this.bs.MoveRoom(BuildShow.MoveDirection.MNEG);
                OnRelease();
                break;
            case "nPositive":
                this.bs.MoveRoom(BuildShow.MoveDirection.NPOS);
                OnRelease();
                break;
            case "nNegative":
                this.bs.MoveRoom(BuildShow.MoveDirection.NNEG);
                OnRelease();
                break;
            default:
                break;
                #endregion immediateInteraction
        }
    }

    private void OnRelease()
    {
        Debug.Log("OnRelease");
        //this.grabbedObject.transform.SetParent(this.grabbedObjectInitialParent);
        this.srcAudio = null;
        this.srcVideo = null;
        this.grabbedObject = null;
        this.rightTriggerActivated = false;
        this.leftTriggerActivated = false;
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
