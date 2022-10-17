using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{    
    [SerializeField]public Transform warriorPrefab;
    [SerializeField]public Transform assassinPrefab;
    [SerializeField]public Transform healerPrefab;
    [SerializeField]public Transform tankPrefab;
    [SerializeField]private Transform endPoint;

    private List<Transform> listOfEnemies = new List<Transform>();
    private int currentEnemies;
    private GameObject currentSpawnPoint;
    private int enemiesKilled;


    // Start is called before the first frame update
    void Start()
    {   
        listOfEnemies.Add(warriorPrefab);
        listOfEnemies.Add(assassinPrefab);
        listOfEnemies.Add(healerPrefab);
        listOfEnemies.Add(tankPrefab);
        currentEnemies = 0;
        enemiesKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int currentIndex, List<GameObject> spawnPositions) 
    {   
        SelectSpawnPoint(spawnPositions);
        StartCoroutine(SpawnEnemy(currentIndex));
    }

    IEnumerator SpawnEnemy(int currentIndex)
    {   
        currentEnemies = currentIndex;
        int randIndex = Random.Range(0, 3);
        for(int i = 0; i < currentIndex; i++){
            Vector3 spawnDirection = endPoint.transform.position - currentSpawnPoint.transform.position;
            Vector3 upVector = new Vector3(0f, currentSpawnPoint.transform.rotation.y, 0f);
            Quaternion finalRotation = Quaternion.LookRotation(spawnDirection, upVector);
            Instantiate(listOfEnemies[randIndex], currentSpawnPoint.transform.position, finalRotation);
            yield return new WaitForSeconds(1f);
        }
        
    }

    void SelectSpawnPoint(List<GameObject> spawnPositions) 
    {
        GameObject randomGrid = spawnPositions[Random.Range(0, spawnPositions.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null) {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
    }

    public bool UpdateEnemy() {
        currentEnemies--;
        enemiesKilled++;
        
        // return true if no more enemies
        if (currentEnemies <= 0) {
            return true;
        }

        return false;
       
    }

    public int GetEnemiesKilled() {
        return enemiesKilled;
    }
}
