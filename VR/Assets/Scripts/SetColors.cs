using LibVRAusstellung;

using UnityEngine;

public class SetColors : MonoBehaviour {

    public Material floor, door, sky, text, guide, audioTimeLine, videoTimeLine, audioMarker, videoMarker, visitorMarker;

    UnityEngine.Color floorColor;
    UnityEngine.Color doorColor;
    UnityEngine.Color skyColor;
    UnityEngine.Color textColor;
    UnityEngine.Color guideColor;
    UnityEngine.Color audioTimeLineColor;
    UnityEngine.Color videoTimeLineColor;
    UnityEngine.Color audioMarkerColor;
    UnityEngine.Color videoMarkerColor;

    private LoadPieces lp;
    private bool colorsSet = false;

    // Use this for initialization
    void Start () {
        lp = GetComponent<LoadPieces>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!colorsSet)
        {
            Debug.Log("floor " + lp.GetFloorColor());
            Debug.Log("door " + lp.GetDoorColor());
            //floor.SetColor("_Color", lp.GetFloorColor());
            floor.color = lp.GetFloorColor();
            door.color = lp.GetDoorColor();
           
            Camera.main.backgroundColor = lp.GetSkyColor();
            //text.color = lp.GetTextColor();
            audioTimeLine.color = lp.GetAudioTimeLineColor();
            videoTimeLine.color = lp.GetVideoTimeLineColor();
            videoMarker.color = lp.GetVideoTimeLineColor();
            audioMarker.color = lp.GetAudioMarkerColor();
            videoMarker.color = lp.GetVideoMarkerColor();
            visitorMarker.color = lp.GetVisitorMarkerColor();
            colorsSet = true;
        }

        foreach (GameObject ts in GameObject.FindGameObjectsWithTag("Tuerschild"))
        {
            ts.GetComponent<TextMesh>().color = lp.GetGuideColor();
        }
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Text"))
        {
            t.GetComponent<TextMesh>().color = lp.GetTextColor();
        }
        foreach (GameObject tp in GameObject.FindGameObjectsWithTag("TextPiece"))
        {
            tp.GetComponent<TextMesh>().color = lp.GetTextColor();
        }
    }
}
