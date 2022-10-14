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
    [SerializeField] public BehaviorParameters behaviorParameters;

    private List<Transform> listOfEnemies = new List<Transform>();
    private List<GameObject> spawnPointList = new List<GameObject>();
    private List<GameObject> listOfEnemyObjects = new List<GameObject>();
    private int currentEnemies;
    private GameObject currentSpawnPoint;
    private int enemiesKilled;

    private int currentIndex;
    private int numberOfSpawnPoints;

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
        Debug.Log("Im here");
        GameObject randomGrid = this.spawnPointList[Random.Range(0, spawnPointList.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint);
        }
        currentSpawnPoint = gridFloor.generateSpawnPoint();
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
        Debug.Log("OnEpisodeBegin CALLED");

        // set number of discrete decisions
        ActionSpec actionSpec = this.behaviorParameters.BrainParameters.ActionSpec;
        int[] branchSizes = new int[1];
        branchSizes[0] = numberOfSpawnPoints;
        actionSpec.BranchSizes = branchSizes;

        // set number of observations



        Debug.Log("End of OnEpisodeBegin()");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("CollectObservations() called");
        sensor.AddObservation(transform.localPosition);
        
    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log("OnActionReceived() called");
        int chosenSpawnPoint = actions.DiscreteActions[0];
        Debug.Log("Number of spoint points: " + numberOfSpawnPoints);
        Debug.Log("chosen this number: " + chosenSpawnPoint);
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristics Called");
        SelectRandomSpawnPoint();
        StartCoroutine(SpawnEnemy());
    }

}
