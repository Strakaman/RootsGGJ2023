using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitProcessor : MonoBehaviour
{
    CombatStateMachine csm;
    private void Awake()
    {
        csm = GetComponentInParent<CombatStateMachine>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            csm.CheckedHitTarget(other.GetComponent<Enemy>());
        }
    }
}
