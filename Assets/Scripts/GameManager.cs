using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<RecipeGoal> recipeGoals = new List<RecipeGoal>();
    public bool isRoundWon = false;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        AudioManager.instance.PlayVoiceLine("MatchStart");
        NewGame();
        UIManager.instance.UpdateGoalUI(recipeGoals.ToArray());
    }

    private void NewGame()
    {
        isRoundWon = false; 
        //hard coded for now for easy testing
        recipeGoals.Add(new RecipeGoal(EnemyType.Carrot, 1));
        recipeGoals.Add(new RecipeGoal(EnemyType.Garlic, 1));
        recipeGoals.Add(new RecipeGoal(EnemyType.Onion, 1));
    }

    public void UpdateRecipeGoal(EnemyType eT)
    {
        foreach (RecipeGoal recipeGoal in recipeGoals)
        {
            if (recipeGoal.enemyType == eT)
            {
                recipeGoal.IncreaseVeggieCount();
            }
        }

        UIManager.instance.UpdateGoalUI(recipeGoals.ToArray());

        if (AllGoalsAreCompleted())
        {
            isRoundWon = true;
            Debug.Log("Round Won");
        }
    } 

    private bool AllGoalsAreCompleted()
    {
        foreach (RecipeGoal recipeGoal in recipeGoals)
        {
            if (recipeGoal.isCompleted == false)
            {
                return false;
            }
        }

        return true;
    }
}
