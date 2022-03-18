using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    public Color tooFarColor;
    private Renderer rend;

    private GameObject tower;

    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (buildManager.GetTowerToBuild() == null)
        {
            return;
        }

        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public float TowerCost() {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().Cost;
    }

    public bool HasTower() {
        return tower != null;
    }

    public Turret BuildTower()
    {

        if (buildManager.GetTowerToBuild() == null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return null;
        }

        if (tower != null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return null;
        }

        // build a tower
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        tower = (GameObject) Instantiate(towerToBuild, transform.position, transform.rotation);
        return tower.GetComponent<Turret>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}