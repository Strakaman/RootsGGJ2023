using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Transform playerSpawn;
    public Transform[] daikonSpawns;
    public Transform[] vegetableSpawns;

    public GameObject carrotEnemy;
    public GameObject daikonEnemy;
    public GameObject garlicEnemy;
    public GameObject onionEnemy;

    public GameObject activeDaikon = null;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(PlayerController pC)
    {
        //flashing active state here keeps the player from clipping through the stage (occasional bug)
        pC.gameObject.SetActive(false);
        pC.gameObject.transform.position = playerSpawn.position;
        pC.gameObject.SetActive(true);
    }

    public void SpawnVegetables(RecipeGoal[] recipeGoals)
    {
        List<Transform> veggieSpawns = new List<Transform>(vegetableSpawns);
        foreach (RecipeGoal recipeGoal in recipeGoals)
        {
            for (int i = 0; i < recipeGoal.veggieGoal; i++)
            {
                int index = Random.Range(0, veggieSpawns.Count);
                if (recipeGoal.enemyType == EnemyType.Carrot)
                {
                    Instantiate(carrotEnemy, veggieSpawns[index].position, Quaternion.identity);
                }
                else if(recipeGoal.enemyType == EnemyType.Garlic)
                {
                    Instantiate(garlicEnemy, veggieSpawns[index].position, Quaternion.identity);
                }
                else if (recipeGoal.enemyType == EnemyType.Onion)
                {
                    Instantiate(onionEnemy, veggieSpawns[index].position, Quaternion.identity);
                }
                veggieSpawns.RemoveAt(index);
            }
        }
    }

    public void SpawnDaikon()
    {
        DestroyDaikon();

        Vector3 playerPos = GameManager.instance.playerReference.transform.position;
        Dictionary<float, Transform> distancesToSpawnPoints = new Dictionary<float, Transform>();
        float biggestDistance = Mathf.NegativeInfinity;
        foreach (Transform spawn in daikonSpawns)
        {
            float distance = GameManager.instance.DistanceBetween(playerPos, spawn.position);
            distancesToSpawnPoints.Add(distance, spawn);

            if (distance > biggestDistance)
            {
                biggestDistance = distance;
            }
        }

        activeDaikon = Instantiate(daikonEnemy, distancesToSpawnPoints[biggestDistance].position, Quaternion.identity);
    }

    public void DestroyDaikon()
    {
        if (activeDaikon != null)
        {
            Destroy(activeDaikon.gameObject);
        }
    }
}
