using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeBaseState : State
{
    public float duration;
    protected Animator animator;
    protected bool shouldCombo;
    protected int attackIndex;
    public List<Enemy> enemiesHit;

    public MeleeBaseState()
    {
        enemiesHit = new List<Enemy>(); 
    }

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
        //Debug.Log("Entering Player Attack " + attackIndex);
    }

    public override void ProcessInput(InputAction action)
    {
        base.ProcessInput(action);
        if ((action.name.Equals("Attack") && animator.GetFloat("ComboWindow.Open") > 0))
        {
            shouldCombo = true;
        }
    }

    public virtual void ProcessHitEnemy(Enemy enemyThatWasHit)
    {
        //Debug.Log("Checking if hit enemy is already hit with this attack " + attackIndex + " Enemy: " + enemyThatWasHit.transform.name);
        if (!enemiesHit.Contains(enemyThatWasHit))
        {
            enemiesHit.Add(enemyThatWasHit);
            enemyThatWasHit.TakeHit(1);
        }
    }
}
