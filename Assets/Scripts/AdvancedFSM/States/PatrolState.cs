using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    private Transform[] waypoints;
    private Transform currentTarget;
    private EnemyTankController controller;

    // Since there is no MonoBehaviour, we initialize through the constructor
    // Always define stateId in the constructor
    public PatrolState(Transform[] waypoints){
        this.waypoints = waypoints;
        SetTargetWaypoint();
        stateId = StateID.Patrol;
    }

    public override void RunState(Transform player, Transform agent){
         agent.GetComponent<EnemyTankController>().ResumeMovement();
         agent.GetComponent<EnemyTankController>().RotateTurret(player);
         controller = agent.GetComponent<EnemyTankController>();
        // Check distance to waypoint
        if(controller == null){
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }
        float distanceToCurrentTarget = Vector3.Distance(agent.position, currentTarget.position);
        if(distanceToCurrentTarget > controller.WaypointDistance){
            controller.MoveToTarget(currentTarget);
        }
        else{
            SetTargetWaypoint();
        }
    }

    public override void CheckTransitionRules(Transform player, Transform agent){
        if(controller == null){
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }

        if(Vector3.Distance(agent.position, player.position) <= controller.ChaseDistance){
            // Call the transition
            controller.PerformTransition(TransitionID.SawPlayer);
        }
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

    private void SetCurrentTarget(Transform target){
        currentTarget = target;
    }
}
