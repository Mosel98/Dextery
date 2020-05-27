﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script by Tamara
public class TerrainManager : MonoBehaviour
{
    public int m_Depth = 15;

    public int m_Width = 129;
    public int m_Height = 129;

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

    // use other functions to build the terrain with given heights and randomness
    public TerrainData GenerateTerrain(TerrainData _terrainData)
    {
        _terrainData.heightmapResolution = m_Width + 1;

        _terrainData.size = new Vector3(m_Width, m_Depth, m_Height);

        _terrainData.SetHeights(0, 0, GenerateHeights());

        return _terrainData;
    }

    // Generate different heights
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

    // get random heights with every start
    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / m_Width * m_Scale + m_OffSetX;
        float yCoord = (float)y / m_Height * m_Scale + m_OffSetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
