using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Game managers that handles different controllers
public class GameManager : MonoBehaviour
{   
    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;
    private int waveIndex = 0;
    
    [SerializeField]public Transform warriorPrefab;
    [SerializeField]public Transform assassinPrefab;
    [SerializeField]public Transform healerPrefab;
    [SerializeField]public Transform tankPrefab;
    private List<Transform> listOfEnemies = new List<Transform>();

    [SerializeField]
    private Button spawnButton;
    // [SerializeField]
    // private Button wavePauseButton;
    [SerializeField] 
    private GameObject gameOverUI;


    private GameObject currentSpawnPoint;
    private DeckController DeckController;
    private int currentEnemies;

    public Transform cardGroup;
    public int currentHandSize;

    public static GameManager instance;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start() {
        DeckController = GetComponent<DeckController>();
        DeckController.ShuffleCard();
        DeckController.DrawCard();
        Button btn = spawnButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        generateSpawnPoint();
        // wavePauseButton.interactable = false;
        listOfEnemies.Add(warriorPrefab);
        listOfEnemies.Add(assassinPrefab);
        listOfEnemies.Add(healerPrefab);
        listOfEnemies.Add(tankPrefab);
        waveIndex = 0;
        currentEnemies = 0;
    }

    void TaskOnClick()
    {
        StartCoroutine(SpawnWave());
        spawnButton.interactable = false;
        DeckController.disableHand();
        // wavePauseButton.interactable = true;

    }

    public void generateSpawnPoint() {
        // get a random grid object
        List<GameObject> currentGrid = gameObject.GetComponent<GridController>().getCurrentGrid();
        GameObject randomGrid = currentGrid[Random.Range(0, currentGrid.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null) {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
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
        int randIndex = Random.Range(0, 3);
        Instantiate(listOfEnemies[randIndex], currentSpawnPoint.transform.position, currentSpawnPoint.transform.rotation);

    }

    public void UpdateEnemy() {
        currentEnemies--;
        WaveEnded += generateSpawnPoint;
        // Delegate wave end event when all the enemies died
        if (currentEnemies == 0 && WaveEnded != null) {
            WaveEnded();
            spawnButton.interactable = true;
            DeckController.enableHand();
            // wavePauseButton.interactable = false;
        }
    }

    public GameObject getCurrentSpawnPoint() {
        return currentSpawnPoint;
    }

    public void GameOver() {
        gameOverUI.SetActive(true);
    }
    
}
