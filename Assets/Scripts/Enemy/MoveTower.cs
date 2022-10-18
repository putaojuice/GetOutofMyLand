using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTower : MonoBehaviour
{
    [SerializeField] public GameObject goal;
    [SerializeField] public GameObject originalGoal;
    [SerializeField] public Eater eater;

    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

    public void UpdateTarget()
    {
        var towers = GameObject.FindObjectsOfType<Turret>();
        float shortestDistance = Mathf.Infinity;
        Turret nearestTower = null;

        foreach(Turret tower in towers) {
            float distance = Vector3.Distance(transform.position, tower.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTower = tower;
            }
        }

        if(nearestTower == null){
            goal = originalGoal;
        } else {
            goal = nearestTower.gameObject;
        }
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(eater.isAttacking){
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 2.0f){
                eater.StopAttack();
                Destroy(goal);
                anim.SetBool("isAttacking", false);
                
                Debug.Log("not attacking anymoire");
            }
            return;
        }

        if(goal == null){
            UpdateTarget();
        } else {
            float distance = Vector3.Distance(transform.position, goal.transform.position);
            if(distance < 1.0f){
                if(!eater.isCharging)
                {
                    anim.SetBool("isCharging", true);
                    eater.Charge();
                } else {
                    if(eater.chargePoints == eater.maxChargePoints){
                        Debug.Log("Attacking now!");
                        anim.SetBool("isAttacking", true);
                        anim.SetBool("isCharging", false);
                        eater.Attack();
                        eater.StopCharge();
                    }
                }

            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider);
        // Notify the enemy to get damaged and die instantly
        if(collider.gameObject.tag == "Endpoint"){
            Enemy enemy = gameObject.GetComponent<Enemy>();
            enemy.GetDamaged(9999999999);
        }

        if(collider.CompareTag("Fire")){
            Debug.Log("hitting fire tower");
            anim.SetBool("isAttacking", true);
        } else {
            Debug.Log("not hitting fire tower");
            anim.SetBool("isAttacking", false);
        }

        // Destroy(gameObject);
    }
}