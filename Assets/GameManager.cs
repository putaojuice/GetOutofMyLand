using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;

    public void ShuffleCard()
    {
        foreach (Card c in deck)
        {
            c.gameObject.SetActive(false);
        }

        System.Random r = new System.Random();

        // Fisher-Yates Shuffle
        for (int n = deck.Count - 1; n > 0; --n)
        {
            int k = r.Next(n + 1);
            Card temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }

        for (int i = 0; i < cardSlots.Length; i++)
        {
            Card randCard = deck[i];

            randCard.gameObject.SetActive(true);
            randCard.transform.position = cardSlots[i].position;
        }
    }
}
