using LibVRAusstellung;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using NReco.VideoConverter;

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

        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        StartCoroutine(LoadAudio(audioSource, filePath));
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
            catch (Exception)
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

    IEnumerator LoadAudio(AudioSource audioSource, string filePath)
    {
        //filePath = "https://upload.wikimedia.org/wikipedia/en/5/58/Barbie_girl_aqua.ogg";

        string targetFormat = string.Empty;
        if (Path.GetExtension(filePath) == ".mp3")
        {
            targetFormat = (string)Format.aiff;
            Debug.LogWarning("converting audio file 2 " + targetFormat);
            string oggFilePath = Application.temporaryCachePath
                + Path.GetFileNameWithoutExtension(filePath) + "." + targetFormat;

            FFMpegConverter ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertProgress += FfMpeg_ConvertProgress;
            ffMpeg.ConvertMedia(filePath, oggFilePath, targetFormat);
            filePath = oggFilePath;
        }

        
        WWW www = new WWW(filePath);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("WWW Error: " + www.error);
        }
        else
        {
            Debug.Log("WWW Ok!: " + www.text);
            if (targetFormat == Format.aiff)
            {
                audioSource.clip = www.GetAudioClip(true, true, AudioType.AIFF);
            }
            else {
                audioSource.clip = www.GetAudioClip(true, true, AudioType.UNKNOWN);
            }
        }




    }

    private void FfMpeg_ConvertProgress(object sender, ConvertProgressEventArgs e)
    {
        Console.WriteLine(String.Format("Progress: {0} / {1}\r\n", e.Processed, e.TotalDuration));
    }
}
