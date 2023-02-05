using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitProcessor : MonoBehaviour
{
    CombatStateMachine csm;
    [SerializeField] string whoDoIHate;
    private void Awake()
    {
        csm = GetComponentInParent<CombatStateMachine>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if player hits enemy
        if (other.tag == "Enemy" && other.tag.Equals(whoDoIHate))
        {
            csm.CheckedHitTarget(other.GetComponent<Enemy>());
        }
        //if enemy hits player
        else if (other.tag == "Player" && other.tag.Equals(whoDoIHate))
        {
            other.GetComponent<PlayerController>().TakeHit(1);
        }
    }

}
