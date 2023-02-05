using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject veggieGoalObjRef;
    public Transform veggieGoalsParent;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateGoalUI(RecipeGoal[] recipeGoals)
    {
        VeggieGoalsUI[] veggieGoalsUIs = GetComponentsInChildren<VeggieGoalsUI>();
        for (int i = 0; i < recipeGoals.Length; i++)
        {
            int j = i;
            if (!recipeGoals[i].isCompleted)
            {
                if (j < veggieGoalsUIs.Length)
                {
                    //if Veggie UI superobject already exists
                    //update veggie icon
                    veggieGoalsUIs[j].veggieTitle.text = recipeGoals[i].enemyType.ToString();
                    veggieGoalsUIs[j].goalNumber.text = (recipeGoals[i].veggieGoal - recipeGoals[i].veggieCount).ToString();
                }
                else if (recipeGoals.Length > veggieGoalsUIs.Length)
                {
                    //need to make new Veggie UI superobject
                    VeggieGoalsUI vGUI = Instantiate(veggieGoalObjRef, veggieGoalsParent).GetComponent<VeggieGoalsUI>();
                    //update veggie icon
                    vGUI.veggieTitle.text = recipeGoals[i].enemyType.ToString();
                    vGUI.goalNumber.text = (recipeGoals[i].veggieGoal - recipeGoals[i].veggieCount).ToString();
                }
            }
            else if (recipeGoals[i].isCompleted)
            {
                if (j < veggieGoalsUIs.Length)
                {
                    veggieGoalsUIs[j].gameObject.SetActive(false);
                }
            }
        }
    }
}
