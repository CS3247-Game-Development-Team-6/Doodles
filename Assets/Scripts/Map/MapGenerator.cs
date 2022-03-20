using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEditor.U2D;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MapGenerator : MonoBehaviour
{
    public float cellSize = 1.0f;
    public int minSpawnToBaseDistance = 15; // careful with increasing this more than 15 for a 10*10 grid,
                                            // may lead to infinite while loop.
    public int maximumTries = 200; // this can be decreased to prevent going for too long
    public int minimumReqScore = 50;
    public int fogTilesClearedByBase = 10;  // change this to clear more fog around the base when starting
    public GameObject mapBase;
    public GameObject tilePrefab;
    public GameObject fogPrefab;
    public GameObject basePrefab;
    public GameObject spawnPrefab;
    public GameObject curvePrefab;
    public GameObject straightPrefab;
    public GameObject waypointPrefab;
    public GameObject waypointEmpty;
    public GameObject playerGameObj;

    private Vector2Int gridSize = new Vector2Int(10, 10);   // current implementation only allows for 10x10
    private int[] spawnCoordinates = new int[2]; 
    private int[] baseCoordinates = new int[2];
    private string[,] backendMatrix;
    private int[,] iterationMatrix;
    
    private Cell[,] cells;
    private int mapWidth;
    private int mapHeight;
    private int manhattanDist;
    List<Cell> curvedCells = new List<Cell>();    // do not know the length (how many waypoints) so we
                                                        // init this as a list and later convert to an array.
                                                        
    private Vector3[] waypointsPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        mapWidth = gridSize.x;
        mapHeight = gridSize.y;

        bool foundGoodPath = false;
        while (!foundGoodPath)  // we try finding paths until we have found a path that satisfies our criteria
        {
            (baseCoordinates, spawnCoordinates) = GenerateSpawnAndBase(mapWidth, mapHeight);
            
            int numberOfTries = 0;
            while (numberOfTries < maximumTries)    // if we go "maximumTries" without finding a path,
                                                    // we change the spawn/base points since they probably lead
                                                    // to a hard time satisfying the criteria.
        {
            // reset the matrix before starting a new "try".
            backendMatrix = CreateMatrix(mapWidth, mapHeight); 
            backendMatrix[spawnCoordinates[1], spawnCoordinates[0]] = "S";
            backendMatrix[baseCoordinates[1], baseCoordinates[0]] = "B";
            

            // variables that are used for generating the path are reset for every try
            int currentX = spawnCoordinates[0];
            int currentY = spawnCoordinates[1];
            bool foundPath = false;
            int lengthOfPath = 0;
            int score = 0;
            int thisMove = 0;
            
            iterationMatrix = new int[mapWidth, mapHeight];
            iterationMatrix[currentY, currentX] = 0;   // just put a zero on the spawn

            for (int _ = 0; _ < 100; _++)   // for every try, we do 100 random moves before either
                                            // succeeding or abandoning this attempt
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
                        
                        lengthOfPath++;
                        iterationMatrix[newY, newX] = lengthOfPath;
                        
                        foundPath = true;
                        break;  // now we are done with this path and have found path from S -> B!
                    } 
                    
                    if (backendMatrix[newY, newX] == "O")
                    {
                        score += ScoreThePath(backendMatrix, newX, newY, mapWidth, mapHeight);
                        currentX = newX;
                        currentY = newY;
                        thisMove = action;
                        
                        lengthOfPath++;
                        iterationMatrix[currentY, currentX] = lengthOfPath;

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
                    
                    // Print2DArray(backendMatrix);

                    foundGoodPath = true;
                    
                    break;  // breaks the while loop of "tries". Have found THE path that will be used for this game
                }
            }
            numberOfTries++;
        }
        }

        Vector3 placeInPosition = transform.position + new Vector3(0.5f, 0, 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = placeInPosition + Vector3.right * c * cellSize;
                var typeOfCell = CellType.NONE;  // default every cell as a NONE-cell
                var rotationINT = 0;
                int pathNum = 0;
                
                var cellCharacter = backendMatrix[c, r];
                if (cellCharacter == "B")
                {
                    typeOfCell = CellType.BASE;
                    pathNum = iterationMatrix[c, r];
                }
                else if (cellCharacter == "S")
                {
                    typeOfCell = CellType.SPAWN;
                    pathNum = iterationMatrix[c, r];
                }
                else if (cellCharacter == "O") typeOfCell = CellType.NONE;
                else
                {
                    (cellCharacter, rotationINT) = ConvertToCell(cellCharacter);
                    if (cellCharacter == "STRAIGHT") typeOfCell = CellType.STRAIGHTPATH;
                    if (cellCharacter == "CURVE")
                    {
                        typeOfCell = CellType.CURVEPATH;
                        pathNum = iterationMatrix[c, r];
                    }
                }

                Vector3 newRotation = new Vector3(0, rotationINT, 0);
                cells[r, c] = new Cell(cellPos, typeOfCell, true, newRotation, pathNum);
                // note: per default we initialise all tiles as foggy, then clear some close to the base.
                
                Cell cell = cells[r, c];

                GameObject tileToPlace = tilePrefab;
                if (typeOfCell == CellType.NONE) tileToPlace = tilePrefab;
                if (typeOfCell == CellType.BASE) {tileToPlace = basePrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.SPAWN) {tileToPlace = spawnPrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.CURVEPATH) {tileToPlace = curvePrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.STRAIGHTPATH) tileToPlace = straightPrefab;
                
                // Instantiate the correct tile for every cell
                GameObject tile = Instantiate(tileToPlace, transform);
                tile.name = $"{tileToPlace.ToString()} {r}, {c}";
                tile.transform.SetParent(mapBase.transform);
                tile.transform.position = cells[r, c].position;
                tile.transform.Rotate(cells[r, c].rotation);
                tile.transform.localScale *= cellSize;
                cell.tile = tile;
                // tile.GetComponent<Node>().cellType = typeOfCell;
                
                // Instantiate the fog tile for every cell
                GameObject fog = Instantiate(fogPrefab, transform);
                fog.name = $"Fog {r}, {c}";
                fog.transform.SetParent(mapBase.transform);
                fog.transform.position = cells[r, c].position;
                fog.transform.localScale *= cellSize;
                cell.fog = fog;
                fog.GetComponent<Fog>().cell = cell;

                if (typeOfCell == CellType.CURVEPATH || typeOfCell == CellType.STRAIGHTPATH)
                // this is to not display all the 2d images used to show where the path is.
                {
                    // set to true now to visualise the path with the new asset
                    tile.SetActive(true);
                }
            }
            placeInPosition += Vector3.forward * cellSize;
        }
        
        // construct all the waypoints that the enemies are using to navigate the path
        var waypointsArr = SetWaypoints(curvedCells.ToArray());
        
        // move the player to a tile nearby the base. 
        playerGameObj.GetComponent<PlayerStartposition>().MovePlayerStartPosition(
            waypointsArr[waypointsArr.Length - 1], waypointsArr[waypointsArr.Length - 2]); 
        
        waypointEmpty.GetComponent<Waypoints>().ActivateWaypoints();
        ClearFogAroundBase(cells, baseCoordinates[1], baseCoordinates[0], mapWidth, mapHeight, 0, fogTilesClearedByBase);
    }

    private void Update()
    {
        for (int r = 0; r < mapWidth; r++) {
            for (int c = 0; c < mapHeight; c++) {
                Cell cell = cells[r, c];
                cell.fog.SetActive(cell.isFog);
            }
        }
    }
    
    private void ClearFogAroundBase(Cell[,] cellMatrix, int x, int y, int w, int h, int numberOfFogCleared, int minCleared)
    {
        int lastX = x;
        int lastY = y;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (y + i >= 0 && y + i < h && x + j >= 0 && x + j < w)   // to avoid index' out of bounds
                {
                    if (cellMatrix[y + i, x + j].isFog && numberOfFogCleared < minCleared) // only do this is there was fog here already
                    {
                        cellMatrix[y + i, x + j].isFog = false;
                        numberOfFogCleared++;

                        lastX = x + j;
                        lastY = y + i;
                    }
                }
            }
        }

        if (numberOfFogCleared < minCleared)
        {
            ClearFogAroundBase(cellMatrix, lastX, lastY, w, h, numberOfFogCleared, minCleared);
        } 
    }

    private (int[], int[]) GenerateSpawnAndBase(int w, int h)
    {
        spawnCoordinates = new int[] {Random.Range(0, w - 1), Random.Range(0, h - 1)} ;
        baseCoordinates = new int[] {Random.Range(0, w - 1), Random.Range(0, h - 1)} ;
        manhattanDist = ManhattanDistance(spawnCoordinates[0], baseCoordinates[0], spawnCoordinates[1],
            baseCoordinates[1]);

        while (manhattanDist < minSpawnToBaseDistance)
        {
            // keep on randomizing coordinates for the enemy spawn and the player base until we have found coords that
            // are far enough away from each other
            spawnCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)};
            baseCoordinates = new int[] {Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)} ;
            manhattanDist = ManhattanDistance(spawnCoordinates[0], baseCoordinates[0], spawnCoordinates[1],
                baseCoordinates[1]);
        }
        
        return (baseCoordinates, spawnCoordinates);
    }
    
    public Vector3[] SetWaypoints(Cell[] waypointCells) {
        // sort array based on their order in the path
        Array.Sort(waypointCells, (oneCell, otherCell) => oneCell.pathOrder.CompareTo(otherCell.pathOrder));

        waypointsPosition = new Vector3[waypointCells.Length];

        GameObject prevWaypoint = null;

        for (int i = 0; i < waypointCells.Length; i++)
        {
            waypointsPosition[i] = waypointCells[i].position;
            GameObject waypoint = Instantiate(waypointPrefab, waypointEmpty.transform, true);
            
            waypoint.name = $"Waypoint {i}";

            // for the very last waypoint (the base), we set the waypoint to be a bit closer to the
            // previous waypoint, for the enemies to target
            waypoint.transform.position = i == waypointCells.Length - 1 ? changeEnemyTargetWaypoint(waypointCells[i].position, prevWaypoint.transform.position) : waypointCells[i].position;
            
            prevWaypoint = waypoint;
        }
        
        return waypointsPosition;
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
        throw new InvalidOperationException("Unintended action");  // this should not happen so we throw error if it would.
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

    private static (string, int) ConvertToCell(string stringAction)
    {
        if (stringAction == "rr" || stringAction == "r") return ("STRAIGHT", 0);
        if (stringAction == "dd" || stringAction == "d") return ("STRAIGHT", 90);
        if (stringAction == "ll" || stringAction == "l") return ("STRAIGHT", 180);
        if (stringAction == "uu" || stringAction == "u") return ("STRAIGHT", 270);

        if (stringAction == "ld" || stringAction == "ur") return ("CURVE", 0);
        if (stringAction == "rd" || stringAction == "ul") return ("CURVE", 90);
        if (stringAction == "dl" || stringAction == "ru") return ("CURVE", 180);
        if (stringAction == "lu" || stringAction == "dr") return ("CURVE", 270);
        
        throw new InvalidOperationException(stringAction);  // this should not happen so we throw error if it would.
    }

    private Vector3 changeEnemyTargetWaypoint(Vector3 basePosition, Vector3 closestWaypointPosition)
    {
        var delta = basePosition - closestWaypointPosition;
        
        delta = delta.normalized;
        delta *= 0.5f;  // this moves the waypoint 0.5 units towards the closest waypoint. can be in-/decreased.
        
        return basePosition - delta;
    }

    /* Grid pattern to visualize Map in editor mode.
    */
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Vector3 position = transform.position;
        Vector3 width = Vector3.right * cellSize * gridSize.x;
        Vector3 height = Vector3.forward * cellSize * gridSize.y;
        for (int r = 0; r <= gridSize.x; r++) {
            Gizmos.DrawLine(position, position + height);
            position += Vector3.right * cellSize;
        }
        position = transform.position;
        for (int c = 0; c <= gridSize.y; c++) {
            Gizmos.DrawLine(position, position + width);
            position += Vector3.forward * cellSize;
        }
    }
}
