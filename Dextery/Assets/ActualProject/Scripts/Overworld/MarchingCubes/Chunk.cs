using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    private int cellSize = 1;
    private int cellAmount = 4;
    private Vector3 position;
    private float[,,] cellPoints;

    public Chunk(Vector3 _pos, int _cellAmount)
    {
        position = _pos;
        cellAmount = _cellAmount;
    }

    public Cell[,,] Generate()
    {
        cellPoints = new float[cellAmount + 1, cellAmount + 1, cellAmount + 1];

        Cell[,,] cells = new Cell[cellAmount, cellAmount, cellAmount];

        for(int z = 0; z < cellPoints.GetLength(2); z++)
        {
            for (int y = 0; y < cellPoints.GetLength(1); y++)
            {
                for(int x = 0; x < cellPoints.GetLength(0); x++)
                {
                    // Density Funktion

                    cellPoints[x, y, z] = -y;
                }
            }
        }

        for(int z = 0; z < cells.GetLength(2); z++)
        {
            for(int y = 0; y < cells.GetLength(1); y++)
            {
                for(int x = 0; x < cells.GetLength(0); x++)
                {
                    cells[x, y, z] = new Cell();

                    cells[x, y, z].Points[0] = position + new Vector3(x, y, z + cellSize);
                    cells[x, y, z].Points[1] = position + new Vector3(x + cellSize, y, z + cellSize);
                    cells[x, y, z].Points[2] = position + new Vector3(x + cellSize, y, z);
                    cells[x, y, z].Points[3] = position + new Vector3(x, y, z);
                    cells[x, y, z].Points[4] = position + new Vector3(x, y + cellSize, z + cellSize);
                    cells[x, y, z].Points[5] = position + new Vector3(x + cellSize, y + cellSize, z + cellSize);
                    cells[x, y, z].Points[6] = position + new Vector3(x + cellSize, y + cellSize, z);
                    cells[x, y, z].Points[7] = position + new Vector3(x, y + cellSize, z);

                    cells[x, y, z].Values[0] = cellPoints[x, y, z + 1];
                    cells[x, y, z].Values[1] = cellPoints[x + 1, y, z + 1];
                    cells[x, y, z].Values[2] = cellPoints[x + 1, y, z];
                    cells[x, y, z].Values[3] = cellPoints[x, y, z];
                    cells[x, y, z].Values[4] = cellPoints[x, y + 1, z + 1];
                    cells[x, y, z].Values[5] = cellPoints[x + 1, y + 1, z + 1];
                    cells[x, y, z].Values[6] = cellPoints[x + 1, y + 1, z];
                    cells[x, y, z].Values[7] = cellPoints[x, y + 1, z];
                }
            }
        }

        return cells;
    }
}
