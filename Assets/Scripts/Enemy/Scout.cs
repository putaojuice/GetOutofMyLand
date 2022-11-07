using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : MonoBehaviour
{
    [SerializeField] public Transform goal;

    public int indexAssigned;

    private int towerCount;
    private List<GameObject> towerList = new List<GameObject>();
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        Destroy(gameObject, 3);
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAt(GameObject spawnPoint, int input)
    {
        //Debug.Log("Scout spawned with index " + indexAssigned);
        this.indexAssigned = input;
        Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
        Instantiate(this.transform, spawnPosition, spawnPoint.transform.rotation);
    }


    void OnTriggerEnter(Collider collider)
    {    
        if (collider.gameObject.tag == "TowerRangeIndicator")
        {
            // Debug.Log("Scout " + indexAssigned + " detected a tower");
            GameObject tower = collider.gameObject;
            if (!towerList.Contains(tower))
            {

                MLEnemyManager enemyManager = gameManager.GetComponent<MLEnemyManager>();
                enemyManager.UpdateSpawnPointTowerCount(indexAssigned);
                towerList.Add(tower);
            }
        }
        if (collider.gameObject.tag == "Endpoint")
        {
            Destroy(gameObject);
        }
    }
}
