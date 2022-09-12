using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public GameObject Hand;
    public GameObject Deck;
    public int maxHandSize = 5;
    public List<Card> deck = new List<Card>();

    private List<Card> usedCards = new List<Card>();
    private GridController GridController;
    private TurretController TurretController;
    private Card currentCard;
    private int currentHandSize = 0;

    // Start is called before the first frame update
    void Start()
    {   
        // All controllers are components of "GameManager"
        GridController = gameObject.GetComponent<GridController>();
        TurretController = gameObject.GetComponent<TurretController>();
        
        // subscribing the DrawCard method to the WaveEnd event so that DrawCard will be called once wave ended
        WaveSpawning.WaveEnded += DrawCard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // to be triggered at the start of the game
    // or todo: whenever a wave is finished
    public void DrawCard() {   
        for (int i = currentHandSize; i < maxHandSize; i++) {
            Card firstCard = GetNextCard();
            Hand = GameObject.Find("PlayerHand");
            firstCard.transform.SetParent(Hand.transform);
            Debug.Log("Drawing card: " + i);
            currentHandSize++;
        }

        // after drawing cards, unsubscribe the DrawCard method from the WaveEnd event to prevent memory leak
        // TODO handle this in a GameEndManager when player loses
        WaveSpawning.WaveEnded -= DrawCard;
        WaveSpawning.WaveEnded += DrawCard;
    }


    public void PlayTileCard(Card card, GameObject prefabPreview) {
        currentCard = card;
        GridController.StartBuild(prefabPreview);
    }

    public void StopPlayCard() {
        currentCard = null;
    }

    public void CompleteCard() {
        Deck = GameObject.Find("CardDeck");
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

}
