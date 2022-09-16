using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Assasin : Enemy
{
    [SerializeField]public Material hiddenMat;
    [SerializeField]private Material originalMat;
    [SerializeField]private float originalSpeed;
    [SerializeField]private float skillDuration = 2f;
    [SerializeField]private bool skillToggled = false;
    [SerializeField]private Renderer rend;
    [SerializeField]private float lerp;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        maxHp = 3f;
        hp = 3f;
        skillCoolDown = 5f;
        defence = 1f;

        // Finding necesary objects
        WaveSpawning = GameObject.Find("GameMaster").GetComponent<WaveSpawning>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalSpeed = agent.speed;
        rend = GetComponent<Renderer>();
        originalMat = rend.material;

    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        // Skill check
        if(skillToggled) {
            if(skillDuration > 0){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 2f;
            }
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
    }

    public override void GetDamaged(float damage) {
        if(skillCoolDown <= 0) {
            UseSkill();
            skillCoolDown = 5f;
        }
        if(damage - defence > 0){
            hp -= damage;
        }
    }

    public override void UseSkill() {

        gameObject.tag = "HiddenEnemy";
        rend.material = hiddenMat;
        skillToggled = true;

    }

    // To undo the skills activated
    public void UndoSkill() {
        
        gameObject.tag = "Enemy";
        rend.material = originalMat;
        skillToggled = false;
    }

}
