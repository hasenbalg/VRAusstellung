using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMapper {

    public List<Vector3> verts;
    public List<Vector2> uvs;
    public List<int[]> triangles;
    public List<Vector3> normals;
    public List<List<int>> faces;

    public MeshMapper() {
        verts = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int[]>();
        normals = new List<Vector3>();
        faces = new List<List<int>>();
    }

    public List<Vector3> GetNormals() {
        if (normals.Count < verts.Count)
        {
            while (normals.Count< verts.Count)
            {
                normals.Add(Vector3.zero);
            }
        return normals;
        }
        return normals.GetRange(0, verts.Count); ;
    }

    public List<Vector2> GetUvs()
    {
        if (uvs.Count > verts.Count)
        {
           
        return uvs.GetRange(0, verts.Count);
        }
        return uvs;
    }
}
