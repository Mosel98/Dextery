using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    public int CellAmount = 16;
    public float IsoLevel = 0.5f;
    public float NoiseScale = 0.1f;

    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

    [ContextMenu("Build Mesh")]
    public void BuildMesh()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        Chunk chunk = new Chunk(Vector3.zero, CellAmount);
        Cell[,,] cells = chunk.Generate();

        Mesh mesh = CellsToMesh(cells);

        m_meshFilter.sharedMesh = mesh;
    }

    private Mesh CellsToMesh(Cell[,,] _cells)
    {
        List<Triangle> tris = new List<Triangle>();
        Triangle[] triArray;

        for(int z = 0; z < _cells.GetLength(2); z++)
        {
            for(int y = 0; y < _cells.GetLength(1); y++)
            {
                for(int x = 0; x < _cells.GetLength(0); x++)
                {
                    triArray = MarchingCubes.Polygonise(_cells[x, y, z], IsoLevel);
                    if(triArray == null)
                    {
                        continue;
                    }
                    tris.AddRange(triArray);
                }
            }
        }

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        for(int i = 0; i < tris.Count; i++)
        {
            verts.AddRange(tris[i].Points);
            indices.Add(i * 3);
            indices.Add(i * 3 + 1);
            indices.Add(i * 3 + 2);
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
}
