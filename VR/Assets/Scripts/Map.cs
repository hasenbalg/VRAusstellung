using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LibVRAusstellung; //For Nametags on Map

public class Map : MonoBehaviour {

    LoadPieces lp;

    public SteamVR_TrackedObject controller;
    public GameObject tilePrefab;
    public GameObject cam, scene;
    public Material visitorMaterial;


    private int activeIdx;
    private SteamVR_Controller.Device device;
    private Room room;

    int maxX, maxY;
    float tileSizeX, tileSizeY;
    Vector3 center;

List <GameObject> mapTiles, nameTags, doors;
    List<List<GameObject>> visitors;

    // Use this for initialization
    void Start () {
        lp = GameObject.Find("GameManager").GetComponent<LoadPieces>();
        room = GameObject.Find("GameManager").GetComponent<Room>();

        device = SteamVR_Controller.Input((int)controller.index);

        maxX = lp.GetMaxX();
        maxY = lp.GetMaxY();
        tileSizeX = tilePrefab.GetComponent<Renderer>().bounds.size.x;
        tileSizeY = tilePrefab.GetComponent<Renderer>().bounds.size.z;

        center = new Vector3(
            (tileSizeX * (maxX - 1)) / -2,
            0,
            (tileSizeX * (maxX))
            );


        buildMap();
    }

    private void buildMap()
    {
        mapTiles = new List<GameObject>();
        visitors = new List<List<GameObject>>();
        nameTags = new List<GameObject>();
        doors = new List<GameObject>();

        for (int y = 0; y < maxY; y++)
        {
            List<GameObject> tmpVisitors = new List<GameObject>();
            for (int x = 0; x < maxX; x++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(
                    tileSizeX * x,
                    0,
                    tileSizeY * -y
                    ), Quaternion.identity);
                tile.transform.Translate(center);
                tile.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = 
                    TextHelper.WrapText(18, lp.GetExhibPiece(x, y).title); //Set Nametag
                nameTags.Add(tile.transform.GetChild(1).gameObject); //Add Nametag to list

                GameObject visitor = tile.transform.GetChild(0).gameObject;
                visitor.GetComponent<MeshRenderer>().material = visitorMaterial;
                tmpVisitors.Add(visitor);

                doors.AddRange(RemoveBadDoors(x, maxX, y, maxY, tile));
                tile.transform.SetParent(transform, false);
                mapTiles.Add(tile);
            }
            visitors.Add(tmpVisitors);
        }
    }

    // Update is called once per frame
    void Update () {
        if (device.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            // Debug.Log("Grab!!!!!!!");
            foreach (GameObject mt in mapTiles)
            {
                mt.GetComponent<MeshRenderer>().enabled = true;
                UpdateVisitor(room.x, room.y);
                MakeVisotorVisible(room.x, room.y);
            }
            foreach (GameObject nt in nameTags)
            {
                nt.GetComponent<MeshRenderer>().enabled = true;
            }
            foreach (GameObject d in doors)
            {
                d.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject mt in mapTiles)
            {
                mt.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject nt in nameTags)
            {
                nt.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject d in doors)
            {
                d.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject v in visitors.SelectMany(i => i))
            {
                v.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void UpdateVisitor(int x, int y)
    {
        visitors[y][x].transform.localPosition = new Vector3(cam.transform.position.x, 2.67f, cam.transform.position.z);
        Vector3 newRotation = new Vector3(-90f, cam.transform.eulerAngles.y, 0f);
        visitors[y][x].transform.transform.localEulerAngles = newRotation;
    }

    private void MakeVisotorVisible(int x, int y) {
        foreach (GameObject v in visitors.SelectMany(i => i))
        {
            v.GetComponent<MeshRenderer>().enabled = false;
        }
        visitors[y][x].GetComponent<MeshRenderer>().enabled = true;
        
    }

    private List<GameObject> RemoveBadDoors(int x, int maxX, int y, int maxY, GameObject tile)
    {
        List<GameObject> doors = new List<GameObject>();
        doors.Add(tile.transform.Find("DoorN").gameObject);
        doors.Add(tile.transform.Find("DoorE").gameObject);
        doors.Add(tile.transform.Find("DoorS").gameObject);
        doors.Add(tile.transform.Find("DoorW").gameObject);
        if (y == 0)
        {
            tile.transform.Find("DoorN").GetComponent<MeshRenderer>().enabled = false;
            doors.Remove(tile.transform.Find("DoorN").gameObject);
            // tile.transform.Find("DoorN").gameObject.SetActive(false);
        }

        if (x == maxX - 1)
        {
            tile.transform.Find("DoorE").GetComponent<MeshRenderer>().enabled = false;
            doors.Remove(tile.transform.Find("DoorE").gameObject);
            // tile.transform.Find("DoorE").gameObject.SetActive(false);
        }

        if (y == maxY - 1)
        {
            tile.transform.Find("DoorS").GetComponent<MeshRenderer>().enabled = false;
            doors.Remove(tile.transform.Find("DoorS").gameObject);
            // tile.transform.Find("DoorS").gameObject.SetActive(false);
        }

        if (x == 0)
        {
            tile.transform.Find("DoorW").GetComponent<MeshRenderer>().enabled = false;
            doors.Remove(tile.transform.Find("DoorW").gameObject);
            // tile.transform.Find("DoorW").gameObject.SetActive(false);
        }
        return doors;
    }

}
