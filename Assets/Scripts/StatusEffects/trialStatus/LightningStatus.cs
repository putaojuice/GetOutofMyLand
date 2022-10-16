using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStatus : Status
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 5.0f;
        statusDuration = 5.0f;
        StatusType = "LightningEffect";
        towerLevel = 1.0f;
        
    }

    void UpdateTowerLevel(){
        towerLevel ++;
    }

    // Update is called once per frame
    void Update()
    {
        damage = 5.0f * towerLevel;
        statusDuration = towerLevel == 1.0f ? 5.0f : towerLevel == 2.0f ? 7.0f : 10.0f;
    }
}
