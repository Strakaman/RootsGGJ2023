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
            if (CurrentState==null)
            {            
                return;
            }
            CurrentState.ProcessInput(callbackContext.action);
        }
    }

    public void CheckedHitTarget(Enemy hitEnemy)
    {
        ((MeleeBaseState)CurrentState).ProcessHitEnemy(hitEnemy);
    }

    public bool IsAttacking()
    {
        if (CurrentState == null)
        {
            return false;
        }
        return !CurrentState.Equals(mainStateType);
    }
}
