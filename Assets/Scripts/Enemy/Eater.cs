using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 300f * (GameManager.instance.waveIndex + 1) * 0.5f;
        hp = maxHp;
        defence = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_statusData != null){
            UpdateStatusEffects();
        }

        if(hp <= 0.0f){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        statusCoolDown -= Time.deltaTime;
    }

    public override void GetDamaged(float damage) {
        if(skillCoolDown <= 0f) {
            UseSkill();
            skillCoolDown = 5f;
        }
        if(damage - defence > 0f){
            hp -= damage;
        }
    }

    public override void GetStatusDamaged(float damage)
    {
            hp -= damage;

    }
}
