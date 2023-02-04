using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all the enemies in the game, Superclasses are intended to mostly only change how the enemy paths its run away function
/// </summary>
public class Health : MonoBehaviour
{
    public Material hitMaterial;

    [SerializeField] public int maxHealth;
    public int currentHealth;

    public int MaxHealth { get => maxHealth; }
    public int CurrentHealth { get => currentHealth; }

    public UnityEvent healthLostEvent;
    public UnityEvent deathEvent;

    protected SkinnedMeshRenderer[] mySkinMeshRenderers;

    private void Awake()
    {
        healthLostEvent = new UnityEvent();
        deathEvent = new UnityEvent();
        InitializeHealth();
    }

    protected virtual void Start()
    {
        mySkinMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void AddHealthLostListener(UnityAction methodToTrigger)
    {
        healthLostEvent.AddListener(methodToTrigger);
    }

    public void AddDeathListener(UnityAction methodToTrigger)
    {
        deathEvent.AddListener(methodToTrigger);
    }

    //Makes sure the starting health and speed are properly set
    protected void InitializeHealth()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces currentHealth and then reduces runAwaySpeed based on ratio of remaining health
    /// </summary>
    /// <param name="damage">Amount of damage dealt by attack</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(GotHitAnimation());
        if (currentHealth <= 0)
        {
            //trigger death stuff
            deathEvent.Invoke();
        }
        else
        {
            healthLostEvent.Invoke();
        }
    }

    IEnumerator GotHitAnimation()
    {
        List<Material[]> materialMatrix = new List<Material[]>(); 
        foreach (SkinnedMeshRenderer asmr in mySkinMeshRenderers)
        {
            Material[] cachedSkinMaterials = asmr.materials;
            materialMatrix.Add(cachedSkinMaterials);
            Material[] newArray = new Material[cachedSkinMaterials.Length];
            for (int i = 0; i < asmr.materials.Length; i++)
            {

                newArray[i] = hitMaterial;
            }
            asmr.materials = newArray;
        }
        yield return new WaitForSeconds(.15f);

        for (int i=0; i < mySkinMeshRenderers.Length; i++)
        {
            Material[] reversionArray = materialMatrix[i];
            mySkinMeshRenderers[i].materials = reversionArray;
        }
    }

}
