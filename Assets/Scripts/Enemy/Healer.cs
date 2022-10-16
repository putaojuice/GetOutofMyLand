using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Healer : Enemy
{

    [Header("Healer Data")]
    [SerializeField] public float range = 7.5f;
    [SerializeField] public int numSegments = 128;
    [SerializeField] LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {

        maxHp = 500f * (GameManager.instance.waveIndex + 1) * 0.5f;
        hp = maxHp;
        skillCoolDown = 1.5f;
        defence = 1f;

        // Finding necesary objects
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalSpeed = agent.speed;

    }

    // Update is called once per frame
    void Update()
    {

        if(_statusData != null){
            UpdateStatusEffects();
        }

        // DoRenderer(numSegments, range);

        if(hp <= 0.0f){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        if(skillCoolDown <= 0f ){
            UseSkill();
            skillCoolDown = 6f;
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
        statusCoolDown -= Time.deltaTime;
    }

    // public void DoRenderer (int steps, float radius) {

    //     line.positionCount = steps;
    //     for(int currStep = 0; currStep < steps; currStep ++){
    //         float circumferenceProgress = (float)currStep/steps;
    //         float currRadian = circumferenceProgress * 2 * Mathf.PI;
    //         float xScaled = Mathf.Cos(currRadian);
    //         float yScaled = Mathf.Sin(currRadian);

    //         float x = xScaled * radius;
    //         float y = yScaled * radius;

    //         Vector3 currPosition = transform.position + new Vector3(x,0,y);
    //         line.SetPosition(currStep, currPosition);

    //     }
    // }

    public override void UseSkill() {
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(var enemy in enemies) {
            if((transform.position - enemy.transform.position).magnitude < range){
                enemy.GetHealed(2);
            }
        }
        gameObject.transform.Find("HealingLight").gameObject.SetActive(true);
        gameObject.transform.Find("HealingLight").GetComponent<ParticleSystem>().Play();
    }
}
