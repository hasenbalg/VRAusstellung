using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Control : MonoBehaviour {

    protected bool isPlaying = false;
    public bool switchOn = false;
    public bool isUpdatingMarkerPos = true;

    protected GameObject timeLine, timeMarker;

    protected void InitMarkers()
    {
        timeLine = transform.parent.Find("TimeLine").gameObject;
        timeMarker = timeLine.transform.Find("TimeMarker").gameObject;
    }

    public void SetMarkerPos(float playProgress)
    {
        if (isUpdatingMarkerPos)
        {
            Vector3 markerPos = markerPos = timeLine.transform.TransformPoint(Vector3.up * (-1));
            if (!System.Single.IsNaN(playProgress))
            {
                markerPos = timeLine.transform.TransformPoint(Vector3.up * (playProgress * 2 - 1));//https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html
            }

            timeMarker.transform.position = markerPos;
        }
    }
}
