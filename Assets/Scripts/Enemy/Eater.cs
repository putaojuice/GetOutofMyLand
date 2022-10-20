using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : Enemy
{
    [SerializeField] public float maxChargePoints = 5.0f;
    [SerializeField] public float chargePoints; // number of charges before attacking
    [SerializeField] public bool isCharging;
    [SerializeField] public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 60.0f * (GameManager.instance.waveIndex + 1);
        // maxHp = 1000f;
        hp = maxHp;
        defence = 0f;

        maxChargePoints = 3.0f;
        chargePoints = 0.0f;
        skillCoolDown = 2.0f;
        isCharging = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(skillCoolDown <= 0f && isCharging) {
            UseSkill(); // charge
            skillCoolDown = 5f;
        }

        if(isCharging){
            skillCoolDown -= Time.deltaTime;
        }

        if(hp <= 0.0f){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }


    }

    public virtual void UseSkill()
    {
        chargePoints += 1.0f;
    }

    public void Charge()
    {
        isCharging = true;
    }

    public void StopCharge()
    {
        isCharging = false;
        isCharging = false;
        chargePoints = 0.0f;
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    public void Attack()
    {
        isAttacking = true;
    }

}