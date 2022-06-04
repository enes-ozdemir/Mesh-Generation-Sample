using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    public int worldSizeX = 10;
    public int worldSizeZ = 10;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        StartCoroutine(CreateShape());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    IEnumerator CreateShape()
    {
        _vertices = new Vector3[(worldSizeX + 1) * (worldSizeZ + 1)];

        //Generate vertices
        for (int i = 0, z = 0; z < worldSizeZ + 1; z++)
        {
            for (int x = 0; x < worldSizeX + 1; x++)
            {
                //Noise for the ground
                float a = Random.Range(0.2f, 1.1f);
                float y = Mathf.PerlinNoise(x * a, z * a) * 2f;

                _vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        _triangles = new int[worldSizeX * worldSizeZ * 6];

        int vert = 0;
        int tris = 0;

        //Generate the triangles on the grid X and Z axis
        for (int z = 0; z < worldSizeZ; z++)
        {
            for (int x = 0; x < worldSizeX; x++)
            {
                _triangles[tris] = 0 + vert;
                _triangles[1 + tris] = worldSizeX + 1 + vert;
                _triangles[2 + tris] = 1 + vert;
                _triangles[3 + tris] = 1 + vert;
                _triangles[4 + tris] = worldSizeX + 1 + vert;
                _triangles[5 + tris] = worldSizeX + 2 + vert;

                vert++;
                tris += 6;
                yield return new WaitForSeconds(.01f);
            }

            vert++;
        }
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null) return;

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], .1f);
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    private void CreateShapeManually()
    {
        _vertices = new Vector3[]
        {
            new(0, 0, 0),
            new(0, 0, 1),
            new(1, 0, 0),
            new(1, 0, 1)
        };

        _triangles = new[]
        {
            0, 1, 2,
            1, 3, 2
        };

        _mesh.RecalculateNormals();
    }
}