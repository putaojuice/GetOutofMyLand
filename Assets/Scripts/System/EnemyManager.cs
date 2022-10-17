using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{    
    [SerializeField] public GameObject enemyWarrior;
    [SerializeField] public GameObject enemyAssasin;
    [SerializeField] public GameObject enemyHealer;
    [SerializeField] public GameObject enemyTank;

    private List<Transform> listOfEnemies = new List<Transform>();
    private List<GameObject> spawnPointList = new List<GameObject>();
    private List<GameObject> listOfEnemyObjects = new List<GameObject>();
    private int currentEnemies;
    private GameObject currentSpawnPoint;
    private int enemiesKilled;


    // Start is called before the first frame update
    void Start()
    {   

        listOfEnemyObjects.Add(enemyWarrior);
        listOfEnemyObjects.Add(enemyAssasin);
        listOfEnemyObjects.Add(enemyHealer);
        listOfEnemyObjects.Add(enemyTank);

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

    void SelectSpawnPoint(List<GameObject> spawnPositions)
    {
        this.spawnPointList = spawnPositions;
        GameObject randomGrid = spawnPositions[Random.Range(0, spawnPositions.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
    }

    IEnumerator SpawnEnemy(int currentIndex)
    {   
        currentEnemies = currentIndex;
        int randIndex = Random.Range(0, 3);
        for(int i = 0; i < currentIndex; i++){
            // Vector3 spawnDirection = endPoint.transform.position - currentSpawnPoint.transform.position;
            // Vector3 upVector = new Vector3(0f, currentSpawnPoint.transform.rotation.y, 0f);
            // Quaternion finalRotation = Quaternion.LookRotation(spawnDirection, upVector);
            // Instantiate(listOfEnemies[randIndex], currentSpawnPoint.transform.position, finalRotation);
            GameObject enemyToSpawn = listOfEnemyObjects[randIndex];
            //Instantiate(enemyToSpawn.transform, tempPosition, currentSpawnPoint.transform.rotation);
            enemyToSpawn.GetComponent<EnemySpawnAgent>().SpawnAt(currentSpawnPoint);

            yield return new WaitForSeconds(1f);
        }
        
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
