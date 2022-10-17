using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{   

    public GameObject Hand;
    public GameObject Deck;
    public int maxHandSize = 5;
    public List<Card> deck = new List<Card>();
    public List<Card> lootDeck = new List<Card>();

    private GameObject canvas;
    private GameObject LootOverlay;
    private GameObject LootDisplay;

    private List<Card> usedCards = new List<Card>();
    private GridController GridController;
    private TurretController TurretController;
    public Card currentCard;
    private int currentHandSize = 0;

    private int ILandQty = 0;
    [SerializeField] private Text ILandQtyText;
    private int LLandQty = 0;
    [SerializeField] private Text LLandQtyText;
    private int TLandQty = 0;
    [SerializeField] private Text TLandQtyText;
    private int ZLandQty = 0;
    [SerializeField] private Text ZLandQtyText;
    private int SquareLandQty = 0;
    [SerializeField] private Text SquareLandQtyText;
    private int WaterTowerQty = 0;
    [SerializeField] private Text WaterTowerQtyText;
    private int LightningTowerQty = 0;
    [SerializeField] private Text LightningTowerQtyText;
    private int FireTowerQty = 0;
    [SerializeField] private Text FireTowerQtyText;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {   
        // All controllers are components of "GameManager"
        GridController = gameObject.GetComponent<GridController>();
        TurretController = gameObject.GetComponent<TurretController>();

        // subscribing the DrawCard method to the WaveEnd event so that DrawCard will be called once wave ended
        GameManager.WaveEnded += GetRandomLoot;
        canvas = GameObject.FindWithTag("canvas");
        LootOverlay = canvas.transform.Find("AddCardPanel").gameObject;
        LootDisplay = LootOverlay.transform.Find("CardDisplay").gameObject;

        SetInitCardQty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() 
    {
        GameManager.WaveEnded -= GetRandomLoot;
    }


    // to be triggered at the start of the game
    // or todo: whenever a wave is finished
    public void DrawCard() {   
        for (int i = currentHandSize; i < maxHandSize; i++) {
            Card firstCard = GetNextCard();
            firstCard.transform.SetParent(Hand.transform);
            currentHandSize++;
        }

    }


    public void PlayTileCard(Card card, GameObject prefabPreview) {
        currentCard = card;
        GridController.StartBuild(prefabPreview);
    }

    public void StopPlayCard() {
        currentCard = null;
        enableHand();
    }

    public void CompleteCard() {
        currentCard.transform.SetParent(Deck.transform);

        currentCard.gameObject.SetActive(false);
        usedCards.Add(currentCard);
        currentHandSize--;
        StopPlayCard();
    }

    public void PlayTurretCard(Card card, GameObject prefabPreview) {
        currentCard = card;
        TurretController.StartBuild(prefabPreview);
    }


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


    private Card GetNextCard() 
    {
        ReshuffleDeck();
        Card firstCard = deck[0];
        firstCard.gameObject.SetActive(true);
        deck.Remove(firstCard);
        return firstCard;
    }

    private void ReshuffleDeck() 
    {
        // Only reshuffle when deck is empty
        if (deck.Count == 0) {
            deck = new List<Card>(usedCards);
            usedCards = new List<Card>();
            ShuffleCard();
        }
        
    }

    public void GetRandomLoot()
    {   
        LootOverlay.SetActive(true);
        List<Card> currentLootDeck = new List<Card>(lootDeck);

        for (int i = 0; i < 3; ++i)
        {
            int index = Random.Range(0, currentLootDeck.Count);
            GameObject newCard = Instantiate(currentLootDeck[index].gameObject);
            newCard.GetComponent<Card>().isLootCard = true;
            newCard.transform.SetParent(LootDisplay.transform);
            newCard.SetActive(true);
            currentLootDeck.RemoveAt(index);
        }

        // after displaying loot, unsubscribe the GetRandomLoot method from the WaveEnd event to prevent memory leak
        // TODO handle this in a GameEndManager when player loses
    }

    public void AddCard(Card card)
    {
        currentCard = card;
        currentCard.GetComponent<Card>().isLootCard = false;
        currentCard.transform.SetParent(Deck.transform);
        currentCard.gameObject.SetActive(false);
        usedCards.Add(currentCard);

        foreach (Transform child in LootDisplay.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        LootOverlay.SetActive(false);
        StopPlayCard();

        DrawCard();
    }

    public void disableHand() {
        Hand.SetActive(false);
    }

    public void enableHand() {
        Hand.SetActive(true);
    }

    public void DrawRandomTowerCard()
    {
        List<string> towerArray = new List<string> { "WaterTowerCard", "LightningTowerCard", "FireTowerCard" };
        System.Random r = new System.Random();

        // Fisher-Yates Shuffle
        for (int n = towerArray.Count - 1; n > 0; --n)
        {
            int k = r.Next(n + 1);
            string temp = towerArray[n];
            towerArray[n] = towerArray[k];
            towerArray[k] = temp;
        }

        int cardIndex = -1;
        int towerIndex = 0;

        while (cardIndex == -1 && towerIndex != towerArray.Count)
        {
            cardIndex = deck.FindIndex(gameObject => gameObject.name.Contains(towerArray[towerIndex]));
            towerIndex++;
        }

        if (cardIndex == -1 && towerIndex == towerArray.Count) // zero tower cards in deck bruh
        {
            return;
        }

        Card chosenCard = deck[cardIndex];
        chosenCard.gameObject.SetActive(true);
        deck.Remove(chosenCard);
        chosenCard.transform.SetParent(Hand.transform);
        currentHandSize++;
    }

    private void SetInitCardQty()
    {
        AddILandQty();
        AddLLandQty();
        AddTLandQty();
        AddZLandQty();
        AddSquareLandQty();
        AddLightningTowerQty();
        AddFireTowerQty();
        AddWaterTowerQty();
    }

    public void AddILandQty()
    {
        ILandQty += 1;
        ILandQtyText.text = "x " + ILandQty.ToString();
    }

    public void AddLLandQty()
    {
        LLandQty += 1;
        LLandQtyText.text = "x " + LLandQty.ToString();
    }

    public void AddTLandQty()
    {
        TLandQty += 1;
        TLandQtyText.text = "x " + TLandQty.ToString();
    }

    public void AddZLandQty()
    {
        ZLandQty += 1;
        ZLandQtyText.text = "x " + ZLandQty.ToString();
    }

    public void AddSquareLandQty()
    {
        SquareLandQty += 1;
        SquareLandQtyText.text = "x " + SquareLandQty.ToString();
    }

    public void AddFireTowerQty()
    {
        FireTowerQty += 1;
        FireTowerQtyText.text = "x " + FireTowerQty.ToString();
    }

    public void AddLightningTowerQty()
    {
        LightningTowerQty += 1;
        LightningTowerQtyText.text = "x " + LightningTowerQty.ToString();
    }

    public void AddWaterTowerQty()
    {
        WaterTowerQty += 1;
        WaterTowerQtyText.text = "x " + WaterTowerQty.ToString();
    }
}
