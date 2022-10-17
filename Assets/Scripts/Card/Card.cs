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
        DeckController = GameManager.instance.gameObject.GetComponent<DeckController>();
    }

    public void UseCard()
    {   
        DeckController.disableHand();
        if (DeckController.currentCard != null)
        {
            Debug.Log("block card");
            return;
        }

        if (isLootCard)
        {
            DeckController.AddCard(this);
            AddCardQty(this.gameObject.name);
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

    private void AddCardQty(string cardName)
    {
        switch (cardName)
        {
            case string a when a.Contains("ILandCard"):
                DeckController.AddILandQty();
                break;
            case string a when a.Contains("LLandCard"):
                DeckController.AddLLandQty();
                break;
            case string a when a.Contains("TLandCard"):
                DeckController.AddTLandQty();
                break;
            case string a when a.Contains("ZLandCard"):
                DeckController.AddZLandQty();
                break;
            case string a when a.Contains("SquareLandCard"):
                DeckController.AddSquareLandQty();
                break;
            case string a when a.Contains("LightningTowerCard"):
                DeckController.AddLightningTowerQty();
                break;
            case string a when a.Contains("FireTowerCard"):
                DeckController.AddFireTowerQty();
                break;
            case string a when a.Contains("WaterTowerCard"):
                DeckController.AddWaterTowerQty();
                break;
            default:
                Debug.Log("ERROR: UNKNOWN CARD");
                break;
        }
    }
}

public enum Type {Tile, Turret};
