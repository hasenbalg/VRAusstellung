using System;
using LibVRAusstellung;
using UnityEngine;

public class Room : MonoBehaviour {

    public int x, y;
    public GameObject doorPrefab;
    private int maxX, maxY;
    private GameObject piece;
    private bool roomNotSetYet = true;



    private void SetupDoors()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tuer"))
        {
            Destroy(go);
        }

        if (x > 0)
        {
            GameObject doorW = Instantiate(doorPrefab, new Vector3(), Quaternion.Euler(new Vector3(0, -90, 0)));
            doorW.GetComponent<SwitchRoom>().x = x-1;
            doorW.GetComponent<SwitchRoom>().y = y;
            doorW.transform.Find("TuerSchild").GetComponent<TextMesh>().text = GetExhibPiece(x - 1, y).title;
        }

        if (x < maxX-1)
        {
            GameObject doorO = Instantiate(doorPrefab, new Vector3(), Quaternion.Euler(new Vector3(0, 90, 0)));
            doorO.GetComponent<SwitchRoom>().x = x + 1;
            doorO.GetComponent<SwitchRoom>().y = y;
            doorO.transform.Find("TuerSchild").GetComponent<TextMesh>().text = GetExhibPiece(x + 1, y).title;

        }

        if (y > 0)
        {
            GameObject doorN = Instantiate(doorPrefab, new Vector3(), Quaternion.Euler(new Vector3(0, 0, 0)));
            doorN.GetComponent<SwitchRoom>().x = x;
            doorN.GetComponent<SwitchRoom>().y = y-1;
            doorN.transform.Find("TuerSchild").GetComponent<TextMesh>().text = GetExhibPiece(x, y-1).title;

        }

        if (y < maxY - 1)
        {
            GameObject doorS = Instantiate(doorPrefab, new Vector3(), Quaternion.Euler(new Vector3(0, 180, 0)));
            doorS.GetComponent<SwitchRoom>().x = x;
            doorS.GetComponent<SwitchRoom>().y = y+1;
            doorS.transform.Find("TuerSchild").GetComponent<TextMesh>().text = GetExhibPiece(x, y +1).title;

        }
    }

    // Update is called once per frame
    void Update () {
        #region DEBUG_KeyboardControls
        if (Input.GetKeyDown(KeyCode.UpArrow)) { SetNewRoom(this.x, this.y++); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { SetNewRoom(this.x, this.y--); }
        #endregion DEBUG_KeyboardControls

        if (GetComponent<LoadPieces>().ExhibitionIsLoaded() && roomNotSetYet)
        {
            maxX = GetComponent<LoadPieces>().GetMaxX();
            maxY = GetComponent<LoadPieces>().GetMaxY();
            int[] initPos = GetComponent<LoadPieces>().GetInitialRoom();
            this.x = initPos[0];
            this.y = initPos[1];
            SetNewRoom();
            roomNotSetYet = false;
        }
	}

    public void SetNewRoom(int x, int y)
    {
        this.x = x;
        this.y = y;
        SetNewRoom();
    }

    public void SetNewRoom()
    {
        Destroy(piece);
        piece = GetComponent<LoadPieces>().GetPiece(x, y);
        SetupDoors();
        GameObject.Find("Hinge").GetComponent<Handout>().ReloadText();
    }


    public Piece GetExhibPiece(int x, int y) {
        return GetComponent<LoadPieces>().GetExhibPiece(x, y);
    }

    public Piece GetExhibPiece()
    {
        return GetExhibPiece( x, y);
    }

    public string GetText(int maxCharOnLine)
    {
        Piece p = GetExhibPiece(x, y);
        return TextHelper.FormatText(p.title, p.description, maxCharOnLine);
    }
}
