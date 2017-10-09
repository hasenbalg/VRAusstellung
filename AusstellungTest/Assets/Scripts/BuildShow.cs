using LibVRAusstellung;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class BuildShow : MonoBehaviour {
    Exhibition exhibition;
    private float tileSize;
    public GameObject prefabShowRoom;
    public Material matShowRoom;
   

    public GameObject prefabImageCanvas, prefabVideoCanvas, prefabAudioCanvas;
    public Texture2D audioFallbackTexture;
    public float groundOffset;

    [HideInInspector]
    public Texture2D showRoomDiff;


    void Awake()
    {
        tileSize = prefabShowRoom.transform.localScale.x;

        exhibition = Exhibition.ReadXMLFile();
        showRoomDiff = LoadPNG(exhibition.iconpath);
        matShowRoom.mainTexture = showRoomDiff;

        
    }

    // Use this for initialization
    void Start () {
        int k = 0;
        for (int i = 0; i < exhibition.width; i++)
        {
            for (int j = 0; j < exhibition.height; j++)
            {
                if (exhibition.pieces.Count > k && exhibition.pieces[k].filePath != null)
                {
                    Vector3 pos = new Vector3(i * tileSize, 0f, j * tileSize);
                    Instantiate(prefabShowRoom,
                   pos,
                   Quaternion.identity
                   );

                    switch (exhibition.pieces[k].fileformat)
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            InstantiateImageCanvas(pos, exhibition.pieces[k].filePath);
                            break;
                        case ".mp4":
                            InstantiateVideoCanvas(pos, exhibition.pieces[k].filePath);
                            break;
                        case ".mp3":
                        case ".wav":
                            InstantiateAudioCanvas(pos, exhibition.pieces[k].filePath);
                            break;
                        default:
                            break;
                    }
                }
                k++;
            }
        }
	}

    

    // Update is called once per frame
    void Update () {
		
	}

    public void InstantiateImageCanvas(Vector3 pos, string filePath) {
        Material mat = new Material(Shader.Find("Standard"));
        Texture2D tex = LoadPNG(filePath);
        mat.mainTexture = tex;
        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", new Color(.10f, .10f, .10f));

        GameObject stand = Instantiate(prefabImageCanvas, pos, Quaternion.identity);
        Transform canvas = stand.transform.Find("Canvas");
        canvas.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        Vector3 liftUp = canvas.position;
        liftUp.y = canvas.localScale.z * .5f + groundOffset;
        canvas.position = liftUp;
        canvas.GetComponent<Renderer>().material = mat;
    }

    public void InstantiateVideoCanvas(Vector3 pos, string filePath)
    {
        GameObject stand = Instantiate(prefabVideoCanvas, pos, Quaternion.identity);
        Transform canvas = stand.transform.Find("Canvas");
        Transform timeLine = stand.transform.Find("TimeLine");
        Vector3 liftUp = canvas.position;
        liftUp.y = canvas.localScale.z * .5f + groundOffset;
        canvas.position = liftUp;

        timeLine.position = liftUp + canvas.forward * 1f;

        var videoPlayer = canvas.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = (new WWW(filePath).url);
        videoPlayer.isLooping = true;
       // canvas.localScale = new Vector3(videoPlayer.clip.width, 1, videoPlayer.clip.height).normalized;
        videoPlayer.Play();
    }

    private void InstantiateAudioCanvas(Vector3 pos, string filePath)
    {
        Material mat = new Material(Shader.Find("Standard"));
        Texture2D tex = LoadAudioArtWork(filePath);
        mat.mainTexture = tex;
        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", new Color(.10f, .10f, .10f));

        GameObject stand = Instantiate(prefabAudioCanvas, pos, Quaternion.identity);
        Transform canvas = stand.transform.Find("Canvas");
        canvas.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        Vector3 liftUp = canvas.position;
        liftUp.y = canvas.localScale.z * .5f + groundOffset;
        canvas.position = liftUp;
        canvas.GetComponent<Renderer>().material = mat;
    }

    private Texture2D LoadAudioArtWork(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            try
            {
                fileData = TagLib.File.Create(filePath).Tag.Pictures[0].Data.Data;
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
            }
            catch (Exception e)
            {
                Debug.LogWarning("No Artwork found on track " + filePath + " .");
                tex = audioFallbackTexture;
            }
             return tex;
        }
        return tex;
    }

    public static Texture2D LoadPNG(string filePath)
    {
        //http://answers.unity3d.com/answers/802424/view.html
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

   
    
}
