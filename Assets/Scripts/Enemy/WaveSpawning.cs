using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawning : MonoBehaviour
{
    public Transform warriorPrefab;
    public Transform assassinPrefab;
    public Transform healerPrefab;
    public Transform tankPrefab;
    private List<Transform> listOfEnemies = new List<Transform>();

    public Transform spawnPoint;
    public Button spawnButton;
    private static int waveIndex = 1;
    public int currentEnemies = waveIndex;
    
    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;

    void Start() 
    {
        listOfEnemies.Add(warriorPrefab);
        listOfEnemies.Add(assassinPrefab);
        listOfEnemies.Add(healerPrefab);
        listOfEnemies.Add(tankPrefab);
        Button btn = spawnButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

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
        int randIndex = Random.Range(0, 3);
        Instantiate(listOfEnemies[randIndex], spawnPoint.position, spawnPoint.rotation);
        // Instantiate(assassinPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void EnemyDied() {
        currentEnemies--;

        // Delegate wave end event when all the enemies died
        if (currentEnemies == 0 && WaveEnded != null) {
            Debug.Log("Reached");
            WaveEnded();
        }
    }
}
