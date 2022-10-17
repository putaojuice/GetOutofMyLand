using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterTower : Turret
{

    [SerializeField] public StatusData rainStatusLevel1;
    [SerializeField] public StatusData rainStatusLevel2;
    [SerializeField] public StatusData rainStatusLevel3;
    [SerializeField] LineRenderer line;
    [SerializeField] public float ActualTowerRange;

    // Start is called before the first frame update
    void Start()
    {
        range = ActualTowerRange;
        firingRate = 0.2f;
        InvokeRepeating("UpdateTarget", 0f, 2.0f);
        towerLevel = 3;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
    }

    public override void UpdateTarget()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyStatus>();

        foreach(EnemyStatus enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < range){
                switch (towerLevel){
                    case 1:
                        enemy.ApplyStatus(rainStatusLevel1);
                        break;
                    case 2:
                        enemy.ApplyStatus(rainStatusLevel2);
                        break;
                    default:
                        enemy.ApplyStatus(rainStatusLevel3);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

}
