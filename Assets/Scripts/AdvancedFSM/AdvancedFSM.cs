using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSM : FSM
{
    private FSMState currentState;
    public FSMState CurrentState => currentState;
    private StateID currentStateID;
    protected List<FSMState> fsmStates = new();

    public void AddState(FSMState state){
        // null check
        if(state == null){
            return;
        }
        // Check if the state to be added is the first state
        // This will be the default state
        if(fsmStates.Count == 0){
            // Add to list
            fsmStates.Add(state);
            // Set default
            currentState = state;
            currentStateID = state.StateId;
        }
        // Prevent adding the same element to the list
        if(fsmStates.Contains(state)){
            return;
        }
        // Default behavior
        fsmStates.Add(state);
    }

    public void RemoveState(FSMState state){
        // null check
        if(state == null){
            return;
        }
        if(fsmStates.Contains(state)){
            fsmStates.Remove(state);
        }
    }

    /// <summary>
    /// This will try to change the state the FSM is based on the transition
    /// </summary>
    /// <param name="transition"></param>
    public void PerformTransition(TransitionID transition){
        // null check
        if(transition == TransitionID.None){
            return;
        }
        //Check if the current state the FSM is has the transition
        StateID id = currentState.GetOutputState(transition);
        // check if the id exists
        if(id == StateID.None){
            Debug.LogError($"Current state does not support {transition}");
            return;
        }
        // set the enum of the current state
        currentStateID = id;
        // Look for the id in our list
        foreach(FSMState state in fsmStates)
        {
            if(state.StateId == currentStateID)
            {
                // Change the currentState
                currentState = state;
                break;
            }
        }
    }

    protected override void Initialize(){
       
    }

    protected override void FSMUpdate(){

    }
}
