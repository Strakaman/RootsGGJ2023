using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateMachine : StateMachine
{
    public Collider hitBox;
    public GameObject hitEffect;

    public void AttackPressed(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Attack pressed");
            CurrentState.ProcessInput(callbackContext.action);
            //animator.SetTrigger("Attack" + val);
        }
    }

}
