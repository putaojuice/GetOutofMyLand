using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image currentCard;

    public void UseCard()
    {
        currentCard.gameObject.SetActive(false);
    }
}
