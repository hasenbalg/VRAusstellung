using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestTheImporter : MonoBehaviour {

    public string path;
    public GameObject prefab3DCanvas;
    // Use this for initialization
    void Start () {

        Instantiate3DCanvas(Vector3.zero, path);

    }
	

    private void Instantiate3DCanvas(Vector3 pos, string filePath)
    {
        Material mat = new Material(Shader.Find("Diffuse"));

        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", new Color(.10f, .10f, .10f));

        GameObject newPiece = Instantiate(prefab3DCanvas, pos, Quaternion.identity);

        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(filePath);

        MeshFilter filter = newPiece.GetComponent<MeshFilter>();
        filter.mesh = holderMesh;

        newPiece.GetComponent<Renderer>().materials = LoadMaterials(filePath);


        //GameObject stand = Instantiate(prefabAudioCanvas, pos, Quaternion.identity);
        //Transform canvas = stand.transform.Find("Canvas");
        //canvas.localScale = new Vector3(tex.width, 1, tex.height).normalized;
        //Vector3 liftUp = canvas.position;
        //liftUp.y = canvas.localScale.z * .5f + groundOffset;
        //canvas.position = liftUp;
        //newPiece.GetComponent<Renderer>().material = mat;


    }
    private Material[] LoadMaterials(string filePath)
    {
        string materialPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)) + ".mtl";
        string texPathMarker = "map_Kd";

        List<Material> materials = new List<Material>();
        if (File.Exists(materialPath))
        {
            foreach (string line in File.ReadAllLines(materialPath))
            {
                string[] tokens = line.Split(' ');

                if (tokens[0] == texPathMarker)
                {
                    string texFileName = tokens[1];
                    Material newMat = new Material(Shader.Find("Standard"));
                    newMat.name = texFileName;
                    newMat.mainTexture = LoadPNG(Path.Combine(Path.GetDirectoryName(materialPath), texFileName));
                    materials.Add(newMat);
                }
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
}
