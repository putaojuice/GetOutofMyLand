using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnAgent : MonoBehaviour
{
    List<GameObject> spawnPointList;
    private GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(List<GameObject> list)
    {
        this.spawnPointList = list;
        GameObject randomGrid = spawnPointList[Random.Range(0, spawnPointList.Count)];
        GridFloor gridFloor = randomGrid.GetComponent<GridFloor>();
        Vector3 spawnPosition = new Vector3(gridFloor.transform.position.x, gridFloor.transform.position.y + 0.6f, gridFloor.transform.position.z);

        MoveDestination endPoint = gameObject.GetComponent<MoveDestination>();
        Vector3 spawnDirection = endPoint.transform.position - spawnPoint.transform.position;
        Vector3 upVector = new Vector3(0f, spawnPoint.transform.rotation.y, 0f);
        Quaternion finalRotation = Quaternion.LookRotation(spawnDirection, upVector);

        Instantiate(this.transform, spawnPosition, finalRotation);
    }

    public void SpawnAt(GameObject spawnPoint)
    {
        Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
        Instantiate(this.transform, spawnPosition, spawnPoint.transform.rotation);
    }
}
