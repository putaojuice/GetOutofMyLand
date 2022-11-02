using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpellButtons : MonoBehaviour
{
    [SerializeField] private GameObject spellCard;
    [SerializeField] private DeckController deckController;

    public void AddSpellCard()
    {
        GameObject newCard = Instantiate(spellCard);
        newCard.transform.SetParent(deckController.Deck.transform);
        newCard.gameObject.SetActive(false);
        deckController.usedCards.Add(newCard.GetComponent<Card>());
        if (spellCard.name == "ExplosionSpellCard")
        {
            deckController.AddExplosionSpellQty();
        } else if (spellCard.name == "StunSpellCard")
        {
            deckController.AddStunSpellQty();
        } else
        {
            Debug.Log("ERROR: UNKNOWN CARD");
        }
        deckController.DrawCard();
    }
}
