using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MapGenerator : MonoBehaviour
{
    public float cellSize = 1.0f;
    public int minSpawnToBaseDistance = 15;
    public int maximumTries = 1000; // this can be decreased to prevent going for too long
    public int minimumReqScore = 50;
    
    private Vector2Int gridSize = new Vector2Int(10, 10);   // current implementation only allows for 10x10
    private int[] spawnCoordinates = new int[2]; 
    private int[] baseCoordinates = new int[2];
    private string[,] backendMatrix;
    
    private Cell[,] cells;
    private int mapWidth;
    private int mapHeight;
    private int manhattanDist;

    // Variables that are used for generating the path
    private int numberOfTries = 0;
    private bool foundPath = false;
    private int lengthOfPath = 0;
    private int score = 0;
    private int thisMove = 0;

    // Start is called before the first frame update
    void Start()
    {
        mapWidth = gridSize.x;
        mapHeight = gridSize.y;
        
        spawnCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)} ;
        baseCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)} ;
        manhattanDist = ManhattanDistance(spawnCoordinates[0], baseCoordinates[0], spawnCoordinates[1],
            baseCoordinates[1]);

        while (manhattanDist < minSpawnToBaseDistance)
        {
            // keep on randomizing coordinates for the enemy spawn and the player base until we have found coords that
            // are far enough away from each other
            baseCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)} ;
            spawnCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)};
            manhattanDist = ManhattanDistance(spawnCoordinates[0], baseCoordinates[0], spawnCoordinates[1],
                baseCoordinates[1]);
        }

        backendMatrix = CreateMatrix(mapWidth, mapHeight);
        
        while (numberOfTries < maximumTries)
        {
            // reset the matrix before starting a new "try".
            backendMatrix = CreateMatrix(mapWidth, mapHeight); 
            backendMatrix[spawnCoordinates[1], spawnCoordinates[0]] = "S";
            backendMatrix[baseCoordinates[1], baseCoordinates[0]] = "B";

            int currentX = spawnCoordinates[0];
            int currentY = spawnCoordinates[1];
            
            for (int _ = 0; _ < 100; _++)
            {
                int previousX = currentX;
                int previousY = currentY;

                int lastMove = thisMove;

                int action = Random.Range(0, 3);    // there are 4 possible moves: {left, up, right, down}

                int newX = currentX + DeltaPosition(action, "x");
                int newY = currentY + DeltaPosition(action, "y");

                if (newX >= 0 && newX < mapWidth && newY >= 0 && newY < mapHeight)    
                    // make sure we do not go out of index bounds
                {
                    if (backendMatrix[newY, newX] == "B")
                    {
                        thisMove = action;
                        string strMove = ConvertMoveToMoveStr(thisMove, lastMove);
                        backendMatrix[previousY, previousX] = strMove;

                        foundPath = true;
                        break;  // now we are done with this path and have found path from S -> B!
                    } 
                    
                    if (backendMatrix[newY, newX] == "O")
                    {
                        score += ScoreThePath(backendMatrix, newX, newY, mapWidth, mapHeight);
                        currentX = newX;
                        currentY = newY;
                        backendMatrix[currentY, currentX] = "N";    // "N" == not yet assigned
                        thisMove = action;
                        lengthOfPath++;

                        if (lengthOfPath > 1)
                        {
                            string strMove = ConvertMoveToMoveStr(thisMove, lastMove);
                            backendMatrix[previousY, previousX] = strMove;
                        }
                    }
                }

            }

            if (foundPath)
            {
                if (lengthOfPath <= manhattanDist * 2 && lengthOfPath > manhattanDist * 1.5 && score > minimumReqScore)
                {
                    Print2DArray(backendMatrix);
                    Debug.Log(score);
                    
                    break;  // breaks the while loop of "tries". Have found THE path that will be used for this game
                    
                }

                
            }
            
            numberOfTries++;
        }


        /*

        Vector3 basePos = Vector3.forward;  // stub value indicating base is on north edge
        Vector3 position = transform.position + (Vector3.one * 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = position + Vector3.right * c * cellSize;
                var typeOfCell = CellType.NONE;  // default every cell as a NONE-cell
                if (new Vector2Int(r, c) == baseCoordinates)
                {
                    typeOfCell = CellType.BASE;
                }
                else if (new Vector2Int(r, c) == spawnCoordinates)
                {
                    typeOfCell = CellType.SPAWN;
                }
                cells[r, c] = new Cell(new Vector2Int(r, c), cellPos, typeOfCell, false);
                 
            }
            position += Vector3.forward * cellSize;
        }
        
        */

    }

    private string ConvertMoveToMoveStr(int currentMove, int lastMove)
    {
        string strMove = "";
        if (lastMove == 0)
        {
            strMove += "l";
        } else if (lastMove == 1)
        {
            strMove += "u";
        } else if (lastMove == 2)
        {
            strMove += "r";
        } else if (lastMove == 3)
        {
            strMove += "d";
        }
        
        if (currentMove == 0)
        {
            strMove += "l";
        } else if (currentMove == 1)
        {
            strMove += "u";
        } else if (currentMove == 2)
        {
            strMove += "r";
        } else if (currentMove == 3)
        {
            strMove += "d";
        }

        return strMove;
    }

    private static int DeltaPosition(int action, string axis)
    {
        switch (action)
        {
            case 0:
                return (axis == "x") ? -1 : 0;  // left
            case 1:
                return (axis == "x") ? 0 : -1;  // up
            case 2:
                return (axis == "x") ? 1 : 0;  // right
            case 3:
                return (axis == "x") ? 0 : 1;  // down
        }
        Debug.Log("Unintended action");
        return 0;   // this should not happen
    }

    private static void Print2DArray(string[,] matrix)
    {
        StringBuilder sb = new StringBuilder();

        for (int i=0; i < matrix.GetLength(1); i++)
        {
            for (int j=0; j < matrix.GetLength(0); j++)
            {
                sb.Append(matrix[i,j]);
                sb.Append(' ');				   
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
    
    private static int ManhattanDistance(int x1, int x2, int y1, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private static int ScoreThePath(string[,] matrix, int x, int y, int w, int h)
    {
        var score = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (y + i >= 0 && y + i < h && x + j >= 0 && x + j < w)   // to avoid index' out of bounds
                {
                    
                    if (matrix[y + i, x + j] == "O")
                        // give points for using unused areas. to prevent clustering of paths.
                    {
                        score++;
                    }
                    else
                    {
                        score -= 2;
                    }
                }
            }
        }

        return score;
    }

    private static string[,] CreateMatrix(int w, int h)
    {
        var matrix = new string[w, h]; 

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = "O";  // initialise all values in the backendMatrix to a O, representing None.
            }
        }

        return matrix;
    }


}
