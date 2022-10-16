using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Game managers that handles different controllers
public class GameManager : MonoBehaviour
{   
    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;
    public int waveIndex = 0;
    
    [SerializeField]public Transform warriorPrefab;
    [SerializeField]public Transform assassinPrefab;
    [SerializeField]public Transform healerPrefab;
    [SerializeField]public Transform tankPrefab;
    [SerializeField]public GameObject MusicControl;
    [SerializeField]private GameObject scoreText;
    [SerializeField]private Transform endPoint;
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
    private static int enemiesKilled;

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
        enemiesKilled = 0;
    }

    void TaskOnClick()
    {
        StartCoroutine(SpawnWave());
        spawnButton.interactable = false;
        DeckController.disableHand();
        // wavePauseButton.interactable = true;

        //Music Stuff
        MusicControlScript MusicController = MusicControl.GetComponent<MusicControlScript>();
        MusicController.PlayFunky();

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
        Vector3 spawnDirection = endPoint.transform.position - currentSpawnPoint.transform.position;
        Vector3 upVector = new Vector3(0f, currentSpawnPoint.transform.rotation.y, 0f);
        Quaternion finalRotation = Quaternion.LookRotation(spawnDirection, upVector);

        // Instantiate(listOfEnemies[randIndex], currentSpawnPoint.transform.position, currentSpawnPoint.transform.rotation);
        Instantiate(listOfEnemies[randIndex], currentSpawnPoint.transform.position, finalRotation );
    }

    public void UpdateEnemy() {
        currentEnemies--;
        enemiesKilled++;
        WaveEnded += generateSpawnPoint;
        // Delegate wave end event when all the enemies died
        if (currentEnemies <= 0 && WaveEnded != null) {
            Debug.Log("wave eneded");
            WaveEnded();
            //Music Stuff
            MusicControlScript MusicController = MusicControl.GetComponent<MusicControlScript>();
            MusicController.PlayAmbient();

            spawnButton.interactable = true;
            DeckController.enableHand();
            // wavePauseButton.interactable = false;
        }
    }

    public GameObject getCurrentSpawnPoint() {
        return currentSpawnPoint;
    }

    public void GameOver() {
        int enemiesScore = enemiesKilled * (1 + (waveIndex / 50));
        int landScore = (GameObject.FindGameObjectsWithTag("GridFloor").Length - 45) * (1 + (waveIndex / 50));
        scoreText.GetComponent<TMP_Text>().text = "Score: " + (enemiesScore + landScore); 
        gameOverUI.SetActive(true);
    }
    
}
