using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the game scales, may need to change implementation using coroutine
        livesText.text = PlayerStats.Lives.ToString() + " LIVES";
    }
}
