using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverManager : MonoBehaviour
{
    public Text wavesText;
    public GameObject raycastOccluder;

    // everytime this is enabled
    void OnEnable()
    {
        wavesText.text = GameManager.rounds.ToString();
        Time.timeScale = 0f;
        raycastOccluder.SetActive(true);
    }

    public void Retry() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() 
    {
        SceneManager.LoadScene("Menu");
    }
}
