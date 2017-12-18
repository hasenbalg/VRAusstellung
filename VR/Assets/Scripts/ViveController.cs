using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViveController : MonoBehaviour {

    public bool triggerButtonDown = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;
    LineRenderer line;
    private bool teleportIsFinished = true;


    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }


    void Start()
    { 
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    void Update()
    {

        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetAxis(triggerButton).x > 0)
        {
            StartCoroutine("DrawLaser");
        }
        if (controller.GetAxis(triggerButton).x == 0)
        {
            teleportIsFinished = true;
        }
       
        HightLightDoor();

    }

    IEnumerator DrawLaser()
    {
        //line.enabled = true;
        while (controller.GetAxis(triggerButton).x > 0)
        {
            Ray ray = new Ray(transform.position + (transform.forward * .2f), transform.forward);

            if (controller.GetAxis(triggerButton).x == 1)
            {
                line.SetWidth(.01f, .01f);
                Teleport(ray);

            }
            else {
                line.SetWidth(.002f, .002f);
                
            }
            
            line.SetPosition(0, ray.origin);
            line.SetPosition(1, ray.GetPoint(100));
            yield return null;
        }
        line.enabled = false;
    }


    private void Teleport(Ray r) {
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 100) && teleportIsFinished)
        {
            if (hit.transform.gameObject.tag == "Tuer")
            {
                hit.transform.gameObject.GetComponent<SwitchRoom>().ChangeRoom();
                teleportIsFinished = false;
            }
        }
    }
    
    private void HightLightDoor()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.tag == "Tuer")
            {
                    hit.transform.gameObject.GetComponent<SwitchRoom>().HightLight();
            }
        }
    }
}
