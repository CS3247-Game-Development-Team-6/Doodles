using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogTarget : MonoBehaviour
{
 
    private Camera cam;
    public Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Right click is detected
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name.Contains("Fog"))
                {
                    Debug.Log("Clicked on " + hit.transform.gameObject.name);
                    // Fog is removed if clicked on
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        // Fog turns red if hovered over with mouse
        renderer.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        // Fog reverts its original color if mouse leaves
        renderer.material.color = new Color32(110, 109, 109, 255);
    }
}
