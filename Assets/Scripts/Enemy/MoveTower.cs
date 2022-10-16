using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTower : MonoBehaviour
{
    [SerializeField] public Transform goal;
    [SerializeField] public Transform originalGoal;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

    }

    // Update is called once per frame
    void Update()
    {
        // var enemies = GameObject.FindObjectsOfType<Enemy>();
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
            goal = nearestTower.transform;
        }
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

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
