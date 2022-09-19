using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tank : Enemy
{

    [SerializeField]private float skillDuration = 2.0f;
    [SerializeField]public bool skillToggled = false;
    [SerializeField]private Renderer rend;
    [SerializeField]public Material aggroMat;
    [SerializeField]private Material originalMat;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 80f;
        hp = maxHp;
        skillCoolDown = 2f;
        defence = 4f;

        // Finding necesary objects
        WaveSpawning = GameObject.Find("GameMaster").GetComponent<WaveSpawning>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = 3.5f;

        rend = GetComponent<Renderer>();
        originalMat = rend.material;

    }

    // Update is called once per frame
    void Update()
    {
        if(_statusData != null){
            UpdateStatusEffects();
        }

        if(hp <= 0.0f || float.IsNaN(hp)){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        // Skill check
        if(!skillToggled){
            if(skillCoolDown <= 0) {
                UseSkill();
                skillCoolDown = 4.0f;
            }
        } else {
            if(skillDuration > 0){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 2.0f;
            }
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
    }

    // Charge Skill will be called
    public override void UseSkill() {
        rend.material = aggroMat;
        skillToggled = true;

    }

    // To undo the skills activated
    public void UndoSkill() {
        rend.material = originalMat;
        skillToggled = false;
    }

}
