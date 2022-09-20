using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : Enemy
{

    [SerializeField]private float originalSpeed;
    [SerializeField]private float chargeSpeed = 3000f;
    [SerializeField]private float skillDuration = 1f;
    [SerializeField]private bool skillToggled = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 50f;
        hp = maxHp;
        skillCoolDown = 5f;
        defence = 3f;

        // Finding necesary objects
        WaveSpawning = GameObject.Find("GameMaster").GetComponent<WaveSpawning>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalSpeed = agent.speed;

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

        // Skill check
        if(!skillToggled){
            if(skillCoolDown <= 0) {
                UseSkill();
                skillCoolDown = 6f;
            }
        } else {
            if(skillDuration > 0){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 1f;
            }
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
    }

    // Charge Skill will be called
    public override void UseSkill() {

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = chargeSpeed;
        skillToggled = true;

    }

    // To undo the skills activated
    public void UndoSkill() {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = originalSpeed;
        skillToggled = false;
    }

}
