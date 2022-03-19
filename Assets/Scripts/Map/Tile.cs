using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] noneTileModels;
    public GameObject curvePathModel;
    public GameObject straightPathModel;
    public CellType cellType;
    private GameObject tileModel;

    private void Start()
    {
        GameObject prefab;
        if (cellType == CellType.CURVEPATH) prefab = curvePathModel;
        else if (cellType == CellType.STRAIGHTPATH) prefab = straightPathModel;
        else if (noneTileModels.Length == 0) return;
        else {
            int indexChosen = (int) Random.Range(0, noneTileModels.Length);
            prefab = noneTileModels[indexChosen];
        }
        tileModel = Object.Instantiate(prefab, transform);
    }
}
