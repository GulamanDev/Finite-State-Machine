using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private EnemyTankController controller;
    private Transform currentTarget;

    // Always define stateId in the constructor
    public ChaseState(){
        stateId = StateID.Chase;
    }

    public override void RunState(Transform player, Transform agent){
        controller = agent.GetComponent<EnemyTankController>();
        if(controller == null){
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }
        currentTarget = player;
        controller.MoveToTarget(player);
    }

    public override void CheckTransitionRules(Transform player, Transform agent){
        if(controller == null){
            Debug.LogError("Make sure agent has EnemyTankController");
            return;
        }

        if(Vector3.Distance(agent.position, player.position) < controller.AttackDistance){
            controller.PerformTransition(TransitionID.ReachPlayer);
        }
        if(Vector3.Distance(agent.position, player.position) >= controller.ChaseDistance){
            controller.PerformTransition(TransitionID.LostPlayer);
        }
    }
}
