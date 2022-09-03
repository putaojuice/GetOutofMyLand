using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{   
    private Card card;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(Card newCard) {
        card = newCard;
    }

    public void RemoveCard() {
        card.gameObject.SetActive(false);
        card = null;
    }

    public bool isOccupied() {
        return card != null;
    }
}
