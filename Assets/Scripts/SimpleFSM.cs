using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM
{
    [SerializeField] private StateID currentState;

    // Movement variables
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    // How "close" to the player is considered as chasing range
    [SerializeField] private float chaseDistance = 5.0f;
    // How "close" from the target waypoint until we choose another waypoint
    [SerializeField] private float waypointDistance = 1.0f;
    // The array of transform points in the scene that the agent will move
    // towards to while patrolling
    [SerializeField] private Transform[] waypoints;
    // Reference to the player tank
    [SerializeField] private Transform player;

    private Transform currentTarget;
    private float distanceToPlayer;

    // Implement the abstract functions
    protected override void Initialize(){
        currentState = StateID.Patrol;
        SetTargetWaypoint();
    }

    protected override void FSMUpdate(){

         //Check distance to player
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch(currentState){
            case StateID.Patrol:
                PatrolBehaviour();
                break;
             case StateID.Chase:
                ChaseBehaviour();
                break;
             case StateID.Attack:
                AttackBehaviour();
                break;
        }
    }

    private void SetCurrentTarget(Transform target){
        currentTarget = target;
    }

    private void SetTargetWaypoint(){
        //Randomize a value from the array
        int randomIndex = Random.Range(0, waypoints.Length);
        //Make sure that the new target is not the same as the previous waypoint
        while(waypoints[randomIndex] == currentTarget){
            //Keep randomizing until currentTarget is new
            randomIndex = Random.Range(0, waypoints.Length);
        }
        SetCurrentTarget(waypoints[randomIndex]);
    }

    private void MoveToTarget(){
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

    private void PatrolBehaviour(){
        // Check distance to waypoint
        float distanceToCurrentTarget = Vector3.Distance(transform.position, currentTarget.position);
        if(distanceToCurrentTarget > waypointDistance){
            MoveToTarget();
        }
        else{
            SetTargetWaypoint();
        }

        //Check distance to player
        if(distanceToPlayer <= chaseDistance){
            SetCurrentTarget(player);
            //Switch to chase state
            currentState = StateID.Chase;
        }
    }

    private void ChaseBehaviour(){
        MoveToTarget();
        if(distanceToPlayer > chaseDistance){
            SetTargetWaypoint();
            currentState = StateID.Patrol;
        }
    }

    private void AttackBehaviour(){

    }
}
