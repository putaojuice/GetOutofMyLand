using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    
    private DeckController DeckController;
    public Transform cardGroup;
    public Button spawnButton;
    private GameObject[] currentGrid;
    public Transform enemyPrefab;
    private static int waveIndex = 1;
    public int currentEnemies = waveIndex;
    public int currentHandSize;

    private GameObject currentSpawnPoint;

    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;


    void Start() {
        DeckController = GetComponent<DeckController>();
        DeckController.ShuffleCard();
        DeckController.DrawCard();
        Button btn = spawnButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        generateSpawnPoint();
    }

    void TaskOnClick()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        waveIndex ++;
        currentEnemies = waveIndex;

        for(int i = 0; i < waveIndex; i ++){
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, currentSpawnPoint.transform.position, currentSpawnPoint.transform.rotation);
    }

    public void EnemyDied() {
        currentEnemies--;
        WaveEnded += generateSpawnPoint;
        // Delegate wave end event when all the enemies died
        if (currentEnemies == 0 && WaveEnded != null) {
            WaveEnded();
        }
    }

    // todo: to be implemented/integrated
    // to be trigger after wave (relevant object: start wave button or timer)
    public void AddCard(Card newCard)
    {
        //deck.Add(newCard);
        //ShuffleCard();
    }

    public void CreateNewColourCard()
    {
        GameObject cardObject = DefaultControls.CreateButton(
            new DefaultControls.Resources());
        cardObject.transform.SetParent(cardGroup);
        cardObject.AddComponent<Card>();

        cardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        cardObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 100);
        cardObject.GetComponent<Image>().color = new Color(255, 0, 145);

        cardObject.SetActive(false);
        AddCard(cardObject.GetComponent<Card>());
    }

    public void updateCurrentGrid() {
        currentGrid = GameObject.FindGameObjectsWithTag("GridBase");
    }

    public void generateSpawnPoint() {
        // get a random grid object
        GameObject randomGrid = currentGrid[Random.Range(0, currentGrid.Length)];
        GridBase gridBase = randomGrid.GetComponent<GridBase>();
        if (currentSpawnPoint != null) {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridBase.generateSpawnPoint();
    }


    
}
