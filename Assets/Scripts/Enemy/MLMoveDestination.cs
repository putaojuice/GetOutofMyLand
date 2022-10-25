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
    private Vector3 destination;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public bool maskActions = true;
    bool episodeJustBegan = false;

    const int k_Up = 0;
    const int k_Down = 1;
    const int k_Left = 2;
    const int k_Right = 3;

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Academy.Instance.AutomaticSteppingEnabled = false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.updateRotation = false;
        this.destination = this.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {

        if (episodeJustBegan || reachedDestination()) {
            episodeJustBegan = false;
            navMeshAgent.isStopped = true;
            animator.SetBool("Walk Forward", false);
            Debug.Log("Requesting Decision");
            RequestDecision();
            Academy.Instance.EnvironmentStep();
        } else {
            if (!CalculateNewPath(navMeshAgent.destination)) {
                Debug.Log("trying to move to somewhere unaccessible"); 
                navMeshAgent.isStopped = true;
                this.destination = this.transform.position;
                navMeshAgent.destination = this.destination;
            }
        }
    }

    private void LateUpdate()
    {
        if (navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
        }
    }

    bool reachedDestination() {
        double currentX = (double) System.Math.Round(this.transform.localPosition.x, 1);
        double currentZ = (double) System.Math.Round(this.transform.localPosition.z, 1);
        double destX = (double)System.Math.Round(this.destination.x, 1);
        double destZ = (double)System.Math.Round(this.destination.z, 1);
        if (currentX == destX && currentZ == destZ) {
            return true;
        } else {
            Debug.Log("currentX: " + currentX + " AND destX: " + destX);
            Debug.Log("currentZ: " + currentZ + " AND destZ: " + destZ);
            return false;
        }
    }

    Vector3 getCurrentRoundedPosition() {
        int currentX = (int)Mathf.Round(this.transform.localPosition.x);
        int currentZ = (int)Mathf.Round(this.transform.localPosition.z);
        float currentY = this.transform.localPosition.y;
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
    public Vector3 getTopGridPosition()
    {
        Vector3 currentPosition = getCurrentRoundedPosition();
        return currentPosition + new Vector3(0, 0, 1);
    }

    // Get Vector3 of Hind grid
    public Vector3 getBottomGridPosition()
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

            if (!CalculateNewPath(getBottomGridPosition()))
            {
                actionMask.SetActionEnabled(0, k_Down, false);
            }

            if (!CalculateNewPath(getTopGridPosition()))
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
            case k_Right:
                Debug.Log("chose to go right");
                animator.SetBool("Walk Forward", true);
                this.destination = getRightGridPosition();
                navMeshAgent.destination = this.destination;
                navMeshAgent.isStopped = false;
                break;
            case k_Left:
                Debug.Log("chose to go left");
                animator.SetBool("Walk Forward", true);
                this.destination = getLeftGridPosition();
                navMeshAgent.destination = this.destination;
                navMeshAgent.isStopped = false;
                break;
            case k_Up:
                Debug.Log("chose to go up");
                animator.SetBool("Walk Forward", true);
                this.destination = getTopGridPosition();
                navMeshAgent.destination = this.destination;
                navMeshAgent.isStopped = false;
                break;
            case k_Down:
                Debug.Log("chose to go down");
                animator.SetBool("Walk Forward", true);
                this.destination = getBottomGridPosition();
                navMeshAgent.destination = this.destination;
                navMeshAgent.isStopped = false;
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
    }

    Vector3 getRandomPosition() {
        return new Vector3(Random.Range(-2.0f, +2.0f), this.transform.position.y, Random.Range(-2.0f, 2.03f));
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode Begins");
        transform.localPosition = getRandomPosition();
        this.destination = this.transform.localPosition;
        episodeJustBegan = true;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.localPosition);


    }




    public override void Heuristic(in ActionBuffers actionsOut)
    {
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
    }


}
