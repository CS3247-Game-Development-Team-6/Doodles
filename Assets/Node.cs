using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    private Renderer rend;

    private GameObject tower;

    // Start is called before the first frame update
    void Start()
    {
        rend= GetComponent<Renderer>();
        startColor =rend.material.color;
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return;
        }

        Debug.Log("We are currently here");

        // build a tower
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        Debug.Log(towerToBuild == null);
        Debug.Log(transform.position);
        Debug.Log(transform.rotation);
        tower = (GameObject) Instantiate(towerToBuild, transform.position, transform.rotation);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}