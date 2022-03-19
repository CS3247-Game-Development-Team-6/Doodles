using UnityEngine;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour
{
    public Text wavesText;

    // everytime this is enabled
    void OnEnable()
    {
        wavesText.text = GameManager.rounds.ToString();
    }

    public void Retry() {

    }

    public void Menu() {
        
    }
}
