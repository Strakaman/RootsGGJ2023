using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGoal
{
    public EnemyType enemyType;
    public int veggieGoal;
    public int veggieCount;
    public bool isCompleted = false;

    public RecipeGoal(EnemyType eT, int vG)
    {
        enemyType = eT;
        veggieGoal = vG;
        veggieCount = 0;
    }

    public void IncreaseVeggieCount()
    {
        veggieCount += 1;
        CheckCompleted();
    }

    public void CheckCompleted()
    {
        if (veggieCount >= veggieGoal)
        {
            isCompleted = true;
            Debug.Log($"The {enemyType} goal is now complete.");
        }
    }
}
