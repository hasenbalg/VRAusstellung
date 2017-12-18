using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSelectDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.tag == "Tuer")
                {
                    hit.transform.gameObject.GetComponent<SwitchRoom>().ChangeRoom();
                }
                else if (hit.transform.gameObject.name.Contains("Zeit"))
                {
                    if (hit.transform.gameObject.transform.parent.GetComponent<AudioSource>().isPlaying)
                    {
                        hit.transform.gameObject.transform.parent.GetComponent<AudioSource>().Play();
                    }
                    else {
                        hit.transform.gameObject.transform.parent.GetComponent<AudioSource>().Pause();
                    }
                }
            }
        }

        
    }
}
