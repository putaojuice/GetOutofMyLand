using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    
    private DeckController DeckController;
    public Transform cardGroup;

    public int currentHandSize;

    void Start() {
        DeckController = GetComponent<DeckController>();
        DeckController.DrawCard();
    }


    // todo: to be adjusted
    public void AddCard(Card newCard)
    {
        //deck.Add(newCard);
        //ShuffleCard();
    }

    public void CreateNewColourCard()
    {
        GameObject cardObject = DefaultControls.CreateButton(
            new DefaultControls.Resources());
        cardObject.transform.SetParent(cardGroup);
        cardObject.AddComponent<Card>();

        cardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        cardObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 100);
        cardObject.GetComponent<Image>().color = new Color(255, 0, 145);

        cardObject.SetActive(false);
        AddCard(cardObject.GetComponent<Card>());
    }
    //
}
