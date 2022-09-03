using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public Transform cardGroup;

    public void ShuffleCard()
    {
        System.Random r = new System.Random();

        // Fisher-Yates Shuffle
        for (int n = deck.Count - 1; n > 0; --n)
        {
            int k = r.Next(n + 1);
            Card temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }
    }

    public void DrawCard()
    {
        foreach (Card c in deck)
        {
            c.gameObject.SetActive(false);
        }

        for (int i = 0; i < cardSlots.Length; i++)
        {
            Card randCard = deck[i];

            randCard.gameObject.SetActive(true);
            randCard.transform.position = cardSlots[i].position;
        }
    }

    public void AddCard(Card newCard)
    {
        deck.Add(newCard);
        ShuffleCard();
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
}
