using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<RecipeGoal> recipeGoals = new List<RecipeGoal>();
    public bool isRecipeComplete = false;
    public GameObject playerReference;

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
        isRecipeComplete = false;
        //hard coded for now for easy testing
        //recipeGoals.Add(new RecipeGoal(EnemyType.Carrot, 1));
        recipeGoals.Add(new RecipeGoal(EnemyType.Carrot, Random.Range(1, 3)));
        recipeGoals.Add(new RecipeGoal(EnemyType.Garlic, Random.Range(1, 3)));
        recipeGoals.Add(new RecipeGoal(EnemyType.Onion, Random.Range(1, 3)));
        SpawnManager.instance.SpawnPlayer(playerReference.GetComponent<PlayerController>());
        SpawnManager.instance.SpawnVegetables(recipeGoals.ToArray());
    }

    public void UpdateRecipeGoal(EnemyType eT)
    {
        bool updatedRecipe = false;
        foreach (RecipeGoal recipeGoal in recipeGoals)
        {
            if (eT == EnemyType.Daikon)
            {
                AudioManager.instance.PlayVoiceLine("DaikonVictory");
                UIManager.instance.EndGameSequence();

            }
            else if (recipeGoal.enemyType == eT)
            {
                recipeGoal.IncreaseVeggieCount();
                updatedRecipe = true;
            }

        }

        UIManager.instance.UpdateGoalUI(recipeGoals.ToArray());

        if (AllGoalsAreCompleted())
        {
            AudioManager.instance.PlayVoiceLine("RecipeComplete");
            isRecipeComplete = true;
            TriggerBossFight();
        }
        else if (updatedRecipe)
        {
            AudioManager.instance.PlayVoiceLine("RecipeUpdate");
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

    public float DistanceBetween(Vector3 pointA, Vector3 pointB)
    {
        float totSqr = 0;
        totSqr += Mathf.Pow(pointB.x - pointA.x, 2);
        totSqr += Mathf.Pow(pointB.y - pointA.y, 2);
        totSqr += Mathf.Pow(pointB.z - pointA.z, 2);
        return Mathf.Sqrt(totSqr);
    }

    public float Pythag(Vector3 pointA)
    {
        float totSqr = 0;
        totSqr += Mathf.Pow(pointA.x, 2);
        totSqr += Mathf.Pow(pointA.y, 2);
        totSqr += Mathf.Pow(pointA.z, 2);
        return Mathf.Sqrt(totSqr);
    }

    private void TriggerBossFight()
    {
        recipeGoals.Add(new RecipeGoal(EnemyType.Daikon, 1));
        UIManager.instance.UpdateGoalUI(recipeGoals.ToArray());
        SpawnManager.instance.SpawnDaikon();
    }

    public void PlayerDeath()
    {
        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        UIManager.instance.PlayerDeathFade();
        yield return new WaitForSeconds(2f);
        SpawnManager.instance.DestroyDaikon();
        yield return new WaitForSeconds(2f);
        playerReference.GetComponent<PlayerController>().ResetPlayer();
        yield return new WaitForSeconds(1f);
        SpawnManager.instance.SpawnDaikon();
        FindObjectOfType<BGMPlayer>().PlayASong();
        UIManager.instance.ResetFadeReload();
    }
}
