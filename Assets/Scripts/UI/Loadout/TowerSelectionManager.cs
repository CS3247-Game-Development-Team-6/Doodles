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
    public int minCardAllowed;

    public static bool isInGame;


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
        letsBuildButton.interactable = towerCards.Count >= minCardAllowed;
    }

    // add to towerinventory
    public void AddSelectionCards(int index) {
        // todo: add cards dynamicly
        GameObject card = Instantiate(cardsGO[index], towerInventory);
        CardManager cardManager = card.GetComponent<CardManager>();
        cardManager.selfGO = cardsGO[index];
        cardManager.towerSelectionManager = this;

        selectionsCards.Add(card);
    }

    public void SelectTower(GameObject selfGO, CardManager parentCard = default) {
        AddTowerCard(new List<GameObject>(cardsGO).IndexOf(selfGO), parentCard);
    }

    // add to towerslot
    private void AddTowerCard(int index, CardManager parentCard = default) {
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
            cardManager.selfGO = cardsGO[index];
            cardManager.towerSelectionManager = this;
            cardManager.parentCard = parentCard;

            towerCards.Add(card);
        }

    }

    public void StartGame() {
        isInGame = true;
        SceneManager.LoadScene("SpiderScene-prototype");
    }


}
