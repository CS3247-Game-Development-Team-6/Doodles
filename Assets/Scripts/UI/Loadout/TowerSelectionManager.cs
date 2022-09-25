using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerSelectionManager : MonoBehaviour {
    [Header("Cards Parameters")]
    public int cardAmount;
    public GameObject[] cardsGO;
    //public GameObject cardPrefab;
    public Transform cardHolderTransform;

    [Header("Towers Parameters")]
    public List<GameObject> towerCards;

    public Transform selectionTransform;
    //public GameObject selectionCardPrefab;
    public List<GameObject> selectionsCards;
    public List<int> selectedIndexes;

    public Button letsBuildButton;
    public int minCardAllowed;

    void Start() {
        cardAmount = cardsGO.Length;
        selectionsCards = new List<GameObject>();
        towerCards = new List<GameObject>();

        for (int i = 0; i < cardAmount; i++) {
            AddTowerCardSelection(i);
        }
    }

    void Update() {
        letsBuildButton.interactable = towerCards.Count >= minCardAllowed;
    }

    // add to towerinventory
    public void AddTowerCardSelection(int index) {
        // todo: add cards dynamicly
        GameObject card = Instantiate(cardsGO[index], selectionTransform);
        CardManager cardManager = card.GetComponent<CardManager>();
        cardManager.selfGO = cardsGO[index];
        cardManager.towerSelectionManager = this;

        selectionsCards.Add(card);
    }

    public void AddTowerReference(GameObject selfGO, CardManager parentCard = default) {
        AddTowerCard(new List<GameObject>(cardsGO).IndexOf(selfGO), parentCard);
    }

    // add to towerslot
    public void AddTowerCard(int index, CardManager parentCard = default) {
        if (selectedIndexes.Contains(index)) {
            //remove
            int indexPos = selectedIndexes.IndexOf(index);
            GameObject tempRef = towerCards[indexPos];

            towerCards.Remove(tempRef);
            Destroy(tempRef);

            selectedIndexes.Remove(index);

        } else {
            selectedIndexes.Add(index);
            GameObject card = Instantiate(cardsGO[index], cardHolderTransform);
            CardManager cardManager = card.GetComponent<CardManager>();
            cardManager.selfGO = cardsGO[index];
            cardManager.towerSelectionManager = this;
            cardManager.parentCard = parentCard;

            towerCards.Add(card);
        }

    }

    public void StartGame() {
        SceneManager.LoadScene("SpiderScene-prototype");
    }
}
