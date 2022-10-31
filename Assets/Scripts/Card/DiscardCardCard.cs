using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiscardCardCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardType cardType;
    [SerializeField] private Text cardQtyText;
    [SerializeField] private Text viewDeckCardQtyText;
    [SerializeField] private DeckController DeckController;
    [SerializeField] private GameObject HoverOverlay;
    private bool isEmpty = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void LoadValues()
    {
        cardQtyText.text = viewDeckCardQtyText.text;
        if (cardQtyText.text == "x 0")
        {
            isEmpty = true;
            GetComponent<Button>().interactable = false;
            HoverOverlay.SetActive(false);
        }
    }

    public enum CardType { 
        ILandCard, LLandCard, TLandCard, ZLandCard, SquareLandCard, 
        LightningTowerCard, FireTowerCard, WaterTowerCard 
    };

    public void onClick()
    {
        switch (cardType)
        {
            case CardType.ILandCard:
                DeckController.ReduceILandQty();
                break;
            case CardType.LLandCard:
                DeckController.ReduceLLandQty();
                break;
            case CardType.TLandCard:
                DeckController.ReduceTLandQty();
                break;
            case CardType.ZLandCard:
                DeckController.ReduceZLandQty();
                break;
            case CardType.SquareLandCard:
                DeckController.ReduceSquareLandQty();
                break;
            case CardType.LightningTowerCard:
                DeckController.ReduceLightningTowerQty();
                break;
            case CardType.FireTowerCard:
                DeckController.ReduceFireTowerQty();
                break;
            case CardType.WaterTowerCard:
                DeckController.ReduceWaterTowerQty();
                break;
            default:
                Debug.Log("ERROR: UNKNOWN CARD");
                break;
        }

        DeckController.DeleteCard(cardType.ToString());
        DeckController.EndDiscardPhase();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEmpty) { HoverOverlay.SetActive(true); }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isEmpty) { HoverOverlay.SetActive(false); }
    }
}
