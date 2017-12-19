using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour {

    LoadPieces lp;

    public SteamVR_TrackedObject controller;
    public GameObject tilePrefab;
    public GameObject cam, scene;


    private int activeIdx;
    private SteamVR_Controller.Device device;
    private Room room;

    int maxX, maxY;
    float tileSizeX, tileSizeY;
    Vector3 center;

List <GameObject> mapTiles;
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
        

        for (int y = 0; y < maxY; y++)
        {
            List<GameObject> tmpVisitors = new List<GameObject>();
            for (int x = 0; x < maxX; x++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(
                    tileSizeX * x,
                    0,
                    tileSizeY  * -y
                    ), Quaternion.identity);
                tile.transform.Translate(center);
                tmpVisitors.Add( tile.transform.GetChild(0).gameObject);
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
            Debug.Log("Grab!!!!!!!");
            foreach (GameObject mt in mapTiles)
            {
                mt.GetComponent<MeshRenderer>().enabled = true;
                UpdateVisitor(room.x, room.y);
                MakeVisotorVisible(room.x, room.y);
            }
        }
        else
        {
            foreach (GameObject mt in mapTiles)
            {
                mt.GetComponent<MeshRenderer>().enabled = false;
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
}
