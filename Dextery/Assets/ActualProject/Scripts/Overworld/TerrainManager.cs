using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public int m_Depth = 15;

    public int m_Width = 256;
    public int m_Height = 256;

    public float m_Scale = 10f;

    public float m_OffSetX = 100f;
    public float m_OffSetY = 100f;

    private TerrainData m_terrainData;

    private void Awake()
    {
        m_OffSetX = Random.Range(0f, 100f);
        m_OffSetY = Random.Range(0f, 100f);

        m_terrainData = GetComponent<Terrain>().terrainData;
    }
    

    void Update()
    {
        m_terrainData = GenerateTerrain(m_terrainData);
    }

    public TerrainData GenerateTerrain(TerrainData _terrainData)
    {
        _terrainData.heightmapResolution = m_Width + 1;

        _terrainData.size = new Vector3(m_Width, m_Depth, m_Height);

        _terrainData.SetHeights(0, 0, GenerateHeights());

        return _terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[m_Width, m_Height];

        for(int x = 0; x < m_Width; x++)
        {
            for(int y = 0; y < m_Height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / m_Width * m_Scale + m_OffSetX;
        float yCoord = (float)y / m_Height * m_Scale + m_OffSetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
