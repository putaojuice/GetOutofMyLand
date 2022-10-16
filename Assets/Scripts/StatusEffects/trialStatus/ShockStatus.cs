using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStatus : Status
{

    // Start is called before the first frame update
    void Start()
    {
        StatusType = "ShockEffect";
        towerLevel = 1.0f;
        slowDebuff = 1.0f;
        statusDuration = 0.0f;
    }

    public void setSlowDebuff(float value){
        slowDebuff = 1.0f/value;
    }

    public void setSlowDuration(float duration){
        statusDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
