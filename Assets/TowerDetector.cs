using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetector : MonoBehaviour
{
    private int towerCount;
    private List<GameObject> towerList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {

        // Notify the enemy to get damaged and die instantly
        if (collider.gameObject.tag == "Tower")
        {
            GameObject tower = collider.gameObject;
            if (!towerList.Contains(tower)) {
                towerCount += 1;
                towerList.Add(tower);
            } 
        }
    }
}
