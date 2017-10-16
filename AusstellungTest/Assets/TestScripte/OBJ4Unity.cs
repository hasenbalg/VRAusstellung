using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class OBJ4Unity : MonoBehaviour {
    GameObject go;
    public string path;

    public GameObject LoadOBJFile(string path) {
        go = new GameObject();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        go.GetComponent<MeshFilter>().mesh = new Mesh();

        MeshMapper mm = new MeshMapper();
        foreach (string line in File.ReadAllLines(path)) {
            string[] tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "mtllib":
                    //LoadMaterials(tokens[1]);
                    LoadMaterials(go, Path.Combine(Path.GetDirectoryName(path), tokens[1]));
                    break;
             
                case "v":
                    Vector3 vert = new Vector3(
                       float.Parse(tokens[1]),
                       float.Parse(tokens[2]),
                       float.Parse(tokens[3])
                        );
                    mm.verts.Add(vert);
                   
                    break;
                case "vt":
                    Vector3 uv = new Vector2(
                       float.Parse(tokens[1]),
                       float.Parse(tokens[2])
                        );
                    mm.uvs.Add(uv);
                    break;
                case "vn":
                    Vector3 normal = new Vector3(
                       float.Parse(tokens[1]),
                       float.Parse(tokens[2]),
                       float.Parse(tokens[3])
                        );
                    mm.normals.Add(normal);
                    break;
                case "usemtl":
                    mm.faces.Add(new List<int>());
                    break;
                case "f":
                    //tris
                    //first triangle
                        int a = int.Parse(tokens[1].Split('/')[0]) -1;
                        int b = int.Parse(tokens[2].Split('/')[0]) - 1;
                        int c = int.Parse(tokens[3].Split('/')[0]) - 1;
                    int lastInFacesList = mm.faces.Count - 1;
                    mm.faces[lastInFacesList].Add(c);
                    mm.faces[lastInFacesList].Add(b);
                    mm.faces[lastInFacesList].Add(a);
                    
                    if (tokens.Length == 5)//quads
                    {
                        //second triangle
                        int d = int.Parse(tokens[4].Split('/')[0]) - 1;
                        mm.faces[lastInFacesList].Add(d);
                        mm.faces[lastInFacesList].Add(c);
                        mm.faces[lastInFacesList].Add(a);
                    }


                    break;
                case "s":
                    if (tokens[1] == "1")
                    {
                        //make mesh smooth
                    }
                    break;
                default:
                    break;
            }
        }
        go.GetComponent<MeshFilter>().mesh.SetVertices(mm.verts);
        go.GetComponent<MeshFilter>().mesh.SetNormals(mm.GetNormals());
        go.GetComponent<MeshFilter>().mesh.SetUVs(0, mm.GetUvs());
        int[][] submeshes = mm.faces.Select(l => l.ToArray()).ToArray();
        go.GetComponent<MeshFilter>().mesh.subMeshCount = submeshes.Length;
        for (int i = 0; i < submeshes.Length; i++)
        {
            go.GetComponent<MeshFilter>().mesh.SetTriangles(submeshes[i], i);
        }
        go.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        
        return go;
    }

    private void LoadMaterials(GameObject go,string path)
    {
        Debug.Log("matfile:" + path);
        MaterialMapper matm = new MaterialMapper();
        foreach (string line in File.ReadAllLines(path))
        {
            string[] tokens = line.Split(' ');
            Debug.Log(line);
            switch (tokens[0])
            {
                case "newmtl":
                    matm.mats.Add(new Material(Shader.Find("Standard")));
                    break;
                case "map_Kd":
                    int lastInMatList = matm.mats.Count - 1;
                    string texPath = Path.Combine(Path.GetDirectoryName(path), tokens[1]);
                    Texture2D tex = null;
                    byte[] fileData;

                    if (File.Exists(texPath))
                    {
                        Debug.Log(tokens[1]);
                        fileData = File.ReadAllBytes(texPath);
                        tex = new Texture2D(1, 1);
                        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                    }
                    matm.mats[lastInMatList].mainTexture = tex;
                    break;
            }
        }
        go.GetComponent<MeshRenderer>().materials = matm.mats.ToArray();
    }

    

    private void Start()
    {
         Instantiate( LoadOBJFile(path), Vector3.zero, Quaternion.identity);
    }
}
