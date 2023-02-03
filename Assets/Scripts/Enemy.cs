using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for all the enemies in the game, Superclasses are intended to mostly only change how the enemy paths its run away function
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject playerReference;
    [SerializeField] public float personalSpaceRange;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Vector3 runAwayPoint;
    protected bool runAwayTriggered = false;

    [SerializeField] public int maxHealth;
    public int currentHealth;
    [SerializeField] protected float baseMaxSpeed;

    //only for testing a destination
    [SerializeField] public Transform target;

    public int CurrentHealth { get => currentHealth; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InitializeHealthAndMaxSpeed();
    }

    void Update()
    {
        Debug.Log(transform.position);
        //if not running and player is within personalSpaceRange, trigger RunAway
        if (!runAwayTriggered && DistanceBetween(playerReference.transform.position, transform.position) <= personalSpaceRange)
        {
            TriggerRunAway(playerReference.transform.position);
        }

        //if at (or close to) runAwayPoint, set the running trigger to false
        if (DistanceBetween(transform.position, runAwayPoint) <= 0.1)
        {
            animator.SetBool("isRunning", false);
            runAwayTriggered = false;
        }
    }

    /// <summary>
    /// Triggers the enemy to run away from the player using the NavMesh system, will be different for each enemy type
    /// </summary>
    protected virtual void TriggerRunAway()
    {
        //base RunAway function, runs towards the testing target
        runAwayTriggered = true;
        runAwayPoint = target.position;
        agent.destination = runAwayPoint;
    }

    /// <summary>
    /// Triggers the enemy to run away from the player in the opposite direction using the NavMesh system
    /// </summary>
    /// <param name="playerPosition">Current position of player at time of trigger</param>
    protected virtual void TriggerRunAway(Vector3 playerPosition)
    {
        runAwayTriggered = true;
        animator.SetBool("isRunning", true);
        Vector3 directionFromPlayer = (playerPosition - transform.position).normalized;
        Debug.Log(directionFromPlayer);
        Vector3 destination = new Vector3(directionFromPlayer.x * 15, directionFromPlayer.y, directionFromPlayer.z * 15);
        runAwayPoint = transform.position - destination;
        agent.destination = runAwayPoint;
    }

    protected float DistanceBetween(Vector3 pointA, Vector3 pointB)
    {
        float totSqr = 0;
        totSqr += Mathf.Pow(pointB.x - pointA.x, 2);
        totSqr += Mathf.Pow(pointB.y - pointA.y, 2);
        totSqr += Mathf.Pow(pointB.z - pointA.z, 2);
        return Mathf.Sqrt(totSqr);
    }

    //Makes sure the starting health and speed are properly set
    protected void InitializeHealthAndMaxSpeed()
    {
        currentHealth = maxHealth;
        agent.speed = baseMaxSpeed;
    }

    /// <summary>
    /// Reduces currentHealth and then reduces runAwaySpeed based on ratio of remaining health
    /// </summary>
    /// <param name="damage">Amount of damage dealt by attack</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //trigger death stuff
        }
        else
        {
            agent.speed = baseMaxSpeed * (currentHealth / maxHealth);
        }
    }
}
