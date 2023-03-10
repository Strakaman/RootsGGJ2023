using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3State : MeleeBaseState
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedtime >= duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackIndex = 3;
        duration = 1.2f;
        animator.SetTrigger("Attack" + attackIndex);
        AudioManager.instance.PlayVoiceLine("Attack" + attackIndex, 2);
        AudioManager.instance.PlaySoundFX("SliceSound" + attackIndex,stateMachine.transform.position);
    }
}
