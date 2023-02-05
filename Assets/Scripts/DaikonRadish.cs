using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaikonRadish : Enemy
{
    [SerializeField]
    PlayerController playerCharacter;


    [SerializeField]
    protected float attackCooldown = 5f;

    protected float timeSinceLastAttack = 0f;

    protected bool goAfterPlayerTriggered = false;

    protected bool playerReached = false;

    protected bool attackingPlayer = false;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        if (playerCharacter == null)
        {
            playerCharacter = GameManager.instance.playerReference.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isDead) { return; }

        timeSinceLastAttack += Time.deltaTime;
        if (attackingPlayer) { return; }
        else if (playerReached)
        {
            if (timeSinceLastAttack > attackCooldown)
            {
                AttackPlayer();
            }
        }
        else if (!goAfterPlayerTriggered)
        {
            GoAfterPlayer();
        }
        else
        {
            CheckIfPlayerReached();
        }
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Speed", agent.speed);
    }

    protected void GoAfterPlayer()
    {
        if (playerCharacter != null)
        {
            Debug.Log("I'm going after the player");
            goAfterPlayerTriggered = true;
            agent.destination = playerCharacter.transform.position;
        }
    }

    protected override void ReduceMaxSpeed()
    {

    }

    protected void AttackPlayer()
    {
        Debug.Log("I'm attacking the player");
        Vector3 turnVector = playerCharacter.transform.position - transform.position;
        turnVector.y = 0;
        turnVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(turnVector);
        transform.rotation = targetRotation;

        timeSinceLastAttack = 0;
        attackingPlayer = true;
        animator.SetTrigger("Attack1");
    }

    protected void CheckIfPlayerReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("I've reached the player");
                    playerReached = true;
                }
            }
        }
    }
    
    //called by animation event
    public void AttackEnded()
    {
        Debug.Log("My attack is done!");
        attackingPlayer = false;
        playerReached = false;
        goAfterPlayerTriggered = false;
    }

    void Hit()
    {

    }

    void FootL()
    {

    }
}
