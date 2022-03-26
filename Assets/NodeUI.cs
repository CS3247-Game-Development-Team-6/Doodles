using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    private Node target;

    public void SetTarget(Node _target)
    {

        target = _target;

        transform.position = target.GetTowerBuildPosition();

        if (target.hasTowerBuilt())
        {
            Debug.Log("We are currently displaying it here: " + transform.position);
            ui.SetActive(true);
        }
    }

    public void Hide()
    {
        ui.SetActive (false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
