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
    [SerializeField]private EnemyManager EnemyManager;
    [SerializeField]private Button spawnButton;
    [SerializeField]
    private Button wavePauseButton;
    [SerializeField]private GameObject gameOverUI;
    [SerializeField]private MusicControlScript MusicController;

    public Transform cardGroup;
    public int currentHandSize;

    public static GameManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        DeckController = GetComponent<DeckController>();
        DeckController.ShuffleCard();
        DeckController.DrawCard();
        spawnButton.onClick.AddListener(TaskOnClick);
        wavePauseButton.interactable = false;

        waveIndex = 0;
    }

    void TaskOnClick()
    {   

        SpawnEnemy();
        spawnButton.interactable = false;
        DeckController.disableHand();
        wavePauseButton.interactable = true;

        //Music Stuff
        MusicController.PlayFunky();

    }

    public void SpawnEnemy() {
        // get a random grid object
        List<GameObject> spawnPointList = gameObject.GetComponent<GridController>().GetPossibleSpawnPointPosition();
        waveIndex++;
        EnemyManager.StartWave(waveIndex, spawnPointList);
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
