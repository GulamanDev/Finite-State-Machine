using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    private EnemyTankController controller;
    private float fireRate = 1.0f; // Time in seconds between shots
    private float nextFireTime = 0.0f;

    public AttackState()
    {
        stateId = StateID.Attack;
    }

    public override void RunState(Transform player, Transform agent)
    {
        // Stop the tank
        agent.GetComponent<EnemyTankController>().StopMovement();

        // Rotate turret towards the player
        agent.GetComponent<EnemyTankController>().RotateTurret(player);

        // Check if it's time to fire
        if (Time.time > nextFireTime)
        {
            agent.GetComponent<EnemyTankController>().FireAtPlayer();
            nextFireTime = Time.time + fireRate;
        }

        // Ensure the agent has the EnemyTankController component
        controller = agent.GetComponent<EnemyTankController>();
        if (controller == null)
        {
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }
    }

    public override void CheckTransitionRules(Transform player, Transform agent){
        if(controller == null){
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }

        if(Vector3.Distance(agent.position, player.position) >= controller.ChaseDistance){
            controller.PerformTransition(TransitionID.LostPlayer);
        }
        if(Vector3.Distance(agent.position, player.position) <= controller.ChaseDistance){
            controller.PerformTransition(TransitionID.SawPlayer);
        }
    }
}