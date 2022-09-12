using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawning : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Button spawnButton;
    private static int waveIndex = 0;
    public int currentEnemies = waveIndex;
    
    public delegate void WaveEnd();
    public static event WaveEnd WaveEnded;

    void Start() 
    {
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
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
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
