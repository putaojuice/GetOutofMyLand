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

    [Header("UI")]
    [SerializeField]private Text infoWaveText;
    [SerializeField]private TextMeshProUGUI gemCount;
    [SerializeField]private GameObject scoreText;
    [SerializeField]private Button spawnButton;
    [SerializeField]private Button wavePauseButton;
    [SerializeField]private GameObject gameOverUI;
    [SerializeField]private GameObject CountDownText;

    [Header("Controllers & Managers")]
    [SerializeField]private DeckController DeckController;
    [SerializeField]private TurretController TurretController;
    [SerializeField]private GridController GridController;
    [SerializeField]private MLEnemyManager EnemyManager;

    [Header("Music")]
    [SerializeField]private MusicControlScript MusicController;
    [SerializeField]private SoundEffectsControlScript SoundEffectsController;

    [Header("Game Info")]
    [SerializeField]public BaseHealth PlayerBase;
    public int waveIndex = 0;
    public Transform cardGroup;
    public int currentHandSize;


    void Start() {
        DeckController.ShuffleCard();
        DeckController.DrawRandomTowerCard();
        DeckController.DrawRandomTowerCard();
        DeckController.DrawCard();
        spawnButton.onClick.AddListener(TaskOnClick);
        wavePauseButton.interactable = false;
        waveIndex = 0;
        EnemyManager.initializeFirstSpawnPoint();
        PlayerBase = GameObject.FindObjectOfType<BaseHealth>();
    }

    // IEnumerator Wait(){
    //     yield return new WaitForSeconds (3);
    //     EnemyManager.SelectSmartSpawnPoint();
    // }

    IEnumerator CountDown (int countDown) {
     int counter = countDown;
     CountDownText.SetActive(true);
     MusicController.fadeAudio();
     MusicController.PlayCountDown();
     spawnButton.interactable = false;
     CountDownText.GetComponent<TextMeshProUGUI>().outlineWidth = 0.2f;
     CountDownText.GetComponent<TextMeshProUGUI>().outlineColor = new Color32(47, 79, 79, 255);
     while (counter > 0) {
         CountDownText.GetComponent<TMP_Text>().text = counter.ToString();
         yield return new WaitForSeconds (1);
         counter--;
     }
     CountDownText.SetActive(false);
     SpawnEnemy();
     
     wavePauseButton.interactable = true;

     //Music Stuff
     MusicController.PlayFunky();
    }

    void TaskOnClick()
    {   
        
        /*
        // get spawnpointlist
        List<GameObject> spawnPointList = gameObject.GetComponent<GridController>().GetPossibleSpawnPointPosition();
        Debug.Log("spawnPointList size: " + spawnPointList.Count);
        // feed spawnpointlist to enemy manager
        EnemyManager.LoadSpawnPointList(spawnPointList);
        // enemy manager scout for optimal spawn point
        EnemyManager.ScoutSpawnPoints();
        */

        StartCoroutine(CountDown(3));

        // get spawnpointlist
 
        List<GameObject> spawnPointList = gameObject.GetComponent<GridController>().GetPossibleSpawnPointPosition();
        Debug.Log("spawnPointList size: " + spawnPointList.Count);
        // feed spawnpointlist to enemy manager
        EnemyManager.LoadSpawnPointList(spawnPointList);
        EnemyManager.ScoutSpawnPoints();


    }

    public void SpawnEnemy() {
        waveIndex++;
        infoWaveText.text = "Wave " + waveIndex.ToString();
        EnemyManager.StartWave(waveIndex);
    }

    public int GetScore()
    {
        int enemiesScore = EnemyManager.GetEnemiesKilled() * (1 + (waveIndex / 60));
        int landScore = (GameObject.FindGameObjectsWithTag("GridFloor").Length - 12) * (1 + (waveIndex / 60));
        return (enemiesScore + landScore);
    }
    
    public void GameOver() {
        int totalScore = GetScore();
        scoreText.GetComponent<TMP_Text>().text = "Score: " + (totalScore); 
        gemCount.text = "X " + (int) ((totalScore) / 30);
        if ((totalScore) / 30 >= 1)
        {
            UpgradeManager.instance.data.upgradePoint += (totalScore) / 40;
            UpgradeManager.instance.Save();
        }
        gameOverUI.SetActive(true);
    }

    public void UpdateWaveInfo() {
        this.SoundEffectsController.PlayEnemyDeathSound();
        int health = PlayerBase.Health;

         // Delegate wave end event when all the enemies died
        if (EnemyManager.UpdateEnemy() && health>0) {
            Debug.Log("Next Wave");
            WaveEnded();

            // enemy manager scout for optimal spawn point
            EnemyManager.SelectSmartSpawnPoint();
            
            // StartCoroutine(Wait());
            

            
            //Music Stuff
            MusicController.PlayAmbient();

            //UI Stuff
            spawnButton.interactable = true;
            wavePauseButton.interactable = false;
            
            //Card Stuff
            if (DeckController.currentCard != null) //still building
            {
                Card currentCard = DeckController.currentCard;
                if (currentCard.type == Type.Turret)
                {
                    TurretController.StopBuild();
                } else if (currentCard.type == Type.Tile)
                {
                    GridController.StopBuild();
                }
            }
            
        }
    }
    
}
