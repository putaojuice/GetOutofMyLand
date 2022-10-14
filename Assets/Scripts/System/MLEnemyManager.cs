using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class MLEnemyManager : Agent
{
    [SerializeField] public GameObject enemyWarrior;
    [SerializeField] public GameObject enemyAssasin;
    [SerializeField] public GameObject enemyHealer;
    [SerializeField] public GameObject enemyTank;
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

    public void StartWave(int currentIndex, List<GameObject> spawnPositions)
    {
        Debug.Log("StartWave() CALLED");
        this.spawnPointList = spawnPositions;
        this.currentIndex = currentIndex;
        this.numberOfSpawnPoints = spawnPointList.Count;
        RequestDecision();
        Academy.Instance.EnvironmentStep();
        

    }

    void SelectRandomSpawnPoint()
    {
        Scout();
        GameObject randomGrid = this.spawnPointList[Random.Range(0, spawnPointList.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
    }

    void Scout()
    {
        int[] countArray = new int[numberOfSpawnPoints];
        this.spawnPointTowerCount = countArray;

        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            GameObject currentSpawnPoint = spawnPointList[i];
            GridFloor floor = currentSpawnPoint.GetComponent<GridFloor>();
            Vector3 currentSpawnPointPosition = new Vector3(floor.transform.position.x, floor.transform.position.y + 0.6f, floor.transform.position.z);
            scout.GetComponent<Scout>().SpawnAt(currentSpawnPoint, i);
        }
    }

    public void UpdateSpawnPointTowerCount(int index) {
        Debug.Log("UPDATED FOR SPAWNPOINT: " + index);
        
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

        this.currentEnemies = this.currentIndex;
        int randIndex = Random.Range(0, 3);
        for (int i = 0; i < this.currentIndex; i++)
        {

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
        SelectRandomSpawnPoint();
        StartCoroutine(SpawnEnemy());
    }

}
