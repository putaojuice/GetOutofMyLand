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

    [SerializeField]private GameObject scoreText;
    [SerializeField]private DeckController DeckController;
    [SerializeField]private MLEnemyManager EnemyManager;
    [SerializeField]private Button spawnButton;
    [SerializeField]private Button wavePauseButton;
    [SerializeField]private GameObject gameOverUI;
    [SerializeField]private MusicControlScript MusicController;
    [SerializeField]private GameObject CountDownText;
    public Transform cardGroup;
    public int currentHandSize;

    public static GameManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        DeckController.ShuffleCard();
        DeckController.DrawRandomTowerCard();
        DeckController.DrawRandomTowerCard();
        DeckController.DrawCard();
<<<<<<< HEAD
        spawnButton.onClick.AddListener(TaskOnClick);
        wavePauseButton.interactable = false;
=======
        Button btn = spawnButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        // wavePauseButton.interactable = false;
        listOfEnemies.Add(warriorPrefab);
        listOfEnemies.Add(assassinPrefab);
        listOfEnemies.Add(healerPrefab);
        listOfEnemies.Add(tankPrefab);
>>>>>>> 4ff872c (change spawn calculation sequence)
        waveIndex = 0;
    }

    IEnumerator CountDown (int countDown) {
     int counter = countDown;
     CountDownText.SetActive(true);
     MusicController.fadeAudio();
     MusicController.PlayCountDown();
     while (counter > 0) {
         CountDownText.GetComponent<TMP_Text>().text = counter.ToString();
         yield return new WaitForSeconds (1);
         counter--;
     }
     CountDownText.SetActive(false);
     SpawnEnemy();
     spawnButton.interactable = false;

     DeckController.disableHand();
     wavePauseButton.interactable = true;

     //Music Stuff
     MusicController.PlayFunky();
    }

    void TaskOnClick()
    {
<<<<<<< HEAD
        // get spawnpointlist
        List<GameObject> spawnPointList = gameObject.GetComponent<GridController>().GetPossibleSpawnPointPosition();
        // feed spawnpointlist to enemy manager
        EnemyManager.LoadSpawnPointList(spawnPointList);
        // enemy manager scout for optimal spawn point
        EnemyManager.ScoutSpawnPoints();
=======
        generateSpawnPoint();
        StartCoroutine(SpawnWave());
        spawnButton.interactable = false;
        DeckController.disableHand();
        // wavePauseButton.interactable = true;
>>>>>>> 4ff872c (change spawn calculation sequence)

        StartCoroutine(CountDown(3));

    }

    public void SpawnEnemy() {
        waveIndex++;
        EnemyManager.StartWave(waveIndex);
    }

    public void GameOver() {
        int enemiesScore = EnemyManager.GetEnemiesKilled() * (1 + (waveIndex / 50));
        int landScore = (GameObject.FindGameObjectsWithTag("GridFloor").Length - 45) * (1 + (waveIndex / 50));
        scoreText.GetComponent<TMP_Text>().text = "Score: " + (enemiesScore + landScore); 
        gameOverUI.SetActive(true);
    }

    public void UpdateWaveInfo() {
         // Delegate wave end event when all the enemies died
        if (EnemyManager.UpdateEnemy()) {
            WaveEnded();
            
            //Music Stuff
            MusicController.PlayAmbient();
            spawnButton.interactable = true;
            DeckController.enableHand();
            wavePauseButton.interactable = false;
        }
    }
    
}
