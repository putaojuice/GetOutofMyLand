using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{   
    
    public Image currentCard;
    public CardEffect cardEffect;
    public GameObject prefabPreview;
    public Type type;
    public bool isLootCard = false;

    private DeckController DeckController;

    private int slotIndex;

    private void Start()
    {
        currentCard = gameObject.GetComponent<Image>();
        gameObject.GetComponent<Button>().onClick.AddListener(UseCard);
        DeckController = GameObject.Find("GameManager").GetComponent<DeckController>();
    }

    public void UseCard()
    {
        if (isLootCard)
        {
            DeckController.AddCard(this);
            return;
        }
        // currentCard.gameObject.SetActive(false);
        if (cardEffect) {
            cardEffect.TriggerEffect();
        }

        if (type == Type.Tile) {
            DeckController.PlayTileCard(this, prefabPreview);
        } else if (type == Type.Turret) {
            DeckController.PlayTurretCard(this, prefabPreview);
        } else {
            Debug.Log("Error: Please assign type to card!");
        }
    }
}

public enum Type {Tile, Turret};
