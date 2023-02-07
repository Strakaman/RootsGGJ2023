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
        AudioManager.instance.PlayVoiceLine("DaikonSpawn");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isDead || playerCharacter.isDead) { return; }

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

/*        Vector3 mySpeed = myRigidBody.velocity;
        //mySpeed.y = 0;
        float myV = GameManager.instance.Pythag(mySpeed);
        animator.SetFloat("Speed", myV);*/
    }

    protected void GoAfterPlayer()
    {
        if (playerCharacter != null)
        {
            Debug.Log("I'm going after the player");
            goAfterPlayerTriggered = true;
            agent.destination = playerCharacter.transform.position;
            animator.SetBool("Running", true);
        }
    }

    protected override void ReduceMaxSpeed()
    {

    }

    protected void AttackPlayer()
    {
        Debug.Log("I'm attacking the player");
        AudioManager.instance.PlayVoiceLine("DaikonAttack");
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
                if (!agent.hasPath || agent.velocity.sqrMagnitude < Mathf.Epsilon)
                {
                    Debug.Log("I've reached the player");
                    playerReached = true;
                    animator.SetBool("Running", false);
                }
                else
                {
                    Debug.Log("I'm falling down into my shadow");
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

    void FootR()
    {

    }
}
