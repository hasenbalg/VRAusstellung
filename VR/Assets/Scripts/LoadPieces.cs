using LibVRAusstellung;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Collections;

public class LoadPieces : MonoBehaviour
{

    Exhibition exhib;

    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab, ThreeDPrefab;

    public Shader shaderDiffuse;

    void Start()
    {
        exhib = GetComponent<LoadXML>().GetExhibition();
    }

    private GameObject GeneratePiece(Piece p)
    {
        GameObject go = null;

        if (p is Image)
        {
            go = GeneratePiece(p as Image);
        }
        else if (p is Audio)
        {
            go = GeneratePiece(p as Audio);
        }
        else if (p is Video)
        {
            go = GeneratePiece(p as Video);
        }
        else if (p is ThreeDModel)
        {
            go = GeneratePiece(p as ThreeDModel);
        }
        else
        {
            go = GeneratePiece(p as Text);
        }

        return go;
    }


    private GameObject GeneratePiece(Text p)
    {
        GameObject go = Instantiate(textPrefab, new Vector3(), Quaternion.identity);
        go.transform.Find("Text").GetComponent<TextMesh>().text =
           TextHelper.FormatText(p.title,  p.description, 30);
        return go;
    }

    private GameObject GeneratePiece(Image p)
    {
        Material mat = new Material(Shader.Find("Standard"));
        Texture2D tex = LoadPNG(p.filePath);
        Debug.Log(p.filePath);
        mat.mainTexture = tex;

        GameObject stand = Instantiate(imagePrefab, new Vector3(), Quaternion.identity);
        Debug.Log(stand.transform.Find("Canvas").GetType());
        Transform c = stand.transform.Find("Canvas");
        c.localScale = new Vector3(tex.width, 1, tex.height).normalized  * 0.1f;
        float scaleFactor = p.height / c.GetComponent<MeshRenderer>().bounds.size.y;
        c.localScale *= scaleFactor;
        Vector3 liftUp = c.position;
        liftUp.y = c.GetComponent<MeshRenderer>().bounds.size.y * .5f + 0.3f; //offset
        c.position = liftUp;
        c.GetComponent<Renderer>().material = mat;
        //stand.transform.localScale = new Vector3(p.height * .1f, p.height * .1f, p.height * .1f);
        return stand;
    }

    private GameObject GeneratePiece(Audio p)
    {
        GameObject go = Instantiate(audioPrefab, new Vector3(), Quaternion.identity);

        GameObject timeLine = go.transform.Find("Zeitleiste").gameObject;
        GameObject timeLineMarker = timeLine.transform.Find("Zeiger").gameObject;
        timeLine.GetComponent<MeshRenderer>().materials[0].color = GetAudioTimeLineColor();
        timeLineMarker.GetComponent<MeshRenderer>().materials[0].color = GetAudioMarkerColor();

        StartCoroutine(LoadAudio(go.GetComponent<AudioSource>(), p.filePath));
        return go;
    }

    
    private GameObject GeneratePiece(Video p)
    {
        GameObject go = Instantiate(videoPrefab, new Vector3(), Quaternion.identity);
        GameObject canvas = go.transform.Find("Canvas").gameObject;

        GameObject timeLine = go.transform.Find("Zeitleiste").gameObject;
        GameObject timeLineMarker = timeLine.transform.Find("Zeiger").gameObject;
        timeLine.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", GetVideoTimeLineColor());
        timeLineMarker.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", GetVideoMarkerColor());

        //canvas.transform.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        Vector3 liftUp = canvas.transform.position;
        //liftUp.y = canvas.transform.localScale.z * .5f + groundOffset;
        canvas.transform.position = liftUp;
        var videoPlayer = canvas.GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = (new WWW(p.filePath).url);
        videoPlayer.isLooping = true;
        //videoPlayer.Play();
        //canvas.localScale = new Vector3(videoPlayer.clip.width, 1, videoPlayer.clip.height).normalized;
        //videoPlayer.Play();

        return go;
    }

    private GameObject GeneratePiece(ThreeDModel p)
    {
        Material mat = new Material(shaderDiffuse); //Using shader from assets instead


        // GameObject newPiece = Instantiate(ThreeDPrefab, new Vector3(), Quaternion.identity);
        GameObject newPiece = Instantiate(ThreeDPrefab, Vector3.zero, Quaternion.identity);

        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(p.filePath);

        MeshFilter filter = newPiece.GetComponent<MeshFilter>();
        filter.mesh = holderMesh;

        newPiece.GetComponent<Renderer>().materials = LoadMaterials(p.filePath);

        newPiece.AddComponent<BoxCollider>();

        float desiredHeight = p.height;
        float actualHeight = newPiece.GetComponent<Renderer>().bounds.size.y;
        float factor = desiredHeight / actualHeight;
        newPiece.transform.localScale *= factor;

        return newPiece;
    }

    private Material[] LoadMaterials(string filePath)
    {
        string materialPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)) + ".mtl";
        Debug.Log(materialPath);
        const string texPathMarker = "map_Kd";
        const string newMatMarker = "newmtl";

        //Original List<Material> materials = new List<Material>(); 
        LinkedList<Material> materials = new LinkedList<Material>();
        if (File.Exists(materialPath))
        {
            foreach (string line in File.ReadAllLines(materialPath))
            {
                string[] tokens = line.Split(' ');

                /* Original
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
                */

                switch (tokens[0])
                {
                    case newMatMarker:
                        string matName = String.Join(" ", tokens);
                        matName = matName.Substring(matName.IndexOf(" "));
                        Material newMat = new Material(Shader.Find("Standard"));
                        newMat.name = matName;
                        materials.AddLast(newMat);
                        break;

                    case texPathMarker:
                        string texFileName = tokens[1];
                        Debug.Log("tex found: " + texFileName);
                        materials.Last.Value.mainTexture = LoadPNG(Path.Combine(Path.GetDirectoryName(materialPath), texFileName));
                        Debug.Log(Path.Combine(Path.GetDirectoryName(materialPath), texFileName));
                        break;

                    case "Kd":
                        //Diffuse Color
                        materials.Last.Value.SetColor("_Color", new UnityEngine.Color(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                        break;

                    case "Ke":
                        //Emission
                        materials.Last.Value.SetColor("_EmissionColor", new UnityEngine.Color(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]), 0.5f));
                        break;

                    case "d":
                        //Alpha
                        UnityEngine.Color colWithAlpha = materials.Last.Value.GetColor("_Color");
                        colWithAlpha.a = float.Parse(tokens[1]);
                        materials.Last.Value.SetColor("_Color", colWithAlpha);
                        break;

                    #region unused_cases
                    //These cases are unused as at the time of writing this, I don't know if you can manipulate the material's according properties at runtime from a script.
                    case "Ns":
                        //Specular Exponent
                        break;

                    case "Ka":
                        //Ambient Color
                        break;

                    case "Ks":
                        //Specular Color
                        break;

                    case "Ni":
                        //For refraction
                        break;

                    case "illum":
                        //https://www.opengl.org/discussion_boards/showthread.php/170682-mtl-file-to-opengl
                        //Illumination Modes : 0 Ambient, 1 Ambient+Diffuse, 3 Ambient+Diffuse+Specular
                        break;
                    #endregion unused_cases
                }

                // materials.Add(newMat);
            }
        }
        return materials.ToArray();
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


        WWW www = new WWW("file:///" + filePath);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("WWW Error: " + www.error);
        }
        else
        {
            Debug.Log("WWW Ok!: " + www.text);

            audioSource.clip = www.GetAudioClip(true, true, AudioType.AIFF);

            audioSource.clip.name = "Audioclip";
        }

    }

    public GameObject GetPiece(int x, int y) {
        return GeneratePiece(exhib.pieces[y][x]);
    }

    public int GetMaxX() {
        
        return exhib.pieces[0].Count;
    }
    public int GetMaxY()
    {
        return exhib.pieces.Count;
    }

    public bool ExhibitionIsLoaded() {
        return exhib != null;
    }

    public string GetText(int x, int y)
    {
        return TextHelper.FormatText(exhib.pieces[y][x].title, exhib.pieces[y][x].description, 20);
    }

    public string GetPieceType(int x, int y)
    {
        return exhib.pieces[y][x].GetType().ToString();
    }

    public Piece GetExhibPiece(int x, int y)
    {
        return exhib.pieces[y][x];
    }

    public UnityEngine.Color GetFloorColor() {
        return new UnityEngine.Color(
            exhib.floorColor.R / 255f,
            exhib.floorColor.G / 255f,
            exhib.floorColor.B / 255f
            );
    }

    public UnityEngine.Color GetDoorColor()
    {
        return new UnityEngine.Color(
            exhib.doorColor.R / 255f,
            exhib.doorColor.G / 255f,
            exhib.doorColor.B / 255f
            );
    }
    public UnityEngine.Color GetSkyColor()
    {
        return new UnityEngine.Color(
            exhib.skyColor.R / 255f,
            exhib.skyColor.G / 255f,
            exhib.skyColor.B / 255f
            );
    }
    public UnityEngine.Color GetTextColor()
    {
        return new UnityEngine.Color(
            exhib.textColor.R / 255f,
            exhib.textColor.G / 255f,
            exhib.textColor.B / 255f
            );
    }
    public UnityEngine.Color GetGuideColor()
    {
        return new UnityEngine.Color(
            exhib.guideColor.R / 255f,
            exhib.guideColor.G / 255f,
            exhib.guideColor.B / 255f
            );
    }
    public UnityEngine.Color GetAudioTimeLineColor()
    {
        return new UnityEngine.Color(
            exhib.audioTimeLineColor.R / 255f,
            exhib.audioTimeLineColor.G / 255f,
            exhib.audioTimeLineColor.B / 255f
            );
    }
    public UnityEngine.Color GetVideoTimeLineColor()
    {
        return new UnityEngine.Color(
            exhib.videoTimeLineColor.R / 255f,
            exhib.videoTimeLineColor.G / 255f,
            exhib.videoTimeLineColor.B / 255f
            );
    }

    public UnityEngine.Color GetAudioMarkerColor()
    {
        return new UnityEngine.Color(
            exhib.audioMarkerColor.R / 255f,
            exhib.audioMarkerColor.G / 255f,
            exhib.audioMarkerColor.B / 255f
            );
    }

    public UnityEngine.Color GetVideoMarkerColor()
    {
        return new UnityEngine.Color(
            exhib.videoMarkerColor.R / 255f,
            exhib.videoMarkerColor.G / 255f,
            exhib.videoMarkerColor.B / 255f
            );
    }

    public UnityEngine.Color GetVisitorMarkerColor()
    {
        return new UnityEngine.Color(
            exhib.visitorMarkerColor.R / 255f,
            exhib.visitorMarkerColor.G / 255f,
            exhib.visitorMarkerColor.B / 255f
            );
    }


    public int[] GetInitialRoom()
    {
        for (int i = 0; i < exhib.pieces.Count; i++)
        {
            for (int j = 0; j < exhib.pieces[i].Count; j++)
            {
                if (exhib.pieces[j][i] is Entrance)
                {
                    return new int[] {j, i };
                }
            }
        }
        return new int[2];

    }

}
