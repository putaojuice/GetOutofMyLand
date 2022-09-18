using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Healer : Enemy
{

    [SerializeField] public float range = 20f;
    [SerializeField] public int numSegments = 128;
    [SerializeField] LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 3f;
        hp = 3f;
        skillCoolDown = 5f;
        defence = 1f;

        // Finding necesary objects
        WaveSpawning = GameObject.Find("GameMaster").GetComponent<WaveSpawning>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

        DoRenderer(numSegments, range);

        if(hp <= 0){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        if(skillCoolDown <= 0 ){
            UseSkill();
            skillCoolDown = 6f;
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
    }

    public void DoRenderer (int steps, float radius) {

        line.positionCount = steps;
        for(int currStep = 0; currStep < steps; currStep ++){
            float circumferenceProgress = (float)currStep/steps;
            float currRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currRadian);
            float yScaled = Mathf.Sin(currRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currPosition = transform.position + new Vector3(x,0,y);
            line.SetPosition(currStep, currPosition);

        }
    }

    public override void UseSkill() {
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(var enemy in enemies) {
            if((transform.position - enemy.transform.position).magnitude < range){
                enemy.GetHealed(1);
            }
        }
    }
}
