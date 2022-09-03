using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawning : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Button spawnButton;
    private int waveIndex = 0;
    

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

        for(int i = 0; i < waveIndex; i ++){
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
