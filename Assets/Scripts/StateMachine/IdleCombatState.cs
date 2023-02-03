using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleCombatState : State
{
    public override void ProcessInput(InputAction action)
    {
        base.ProcessInput(action);
        if (action.name.Equals("Attack"))
        {
            stateMachine.SetNextState(new Attack1State());
        }
    }
}
