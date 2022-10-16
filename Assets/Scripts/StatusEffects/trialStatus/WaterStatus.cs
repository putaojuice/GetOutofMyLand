using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStatus : Status
{
    // Start is called before the first frame update
    void Start()
    {
        DOTPoints = 1.0f;
        statusDuration = 5.0f;
        DOTInterval = 1.0f;
        StatusType = "WaterEffect";
        towerLevel = 1.0f;
        
    }

    void UpdateTowerLevel(){
        towerLevel ++;
    }

    // Update is called once per frame
    void Update()
    {
        DOTPoints = 1.0f * towerLevel;
        statusDuration = towerLevel == 1.0f ? 5.0f : towerLevel == 2.0f ? 7.0f : 10.0f;
    }
}
