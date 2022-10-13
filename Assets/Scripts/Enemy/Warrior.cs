using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : Enemy
{

    [Header("Warrior Data")]
    [SerializeField]private float chargeSpeed = 3.0f;
    [SerializeField]private float skillDuration = 0.5f;
    [SerializeField]private bool skillToggled = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 300f * (GameManager.instance.waveIndex + 1) * 0.5f;
        hp = maxHp;
        skillCoolDown = 1f;
        defence = 1f;

        // Finding necesary objects
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalSpeed = agent.speed;
        agent.speed = 0.0f;

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
            if(skillCoolDown <= 0f) {
                UseSkill();
                skillCoolDown = 2f;
            }
        } else {
            if(skillDuration > 0f){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 1f;
            }
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
        statusCoolDown -= Time.deltaTime;
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
