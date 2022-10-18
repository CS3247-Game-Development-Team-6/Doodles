using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerSelectionManager : MonoBehaviour {
    [Header("Cards Parameters")]
    public GameObject[] cardsGO;
    public Transform towerInventory;
    private int cardAmount;
    private List<GameObject> selectionsCards;

    [Header("Towers Parameters")]
    public Transform towerSlot;
    private List<GameObject> towerCards;
    public List<int> selectedIndexes;

    public Button letsBuildButton;
    public int validCardAllowed;
    public string nextSceneName;

    public static bool isInGame;

    [Header("UI Components")]
    [SerializeField] private TowerDescriptionUI towerDescriptionUI;
    [SerializeField] private GameObject loadingText;

    void Start() {
        cardAmount = cardsGO.Length;
        selectionsCards = new List<GameObject>();
        towerCards = new List<GameObject>();
        isInGame = false;

        for (int i = 0; i < cardAmount; i++) {
            AddSelectionCards(i);
        }
    }

    void Update() {
        letsBuildButton.interactable = towerCards.Count == validCardAllowed;
    }

    public void HoverTower(TowerInfo towerInfo, Sprite sprite) {
        towerDescriptionUI.SetInfo(towerInfo, sprite);
    }

    public void UnhoverTower() {
        towerDescriptionUI.ResetInfo();
    }

    // add to towerinventory
    public void AddSelectionCards(int index) {
        GameObject card = Instantiate(cardsGO[index], towerInventory);
        CardManager cardManager = card.GetComponent<CardManager>();
        cardManager.InitCard(cardsGO[index], this);

        selectionsCards.Add(card);
    }

    public void SelectTower(GameObject selfGO) {
        AddTowerCard(new List<GameObject>(cardsGO).IndexOf(selfGO));
    }

    // add to towerslot
    private void AddTowerCard(int index) {
        if (selectedIndexes.Contains(index)) {
            //remove
            int indexPos = selectedIndexes.IndexOf(index);
            GameObject tempRef = towerCards[indexPos];
            towerCards.Remove(tempRef);
            Destroy(tempRef);
            selectedIndexes.Remove(index);

        } else {
            selectedIndexes.Add(index);
            GameObject card = Instantiate(cardsGO[index], towerSlot);
            CardManager cardManager = card.GetComponent<CardManager>();
            cardManager.InitCard(cardsGO[index], this);
            towerCards.Add(card);
        }

    }

    public void StartGame() {
        isInGame = true;
        towerDescriptionUI.enabled = false;
        Shop shop = GetComponentInChildren<Shop>();
        shop.SetDefaultTower(towerCards[0].GetComponent<CardManager>());
        shop.enabled = true;
        loadingText.SetActive(true);
        SceneManager.LoadScene(nextSceneName);
        Destroy(this);
    }


}
