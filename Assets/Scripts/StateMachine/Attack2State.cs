using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : MeleeBaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();


        if (shouldCombo && animator.GetFloat("AnimationCancel2.Open") > 0)
        {
            //even though the beginning of the animation clip is supposed to set it back to 0, it's instead LERPing back

            stateMachine.SetNextState(new Attack3State());
        }
        else if (fixedtime >= duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackIndex = 2;
        duration = 1.2f;
        animator.SetTrigger("Attack" + attackIndex);
    }
}
