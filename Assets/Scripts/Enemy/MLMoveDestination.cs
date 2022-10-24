using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public class MLMoveDestination : Agent
{
    [SerializeField] public Transform goal;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public bool maskActions = true;

    const int k_NoAction = 0;  // do nothing!
    const int k_Up = 1;
    const int k_Down = 2;
    const int k_Left = 3;
    const int k_Right = 4;

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        getLeftGridPosition();

    }

    // Start is called before the first frame update
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 getCurrentRoundedPosition() {
        int currentX = (int)Mathf.Round(this.transform.position.x);
        int currentZ = (int)Mathf.Round(this.transform.position.z);
        float currentY = this.transform.position.y;
        return new Vector3(currentX, currentY, currentZ);
    }

    // Get Vector3 of left grid
    public Vector3 getLeftGridPosition() {
        Vector3 currentPosition = getCurrentRoundedPosition();
        return currentPosition - new Vector3(1,0,0);
    }

    // Get Vector3 of right grid
    public Vector3 getRightGridPosition()
    {
        Vector3 currentPosition = getCurrentRoundedPosition();
        return currentPosition + new Vector3(1, 0, 0);
    }

    // Get Vector3 of Front grid
    public Vector3 getFrontGridPosition()
    {
        Vector3 currentPosition = getCurrentRoundedPosition();
        return currentPosition + new Vector3(0, 0, 1);
    }

    // Get Vector3 of Hind grid
    public Vector3 getHindGridPosition()
    {
        Vector3 currentPosition = getCurrentRoundedPosition();
        return currentPosition - new Vector3(0, 0, 1);
    }

    // Calculate path based on navmesh
    bool CalculateNewPath(Vector3 v3Input) {
        NavMeshPath path = new NavMeshPath();
        this.navMeshAgent.CalculatePath(v3Input, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else {
            return true;
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        // Mask the necessary actions if selected by the user.
        if (maskActions)
        {
            // Prevents the agent from picking an action that would make it collide with a wall

            if (!CalculateNewPath(getLeftGridPosition()))
            {
                actionMask.SetActionEnabled(0, k_Left, false);
            }

            if (!CalculateNewPath(getRightGridPosition()))
            {
                actionMask.SetActionEnabled(0, k_Right, false);
            }

            if (!CalculateNewPath(getHindGridPosition()))
            {
                actionMask.SetActionEnabled(0, k_Down, false);
            }

            if (!CalculateNewPath(getFrontGridPosition()))
            {
                actionMask.SetActionEnabled(0, k_Up, false);
            }
        }
    }

    // to be implemented by the developer
    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        AddReward(-0.01f);
        var action = actionBuffers.DiscreteActions[0];

        var targetPos = transform.position;
        switch (action)
        {
            case k_NoAction:
                // do nothing
                break;
            case k_Right:
                targetPos = transform.position + new Vector3(1f, 0, 0f);
                break;
            case k_Left:
                targetPos = transform.position + new Vector3(-1f, 0, 0f);
                break;
            case k_Up:
                targetPos = transform.position + new Vector3(0f, 0, 1f);
                break;
            case k_Down:
                targetPos = transform.position + new Vector3(0f, 0, -1f);
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
    }

    Vector3 getRandomPosition() {
        return new Vector3(Random.Range(-3f, +3f), this.transform.position.y, Random.Range(-3f, +3f));
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin IS CALLED IN MLMOVEDESTINATION");
        transform.localPosition = getRandomPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("CollectObservations IS CALLED IN MLMOVEDESTINATION");
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.localPosition);

    }




    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristic IS CALLED IN MLMOVEDESTINATION");
        animator.SetBool("Walk Forward", true);
        navMeshAgent.destination = goal.position;
    }

    void OnTriggerEnter(Collider collider)
    {

        // Notify the enemy to get damaged and die instantly
        if(collider.gameObject.tag == "Endpoint"){
            Debug.Log("REACHED END GOAL HEHEXD");
            SetReward(+1f);
            EndEpisode();
        }
  
        // Destroy(gameObject);
    }

}
