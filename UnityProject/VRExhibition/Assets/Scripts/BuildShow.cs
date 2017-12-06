using LibVRAusstellung;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using NReco.VideoConverter;
using Valve.VR.InteractionSystem;

// !!! NOTE !!!
// Validation for mIndex and nIndex is missing so it's theoretically possible to move to non-existing rooms
// but no doors to such should be able to spawn so no user interaction should allow this to happen! 

public class BuildShow : MonoBehaviour {
    Exhibition exhibition;
    // FOR PRESENTATION
    List<Piece> pieces;

    private float tileSize;
    public GameObject prefabShowRoom, prefabDoor, dontDestroyOnRoomSwitch;
    public Material matShowRoom, matCanvas;
    public Shader shaderDiffuse; //TEST : Need to attach needed shaders as assets since Shader.Find("XYZ") seems to not function after build

    public GameObject prefabImageCanvas, prefabVideoCanvas, prefabAudioCanvas, prefab3DCanvas, prefabTable;
    public Texture2D audioFallbackTexture;
    public float groundOffset;

    [HideInInspector]
    public Texture2D showRoomDiff;

    private int listIndex, mIndex, nIndex; //m = width, n = height
    private GameObject currentPiece;

    private GameObject sceneControl;
    public Vector3 sceneScale;
    public bool cancelSteamVRTeleportHint; //For cancelling the TeleportHint and Vibration, very annoying for developement.
    private AudioSource srcAudio;
    private MediaType ? mediaType = null;
    private Transform VolumeKnobAudio, VolumeKnobVideo;

    public enum MoveDirection {MPOS, MNEG, NPOS, NNEG}
    enum MediaType {Image, Video, Audio, Model}

    void Awake()
    {
        Application.targetFrameRate = 120; //Max. 120fps for temp reasons
        this.sceneControl = new GameObject();
        this.sceneControl.name = "SceneControl"; //Enable scaling down the entire room for whatever purpose
        //this.sceneScale = new Vector3(0.3f, 0.3f, 0.3f); //Set Scale of scene

        //tileSize = prefabShowRoom.transform.localScale.x;
        //tileSize = prefabShowRoom.GetComponent<Renderer>().bounds.size.x / 1.2f;
        //tileSize = prefabShowRoom.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x / 1.55f;
        tileSize = GameObject.Find("Teleportarea").GetComponent<Renderer>().bounds.size.x / 1.265f;
        //tileSize = GameObject.Find("Teleportarea").GetComponent<Renderer>().bounds.size.x;

        Debug.Log(Directory.GetParent(Application.dataPath));

        #region presentation_hardcoded
        //PRESENTATION HARDCODED
        this.pieces = new List<Piece>();

        Piece piece1 = new Piece();
        piece1.filePath = Directory.GetParent(Application.dataPath) + "/Testdata/Audio/Siegesgeheul.mp3";
        piece1.fileformat = ".mp3";
        piece1.id = 0;
        piece1.title = "Siegesgeheul";
        piece1.description = "Lulululu";
        this.pieces.Add(piece1);

        Piece piece2 = new Piece();
        piece2.filePath = Directory.GetParent(Application.dataPath) + "/Testdata/Images/Ernie.jpg";
        piece2.fileformat = ".jpg";
        piece2.id = 1;
        piece2.title = "Ernie X";
        piece2.description = "Ernie gon' give it to ya!";
        this.pieces.Add(piece2);

        Piece piece3 = new Piece();
        piece3.filePath = Directory.GetParent(Application.dataPath) + "/Testdata/Video/NochnStueck.mp4";
        piece3.fileformat = ".mp4";
        piece3.id = 2;
        piece3.title = "Fahr Weiter";
        piece3.description = "Nochn Stueck, nochn Stueck, nochn Stueck, nochn Stueck...";
        this.pieces.Add(piece3);
        
        Piece piece4 = new Piece();
        piece4.filePath = Directory.GetParent(Application.dataPath) + "/Testdata/Models/mittelfinger.obj";
        piece4.fileformat = ".obj";
        piece4.id = 3;
        piece4.title = "Mittelfinger";
        piece4.description = "Ist der Mittelfinger oben...";
        this.pieces.Add(piece4);

        exhibition = new Exhibition();
        exhibition.width = 2;
        exhibition.height = 2;
        exhibition.pieces = this.pieces;
        exhibition.iconpath = Directory.GetParent(Application.dataPath) + "/Testdata/Images/gray.png";
        #endregion presentation_hardcoded;

        // COMMENTED FOR PRESENTATION exhibition = Exhibition.ReadXMLFile();
        showRoomDiff = LoadPNG(exhibition.iconpath);
        matShowRoom.mainTexture = showRoomDiff;

        this.VolumeKnobAudio = null;
        this.VolumeKnobVideo = null;
        this.srcAudio = null;
    }

    // Use this for initialization
    void Start() {

        //Build whole Show
        /*
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
                        case ".obj":
                            Instantiate3DCanvas(pos, exhibition.pieces[k].filePath);
                            break;
                        default:
                            break;
                    }
                }
                k++;
            }
        }
        */
        //Build only first Room
        this.listIndex = 0;
        this.mIndex = 0;
        this.nIndex = 0;
        this.BuildExhibitionPiece();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(GameObject.Find("TimeMarker").transform.parent.parent.Find("Canvas").GetComponent<ControlAudio>());
        //Could well be inefficient, might reenable for release, annoying for developement though.
        if (cancelSteamVRTeleportHint)
        {
            GameObject.Find("Teleporting").GetComponent<Teleport>().CancelTeleportHint(); //Cancel Teleport Hint (annoying while testing)
        }
        if(this.srcAudio != null)
        {
            if(this.mediaType == MediaType.Video)
            {
                this.srcAudio.volume = this.VolumeKnobVideo.rotation.eulerAngles.z / 360f;
            } else if(this.mediaType == MediaType.Audio)
            {
                this.srcAudio.volume = this.VolumeKnobAudio.rotation.eulerAngles.z / 360f;
            }
        }
    }

    //Switches to room with explicitly defined mIndex and nIndex
    public void SwitchRoom(int mIndex, int nIndex)
    {
        this.mIndex = mIndex;
        this.nIndex = nIndex;
        this.listIndex = nIndex * this.exhibition.width + mIndex;
        Debug.Log(this.mIndex + " " + this.nIndex);
        BuildExhibitionPiece();
    }

    public int[] GetCurrentRoom()
    {
        return new int[] {this.mIndex, this.nIndex};
    }

    //Moves to an adjacent room without the need to know mIndex and nIndex
    public void MoveRoom(BuildShow.MoveDirection md)
    {
        switch (md)
        {
            case MoveDirection.MPOS: this.mIndex++;
                break;
            case MoveDirection.MNEG: this.mIndex--;
                break;
            case MoveDirection.NPOS: this.nIndex++;
                break;
            case MoveDirection.NNEG: this.nIndex--;
                break;
            default:
                break;
        }
        this.listIndex = nIndex * this.exhibition.width + mIndex;
        Debug.Log(this.mIndex + " " + this.nIndex);
        BuildExhibitionPiece();
    }

    public void BuildExhibitionPiece()
    {

        this.sceneControl.transform.localScale = Vector3.one; //Reset scale before adding any objects to sceneControl
        //Destroy objects from previous room if there are any.
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ShowObject"))
        {
            GameObject.Destroy(go);
        }
        //And destroy doors, also
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Door"))
        {
            GameObject.Destroy(go);
        }
        
        if (exhibition.pieces.Count > this.listIndex && exhibition.pieces[this.listIndex].filePath != null)
        {
            Vector3 pos = new Vector3(0f, 0f, 0f);
            Instantiate(prefabShowRoom,
           pos,
           Quaternion.identity
           ).transform.SetParent(this.sceneControl.transform);

            //Place Doors
            if (this.mIndex < this.exhibition.width-1) {
                if ((this.nIndex * this.exhibition.width + this.mIndex + 1) < this.exhibition.pieces.Count) //Check if next place is not empty (lines can be incomplete in exhibition)
                {
                    GameObject doorMP = Instantiate(prefabDoor, new Vector3(pos.x - tileSize / 2, pos.y, pos.z), Quaternion.Euler(0, 0, 0));
                    doorMP.name = "mPositive";
                    doorMP.transform.SetParent(this.sceneControl.transform);
                }
            }
            if (this.mIndex > 0) {
                GameObject doorMN = Instantiate(prefabDoor, new Vector3(pos.x + tileSize / 2, pos.y, pos.z), Quaternion.Euler(0, 180, 0));
                doorMN.name = "mNegative";
                doorMN.transform.SetParent(this.sceneControl.transform);
            }
            if(this.nIndex < this.exhibition.height-1)
            {
                if ((this.nIndex + 1 * this.exhibition.width + this.mIndex) < this.exhibition.pieces.Count) //Check if next place is not empty (lines can be incomplete in exhibition)
                {
                    GameObject doorNP = Instantiate(prefabDoor, new Vector3(pos.x, pos.y, pos.z + tileSize / 2), Quaternion.Euler(0, 270, 0));
                    doorNP.name = "nPositive";
                    doorNP.transform.SetParent(this.sceneControl.transform);
                }
            }
            if(this.nIndex > 0)
            {
                GameObject doorNN = Instantiate(prefabDoor, new Vector3(pos.x, pos.y, pos.z - tileSize/2), Quaternion.Euler(0, 90, 0));
                doorNN.name = "nNegative";
                doorNN.transform.SetParent(this.sceneControl.transform);
            }

            switch (exhibition.pieces[this.listIndex].fileformat)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    this.mediaType = MediaType.Image;
                    InstantiateImageCanvas(pos, exhibition.pieces[this.listIndex].filePath);
                    break;
                case ".mp4":
                    GameObject.Find("Player").transform.position = new Vector3(0, 0, 2);
                    GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, 180, 0);
                    this.mediaType = MediaType.Video;
                    InstantiateVideoCanvas(pos, exhibition.pieces[this.listIndex].filePath);
                    break;
                case ".mp3":
                case ".wav":
                    GameObject.Find("Player").transform.position = new Vector3(0, 0, 2);
                    GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, 180, 0);
                    this.mediaType = MediaType.Audio;
                    InstantiateAudioCanvas(pos, exhibition.pieces[this.listIndex].filePath);
                    break;
                case ".obj":
                    GameObject.Find("Player").transform.position = new Vector3(0, 0, 5);
                    GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, 180, 0);
                    this.mediaType = MediaType.Model;
                    Instantiate3DCanvas(pos, exhibition.pieces[this.listIndex].filePath);
                    break;
                default:
                    GameObject.Find("Player").transform.position = new Vector3(0, 0, 5);
                    GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, 180, 0);
                    this.mediaType = null;
                    break;
            }
        }

        this.sceneControl.transform.localScale = this.sceneScale;
        this.dontDestroyOnRoomSwitch.transform.localScale = this.sceneScale;
    }

    public void InstantiateImageCanvas(Vector3 pos, string filePath) {
        //Material mat = new Material(Shader.Find("Standard"));
        Material mat = this.matCanvas;
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

        this.srcAudio = null;
        stand.transform.SetParent(this.sceneControl.transform);
    }

    public void InstantiateVideoCanvas(Vector3 pos, string filePath)
    {
        GameObject stand = Instantiate(prefabVideoCanvas, pos, Quaternion.identity);
        GameObject table = Instantiate(prefabTable, pos, Quaternion.identity);
        // Transform canvas = stand.transform.Find("Canvas");
        Transform canvas = stand.transform.GetChild(0).Find("Screen");
        // Transform timeLine = stand.transform.Find("TimeLine");
        Transform timeLine = stand.transform.GetChild(0).Find("TimeLine");
        // Vector3 liftUp = canvas.position;
        // liftUp.y = canvas.localScale.z * .5f + groundOffset;
        // canvas.position = liftUp;
        stand.transform.position = new Vector3(0, table.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z, 0);

        // timeLine.position = liftUp + canvas.forward * 1f;

        var videoPlayer = canvas.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = (new WWW(filePath).url);
        videoPlayer.isLooping = true;
        // Thanks to Robert Taylor on https://stackoverflow.com/questions/45532234/unity-videoplayer-is-not-playing-audio
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, canvas.GetComponent<AudioSource>());

       // canvas.localScale = new Vector3(videoPlayer.clip.width, 1, videoPlayer.clip.height).normalized;
        videoPlayer.Play();

        this.srcAudio = canvas.GetComponent<AudioSource>();
        this.VolumeKnobVideo = stand.transform.GetChild(0).Find("VolumeKnobVideo").transform;
        stand.transform.SetParent(this.sceneControl.transform);
        table.transform.SetParent(this.sceneControl.transform);
    }

    private void InstantiateAudioCanvas(Vector3 pos, string filePath)
    {
        //Material mat = new Material(Shader.Find("Standard"));
        Material mat = this.matCanvas;
        Texture2D tex = LoadAudioArtWork(filePath);
        mat.mainTexture = tex;
        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", new Color(.10f, .10f, .10f));

        GameObject stand = Instantiate(prefabAudioCanvas, pos, Quaternion.identity);
        GameObject table = Instantiate(prefabTable, pos, Quaternion.identity);
        //Transform canvas = stand.transform.Find("Canvas");
        Transform canvas = stand.transform.GetChild(0);
        //canvas.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        //Vector3 liftUp = canvas.position;
        //liftUp.y = canvas.localScale.z * .5f + groundOffset;
        //canvas.position = liftUp;
        stand.transform.position = new Vector3(0, table.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z, 0);
        //canvas.GetComponent<Renderer>().material = mat;

        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        StartCoroutine(LoadAudio(audioSource, filePath));

        this.srcAudio = canvas.GetComponent<AudioSource>();
        this.VolumeKnobAudio = stand.transform.GetChild(0).Find("VolumeKnobAudio").transform;
        stand.transform.SetParent(this.sceneControl.transform);
        table.transform.SetParent(this.sceneControl.transform);
    }

    private void Instantiate3DCanvas(Vector3 pos, string filePath)
    {
        // Material mat = new Material(Shader.Find("Diffuse")); //After building, Shader.Find("XYZ") appears to cause problems.
        Material mat = new Material(this.shaderDiffuse); //Using shader from assets instead

        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", new Color(.10f, .10f, .10f));

        GameObject newPiece = Instantiate(prefab3DCanvas, pos, Quaternion.identity);

        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(filePath);
        
        MeshFilter filter = newPiece.GetComponent<MeshFilter>();
        filter.mesh = holderMesh;
        
        newPiece.GetComponent<Renderer>().materials = LoadMaterials(filePath);

        newPiece.AddComponent<BoxCollider>();

        //GameObject stand = Instantiate(prefabAudioCanvas, pos, Quaternion.identity);
        //Transform canvas = stand.transform.Find("Canvas");
        //canvas.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        //Vector3 liftUp = canvas.position;
        //liftUp.y = canvas.localScale.z * .5f + groundOffset;
        //canvas.position = liftUp;
        //newPiece.GetComponent<Renderer>().material = mat;

        this.srcAudio = null;
        newPiece.transform.SetParent(this.sceneControl.transform);
    }

    private Material[] LoadMaterials( string filePath)
    {
       string materialPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)) + ".mtl";
        Debug.Log(materialPath);
        string texPathMarker = "map_Kd";

        List<Material> materials = new List<Material>();
        if (File.Exists(materialPath)) {
            foreach (string line in File.ReadAllLines(materialPath))
            {
                string[] tokens = line.Split(' ');

                if (tokens[0] == texPathMarker)
                {
                    string texFileName = tokens[1];
                    Debug.Log("tex found: " + texFileName);
                    Material newMat = new Material(Shader.Find("Standard"));
                    newMat.name = texFileName;
                    newMat.mainTexture = LoadPNG(Path.Combine(Path.GetDirectoryName(materialPath), texFileName));
                    Debug.Log(Path.Combine(Path.GetDirectoryName(materialPath), texFileName));
                    materials.Add(newMat);
                }
            }
        }
        return materials.ToArray();
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
        if (File.Exists(filePath))
        {
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

            WWW www = new WWW("file:///"+filePath);
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
                audioSource.clip.name = "Audioclip";
            }
        }




    }

    private void FfMpeg_ConvertProgress(object sender, ConvertProgressEventArgs e)
    {
        Console.WriteLine(String.Format("Progress: {0} / {1}\r\n", e.Processed, e.TotalDuration));
    }
}
