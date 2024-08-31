using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    // key: TransitionId, value: StateId
    // This dictionary will contain the conditions and what state it will transition to
    // Chast state: transitionRule -> LostPlayer, switch to Patrol
    // Chast state: transitionRule -> ReachPlayer, switch to Attack
    protected Dictionary<TransitionID, StateID> transitionMap = new();
   
    protected StateID stateId;
    public StateID StateId => stateId;
    // above is a shortcut for this:
    /*
    public StateID StateId{
        get {return stateId;}
    }*/

    public void AddTransition(TransitionID transition, StateID state){
        // Check if the arguments are valid
        if(transition == TransitionID.None || state == StateID.None){
            Debug.LogWarning($"Cannot transition to None");
            return;
        }
        // Prevent double entries to the dictionary
        if(transitionMap.ContainsKey(transition)){
            Debug.LogWarning($"Cannot add {transition} because it already exists");
            return;
        }
        transitionMap.Add(transition, state);
    }

    public void RemoveTransition(TransitionID transition){
        // Check if the arguments are valid
        if(transition == TransitionID.None){
            return;
        }
        if(transitionMap.ContainsKey(transition)){
            transitionMap.Remove(transition);
        }
    }

    // This method returns the next state the FSM should be if it receives
    // the transition
    public StateID GetOutputState(TransitionID transition){
        // Check if the arguments are valid
        if(transition == TransitionID.None){
            Debug.LogWarning($"Cannot transition to None");
            return StateID.None;
        }
        if(transitionMap.ContainsKey(transition)){
            return transitionMap[transition];
        }
        return StateID.None;
    }

    // Control any kind of behavior in this state
    public abstract void RunState(Transform player, Transform agent);
    public abstract void CheckTransitionRules(Transform player, Transform agent);
}
