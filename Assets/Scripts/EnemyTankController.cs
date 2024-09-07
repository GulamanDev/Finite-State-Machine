using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : AdvancedFSM
{
    // Movement variables
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    // How "close" to the player is considered as chasing range
    [SerializeField] private float chaseDistance = 5.0f;
    [SerializeField] private float attackDistance = 3.0f;
    // How "close" from the target waypoint until we choose another waypoint
    [SerializeField] private float waypointDistance = 1.0f;
    // The array of transform points in the scene that the agent will move
    // towards to while patrolling
    [SerializeField] private Transform[] waypoints;
    // Reference to the player tank
    [SerializeField] private Transform player;

    public float MoveSpeed => moveSpeed;
    public float RotateSpeed => rotateSpeed;
    public float ChaseDistance => chaseDistance;
    public float AttackDistance => attackDistance;
    public float WaypointDistance => waypointDistance;
    protected override void Initialize()
    {
        fsmStates = new();
        //Construct the FSM
        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(TransitionID.SawPlayer, StateID.Chase);
        ChaseState chase = new ChaseState();
        chase.AddTransition(TransitionID.LostPlayer, StateID.Patrol);
        chase.AddTransition(TransitionID.ReachPlayer, StateID.Attack);

        // First element is the default
        AddState(patrol);
        AddState(chase);
    }

    protected override void FSMUpdate()
    {
       CurrentState.RunState(player, this.transform);
       CurrentState.CheckTransitionRules(player, this.transform);
    }

    public void MoveToTarget(Transform currentTarget){
        // Get the vector pointing towards the direction of the target
        Vector3 targetDirection = currentTarget.position - transform.position;
        // Get the roatation that faces the targetDirection
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // Rotate the tank to face the targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
            Time.deltaTime * rotateSpeed);
        // Tank is already rotate, simply move forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
}
