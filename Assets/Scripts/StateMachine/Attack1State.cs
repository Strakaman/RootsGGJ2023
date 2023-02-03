using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : MeleeBaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new Attack2State());
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackIndex = 1;
        duration = 0.5f;
        animator.SetTrigger("Attack" + attackIndex);
        Debug.Log("Entering Player Attack " + attackIndex);
    }
}
