using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class MLMoveDestination : Agent
{
    [SerializeField] public Transform goal;
    private Animator animator;

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin IS CALLED IN MLMOVEDESTINATION");
        // set number of discrete decisions
      

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("CollectObservations IS CALLED IN MLMOVEDESTINATION");
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localPosition);

    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log("OnActionReceived IS CALLED IN MLMOVEDESTINATION");
        float forwardMovement = actions.ContinuousActions[0];
        float horizontalMovement = 0;
        if (actions.ContinuousActions[1] == 1f)
        {
            horizontalMovement = -1f;
        }
        else {
            horizontalMovement = 1f;
        }

        // Set animator parameters
        animator.SetFloat("Vertical", forwardMovement);
        animator.SetFloat("Horizontal", horizontalMovement);

        // Tiny negative reward every step
        AddReward(-0.1f);
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristic IS CALLED IN MLMOVEDESTINATION");
        animator.SetBool("Walk Forward", true);
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
    }

    void OnTriggerEnter(Collider collider)
    {

        // Notify the enemy to get damaged and die instantly
        if(collider.gameObject.tag == "Endpoint"){
            SetReward(+1f);
            EndEpisode();
        }
  
        // Destroy(gameObject);
    }

}
