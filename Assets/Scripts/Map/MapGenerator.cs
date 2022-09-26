using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

// DEPRECATED: DO NOT USE/UPDATE
public class MapGenerator : MonoBehaviour
{
    public float cellSize = 1.0f;
    
    // careful with increasing this more than 15 for a 10*10 grid, may lead to infinite while loop.
    private int minSpawnToBaseDistance = 12; 
    
    // this can be decreased to prevent going for too long, this is how many attempts of finding path before finding 
    // new locations for base and enemy spawn.
    [SerializeField] private int maximumTries = 1000; 
    
    // the minimum score of a path for being accepted as a good path, this is based on a heuristic of scoring paths 
    // higher that spread out paths more.
    [SerializeField] private int minimumReqScore = 50;
    
    // change this to clear more fog around the base when starting the game
    [SerializeField] private int fogTilesClearedByBase = 15;
    
    [SerializeField] private GameObject mapBase;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject fogPrefab;
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private GameObject curvePrefab;
    [SerializeField] private GameObject straightPrefab;
    [SerializeField] private GameObject waypointPrefab;
    [SerializeField] private GameObject waypointEmpty;
    [SerializeField] private GameObject playerGameObj;

    private Vector2Int gridSize = new Vector2Int(10, 10);   // NOTE: current implementation only allows for 10x10
    private int[] spawnCoordinates = new int[2]; 
    private int[] baseCoordinates = new int[2];
    private string[,] backendMatrix;
    private int[,] iterationMatrix;
    private Cell[,] cells;
    private Vector3[] waypointsPosition;
    private int mapWidth;
    private int mapHeight;
    private int manhattanDist;

    // do not know the length (how many waypoints) so we init this as a list and later convert to an array.
    private readonly List<Cell> curvedCells = new List<Cell>();    
    
    // Start is called before the first frame update
    void Start()
    {
        mapWidth = gridSize.x;
        mapHeight = gridSize.y;
        
        // a quick check before starting to make sure we are not aiming for an unachievable path length
        var maximumPlausibleDistance = mapWidth + mapHeight - (mapWidth + mapHeight) / 4;
        if ( minSpawnToBaseDistance > maximumPlausibleDistance)
        {
            minSpawnToBaseDistance = maximumPlausibleDistance;
        }

        bool foundGoodPath = false;
        while (!foundGoodPath)  // we try finding paths until we have found a path that satisfies our criteria
        {
            (baseCoordinates, spawnCoordinates) = GenerateSpawnAndBase(mapWidth, mapHeight);
            int numberOfTries = 0;
            
            // if we go "maximumTries" without finding a path, we change the spawn/base points since they probably lead
            // to a hard time satisfying the criteria.
            while (numberOfTries < maximumTries)    
            {
                // reset the matrix before starting a new "try".
                backendMatrix = CreateMatrix(mapWidth, mapHeight); 
                backendMatrix[spawnCoordinates[1], spawnCoordinates[0]] = "S";  // enemy spawn position
                backendMatrix[baseCoordinates[1], baseCoordinates[0]] = "B";    // player base position

                // variables that are used for generating the path are reset for every try
                int currentX = spawnCoordinates[0];
                int currentY = spawnCoordinates[1];
                bool foundPath = false;
                int lengthOfPath = 0;
                int score = 0;
                int thisMove = 0;
            
                iterationMatrix = new int[mapWidth, mapHeight];
                iterationMatrix[currentY, currentX] = 0;   // just put a zero on the spawn

                // for every try, we do 100 random moves before either succeeding or abandoning this attempt
                for (int _ = 0; _ < 100; _++)   
                {
                    int previousX = currentX;
                    int previousY = currentY;

                    int lastMove = thisMove;

                    int action = Random.Range(0, 4);    // there are 4 possible moves: {0:left, 1:up, 2:right, 3:down}

                    int newX = currentX + DeltaPosition(action, "x");
                    int newY = currentY + DeltaPosition(action, "y");

                    // make sure we do not go out of index bounds
                    if (newX >= 0 && newX < mapWidth && newY >= 0 && newY < mapHeight)    
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
                var pathNum = 0;
                
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
                cells[r, c] = new Cell(new Vector2Int(r,c), cellPos, typeOfCell, true, newRotation, pathNum);
                // NOTE: per default we initialise all tiles as foggy, then clear some close to the base.
                
                Cell cell = cells[r, c];

                GameObject tileToPlace = tilePrefab;
                if (typeOfCell == CellType.NONE) tileToPlace = tilePrefab;
                if (typeOfCell == CellType.BASE) {tileToPlace = basePrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.SPAWN) {tileToPlace = spawnPrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.CURVEPATH) {tileToPlace = curvePrefab; curvedCells.Add(cell);}
                if (typeOfCell == CellType.STRAIGHTPATH) tileToPlace = straightPrefab;
                
                // Instantiate the correct tile for every cell
                GameObject tile = Instantiate(tileToPlace, transform);
                tile.name = $"{tileToPlace} {r}, {c}";
                tile.transform.SetParent(mapBase.transform);
                tile.transform.position = cells[r, c].position;
                tile.transform.Rotate(cells[r, c].rotation);
                tile.transform.localScale *= cellSize;
                cell.tile = tile;
                if (tileToPlace.GetComponent<Node>() != null) {
                    tile.GetComponent<Node>().cell = cell;
                }
                
                // Instantiate the fog tile for every cell
                GameObject fog = Instantiate(fogPrefab, transform);
                fog.name = $"Fog {r}, {c}";
                fog.transform.SetParent(mapBase.transform);
                fog.transform.position = cells[r, c].position;
                fog.transform.localScale *= cellSize;
                cell.fog = fog;
                fog.GetComponent<Fog>().cell = cell;
            }
            placeInPosition += Vector3.forward * cellSize;
        }
        
        // construct all the waypoints that the enemies are using to navigate the path
        var waypointsArr = SetWaypoints(curvedCells.ToArray());
        
        // move the player to a tile nearby the base. 
        playerGameObj.GetComponent<PlayerStartposition>().MovePlayerStartPosition(
            waypointsArr[waypointsArr.Length - 1], waypointsArr[waypointsArr.Length - 2]); 
        
        // sets the waypoints and activates, triggers a change of the enemy's spawn position etc.
        waypointEmpty.GetComponent<Waypoints>().ActivateWaypoints();

        // remove fog around the base, the amount of fog is declared in fogTilesClearedByBase.
        ClearFogAroundBase(cells, baseCoordinates[1], baseCoordinates[0], mapWidth, mapHeight, fogTilesClearedByBase);
    }

    private void Update()
    {
        for (int r = 0; r < mapWidth; r++) {
            for (int c = 0; c < mapHeight; c++) {
                Cell cell = cells[r, c];
                if (cell.fog != null) cell.fog.SetActive(cell.isFog);
                if (cell.tile != null) cell.tile.SetActive(!cell.isFog);
            }
        }
    }
    
    private static void ClearFogAroundBase(Cell[,] cellMatrix, int x, int y, int w, int h, int minCleared)
    {
        // clears fog around the base until a minimum amount has been cleared. clears it in a circle with an increasing
        // radius until enough fog have been cleared.
        var numberOfFogCleared = 0;
        var radius = 1;
        while (numberOfFogCleared < minCleared)
        {
            for (int i = -1 * radius; i < radius + 1; i++)
            {
                for (int j = -1 * radius; j < radius + 1; j++)
                {
                    if (y + i >= 0 && y + i < h && x + j >= 0 && x + j < w) 
                        // to avoid index' out of bounds
                    {
                        if (cellMatrix[y + i, x + j].isFog &&
                            numberOfFogCleared < minCleared) 
                            // only do this is there was fog here already
                        {
                            cellMatrix[y + i, x + j].isFog = false;
                            numberOfFogCleared++;
                        }
                    }
                }
            }

            radius++;
        }
    }

    private (int[], int[]) GenerateSpawnAndBase(int w, int h)
    {
        spawnCoordinates = new[] {Random.Range(0, h), Random.Range(0, w)} ;
        baseCoordinates = new[] {Random.Range(0, h/2 - 1), Random.Range(0, w)} ;

        manhattanDist = ManhattanDistance(spawnCoordinates[0], baseCoordinates[0], spawnCoordinates[1],
            baseCoordinates[1]);
        
        // keep on randomizing coordinates for the enemy spawn and the player base using recursion
        // until we have found coords that are far enough away from each other
        return manhattanDist < minSpawnToBaseDistance
            ? GenerateSpawnAndBase(w,h)
            : (baseCoordinates, spawnCoordinates);
    }

    private Vector3[] SetWaypoints(Cell[] waypointCells) {
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
            if (waypoint.transform != null)
                    waypoint.transform.position = i == waypointCells.Length - 1
                        ? changeEnemyTargetWaypoint(waypointCells[i].position, prevWaypoint.transform.position)
                        : waypointCells[i].position;

            prevWaypoint = waypoint;
        }
        
        return waypointsPosition;
    }


    private static string ConvertMoveToMoveStr(int currentMove, int lastMove)
    {
        var strMove = "";
        switch (lastMove)
        {
            case 0:
                strMove += "l";
                break;
            case 1:
                strMove += "u";
                break;
            case 2:
                strMove += "r";
                break;
            case 3:
                strMove += "d";
                break;
        }
        
        switch (currentMove)
        {
            case 0:
                strMove += "l";
                break;
            case 1:
                strMove += "u";
                break;
            case 2:
                strMove += "r";
                break;
            case 3:
                strMove += "d";
                break;
        }
        return strMove;
    }

    private static int DeltaPosition(int action, string axis)
    {
        switch (action) {
            case 0:
                return (axis == "x") ? -1 : 0;  // left
            case 1:
                return (axis == "x") ? 0 : -1;  // up
            case 2:
                return (axis == "x") ? 1 : 0;  // right
            case 3:
                return (axis == "x") ? 0 : 1;  // down
            default:
                throw new InvalidOperationException("Unintended action");  // this should not happen so we throw error if it would.
        }
    }

    [UsedImplicitly]
    private static void Print2DArray(string[,] matrix)
    // can be used to print the backend matrix for an easy visual 
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

    public Cell[,] GetCells()
    {
        return cells;
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
