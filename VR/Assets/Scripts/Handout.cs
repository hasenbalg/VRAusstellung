using UnityEngine;

public class Handout : MonoBehaviour {

    public SteamVR_TrackedObject controller;
    public float sensibility;
    public int maxCharOnOneLine;

    private int activeIdx;
    private string richText, type;
    private SteamVR_Controller.Device device;
    private Room room;


    void Start()
    {
        device = SteamVR_Controller.Input((int)controller.index);
        room = GameObject.Find("GameManager").GetComponent<Room>();
        richText = room.GetText(maxCharOnOneLine);
        GetComponent<TextMesh>().text = richText;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (!type.ToLower().Contains("text"))
        {
            if (device.GetTouch(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = false;
            }

            //scrolling
            if (device.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                Vector2 touch = device.GetAxis();
                Vector3 currentScrollPos = transform.localPosition;
                currentScrollPos = currentScrollPos + Vector3.up * touch.y * sensibility;
                transform.localPosition = currentScrollPos;
            }
        }
        else {
            //scroll big text
            if (device.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                GameObject txtGo = GameObject.FindGameObjectWithTag("TextPiece");

                Vector2 touch = device.GetAxis();
                Vector3 currentScrollPos = txtGo.transform.localPosition;
                currentScrollPos = currentScrollPos + Vector3.up * touch.y * (sensibility/20);
                txtGo.transform.localPosition = currentScrollPos;
            }
        }
        
    }

    public void ReloadText() {
        richText = room.GetText(maxCharOnOneLine);
        GetComponent<TextMesh>().text = richText;
        transform.localPosition = new Vector3();
        type = room.GetExhibPiece().GetType().ToString();
    }
}
