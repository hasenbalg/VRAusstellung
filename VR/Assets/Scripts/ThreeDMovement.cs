using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDMovement : MonoBehaviour {
    
    private void FixedUpdate()
    {
        this.transform.RotateAround(Vector3.zero, new Vector3(0f, 1f, 0f), 0.1f);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.name == "Boden")
        {
            this.transform.Translate(0f, 0.1f, 0f);
        }
    }
}
