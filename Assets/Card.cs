using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image currentCard;
    public CardEffect cardEffect;

    private void Start()
    {
        currentCard = gameObject.GetComponent<Image>();
        gameObject.GetComponent<Button>().onClick.AddListener(UseCard);
    }

    public void UseCard()
    {
        currentCard.gameObject.SetActive(false);
        if (cardEffect) {
            cardEffect.TriggerEffect();
        }
    }
}
