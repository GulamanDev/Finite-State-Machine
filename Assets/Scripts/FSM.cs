using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create an abstract class for FSM
public abstract class FSM : MonoBehaviour
{
    // Derived classes should use these functions
    protected abstract void Initialize();
    protected abstract void FSMUpdate();

    private void Start(){
        Initialize();
    }

    private void Update(){
        FSMUpdate();
    }
}
