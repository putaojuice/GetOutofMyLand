using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Game managers that handles different controllers
public class GameManager : MonoBehaviour
{   
    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;
    private static int waveIndex = 1;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Button spawnButton;

    private GameObject currentSpawnPoint;
    private DeckController DeckController;
    private int currentEnemies = waveIndex;

    public Transform cardGroup;
    public int currentHandSize;

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

    public void generateSpawnPoint() {
        // get a random grid object
        GameObject[] currentGrid = gameObject.GetComponent<GridController>().getCurrentGrid();
        GameObject randomGrid = currentGrid[Random.Range(0, currentGrid.Length)];
        GridBase gridBase = randomGrid.GetComponent<GridBase>();
        if (currentSpawnPoint != null) {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridBase.generateSpawnPoint();
    }

    IEnumerator SpawnWave()
    {
        waveIndex ++;
        currentEnemies = waveIndex;
        for(int i = 0; i < waveIndex; i ++){
            Instantiate(enemyPrefab, currentSpawnPoint.transform.position, currentSpawnPoint.transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateEnemy() {
        currentEnemies--;
        WaveEnded += generateSpawnPoint;
        // Delegate wave end event when all the enemies died
        if (currentEnemies == 0 && WaveEnded != null) {
            WaveEnded();
        }
    }
    
}
