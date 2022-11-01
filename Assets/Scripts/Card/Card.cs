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
    public bool isDiscardCard = false;

    private Image cardImage;
    [SerializeField]
    private Sprite backSprite;
    private Sprite faceSprite;

    private DeckController DeckController;

    private void Start()
    {
        currentCard = gameObject.GetComponent<Image>();
        gameObject.GetComponent<Button>().onClick.AddListener(UseCard);
        DeckController = GameManager.instance.gameObject.GetComponent<DeckController>();
    }

    public void UseCard()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        DeckController.disableHand();
        if (DeckController.currentCard != null)
        {
            Debug.Log("block card");
            return;
        }

        if (isDiscardCard)
        {
            DeckController.UseDiscardCard();
            return;
        }

        if (isLootCard)
        {
            DeckController.AddCard(this);
            AddCardQty(this.gameObject.name);
            return;
        }
        // currentCard.gameObject.SetActive(false);
        if (cardEffect)
        {
            cardEffect.TriggerEffect();
        }

        if (type == Type.Tile)
        {
            DeckController.PlayTileCard(this, prefabPreview);
        }
        else if (type == Type.Turret)
        {
            DeckController.PlayTurretCard(this, prefabPreview);
        } 
        else if (type == Type.Spell)
        {
            DeckController.PlaySpellCard(this, prefabPreview);
        }
        else
        {
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
            case string a when a.Contains("StunSpellCard"):
                DeckController.AddStunSpellQty();
                break;
            case string a when a.Contains("ExplosionSpellCard"):
                DeckController.AddExplosionSpellQty();
                break;
            default:
                Debug.Log("ERROR: UNKNOWN CARD");
                break;
        }
    }

    public void OpenCard()
    {
        cardImage = GetComponent<Image>();
        faceSprite = cardImage.sprite;
        cardImage.sprite = backSprite; // start with card back

        StartCoroutine(RotateCard());
    }

    private IEnumerator RotateCard()
    {
        for (float i = 0f; i <= 360f; i += 10f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                i += 180f; // to prevent reversed image
                cardImage.sprite = faceSprite;
            }
        }
    }
}

public enum Type {Tile, Turret, Spell};
