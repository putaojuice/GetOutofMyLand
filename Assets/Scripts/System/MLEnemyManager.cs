using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class MLEnemyManager : Agent
{   
    [Header("Enemy Prefabs")]
    [SerializeField] public GameObject enemyWarrior;
    [SerializeField] public GameObject enemyAssasin;
    [SerializeField] public GameObject enemyHealer;
    [SerializeField] public GameObject enemyTank;
    [SerializeField] public GameObject enemyEater;
    [SerializeField] public GameObject scout;
    [SerializeField] public BehaviorParameters behaviorParameters;
    [SerializeField] public TurretController turretController;

    private List<Transform> listOfEnemies = new List<Transform>();
    private List<GameObject> spawnPointList = new List<GameObject>();
    private List<GameObject> listOfEnemyObjects = new List<GameObject>();
    private int currentEnemies;
    private GameObject currentSpawnPoint;
    private int enemiesKilled;

    private int currentIndex;
    private int numberOfSpawnPoints;
    private int[] spawnPointTowerCount;

    VectorSensorComponent m_TurretSensor;


    public override void Initialize()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
        m_TurretSensor = this.GetComponent<VectorSensorComponent>();
    }

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

    public void initializeFirstSpawnPoint() {
        GridFloor grid = GameObject.FindObjectOfType<GridFloor>();
        GameObject spawnPoint = grid.generateFirstSpawnPoint();
        this.currentSpawnPoint = spawnPoint;
    }

    public void StartWave(int currentIndex)
    {  
        this.currentIndex = currentIndex;
        RequestDecision();
        Academy.Instance.EnvironmentStep();
    }

    public void LoadSpawnPointList(List<GameObject> spawnPointList) {
        this.spawnPointList = spawnPointList;
        this.numberOfSpawnPoints = spawnPointList.Count;
        int[] countArray = new int[numberOfSpawnPoints];
        this.spawnPointTowerCount = countArray;
    }

    void SelectRandomSpawnPoint()
    {
        GameObject randomGrid = this.spawnPointList[Random.Range(0, spawnPointList.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
    }

    public void SelectSmartSpawnPoint()
    {
        int minCount = 99999;
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            if (spawnPointTowerCount[i] < minCount) {
                Debug.Log("Found better spawnpoint: " + i);
                minCount = spawnPointTowerCount[i];
                GameObject spawnPointChosen = spawnPointList[i];
                GridFloor gridFloor = spawnPointChosen.GetComponent<GridFloor>();
                if (this.currentSpawnPoint != null)
                {
                    Destroy(currentSpawnPoint);
                }
                this.currentSpawnPoint = gridFloor.generateSpawnPoint();
            }
        }
    }



    public void ScoutSpawnPoints()
    {
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            GameObject currentSpawnPoint = spawnPointList[i];
            GridFloor floor = currentSpawnPoint.GetComponent<GridFloor>();
            Vector3 currentSpawnPointPosition = new Vector3(floor.transform.position.x, floor.transform.position.y + 0.6f, floor.transform.position.z);
            scout.GetComponent<Scout>().SpawnAt(currentSpawnPoint, i);
        }
    }

    public void UpdateSpawnPointTowerCount(int index) {
        //Debug.Log("UPDATED FOR SPAWNPOINT: " + index);
        
        if (this.spawnPointTowerCount[index] == 0)
        {
            this.spawnPointTowerCount[index] = 1;
        }
        else { 
            this.spawnPointTowerCount[index] += 1; 
        }
        Debug.Log("No. of turrets for SpawnPoint @ index " + index + " is " + spawnPointTowerCount[index]);
    }





    IEnumerator SpawnEnemy()
    {

        int i = 0;
        if(currentIndex % 5 == 0){
            enemyEater.GetComponent<EnemySpawnAgent>().SpawnAt(currentSpawnPoint);
            i ++;
        }

        this.currentEnemies = this.currentIndex;

        for (i = i;i < this.currentIndex; i++)
        {
            int randIndex = Random.Range(0, 3);
            GameObject enemyToSpawn = listOfEnemyObjects[randIndex];
            //Instantiate(enemyToSpawn.transform, tempPosition, currentSpawnPoint.transform.rotation);
            enemyToSpawn.GetComponent<EnemySpawnAgent>().SpawnAt(currentSpawnPoint);

            yield return new WaitForSeconds(1f);
        }

    }



    public bool UpdateEnemy()
    {
        currentEnemies--;
        enemiesKilled++;

        // return true if no more enemies
        if (currentEnemies <= 0)
        {
            return true;
        }

        return false;

    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }


    //ML STUFF
    public override void OnEpisodeBegin()
    {

        // set number of discrete decisions
        ActionSpec actionSpec = this.behaviorParameters.BrainParameters.ActionSpec;
        int[] branchSizes = new int[1];
        branchSizes[0] = numberOfSpawnPoints;
        actionSpec.BranchSizes = branchSizes;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        
    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        int chosenSpawnPoint = actions.DiscreteActions[0];
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
        StartCoroutine(SpawnEnemy());
    }

}
