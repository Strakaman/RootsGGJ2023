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
    public HorizontalLayoutGroup veggieGoalsHoriLayout;

    public Sprite carrotSprite;
    public Sprite daikonSprite;
    public Sprite garlicSprite;
    public Sprite onionSprite;

    public Dictionary<EnemyType, Sprite> spriteDictionary = new Dictionary<EnemyType, Sprite>();

    private void Awake()
    {
        instance = this;
        BuildImageDictionary();

    }

    private void BuildImageDictionary()
    {
        spriteDictionary.Add(EnemyType.Carrot, carrotSprite);
        spriteDictionary.Add(EnemyType.Daikon, daikonSprite);
        spriteDictionary.Add(EnemyType.Garlic, garlicSprite);
        spriteDictionary.Add(EnemyType.Onion, onionSprite);
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
                    veggieGoalsUIs[j].veggieSprite.sprite = spriteDictionary[recipeGoals[i].enemyType];
                    veggieGoalsUIs[j].goalNumber.text = (recipeGoals[i].veggieGoal - recipeGoals[i].veggieCount).ToString();
                }
                else if (recipeGoals.Length > veggieGoalsUIs.Length)
                {
                    //need to make new Veggie UI superobject
                    VeggieGoalsUI vGUI = Instantiate(veggieGoalObjRef, veggieGoalsParent).GetComponent<VeggieGoalsUI>();
                    vGUI.veggieSprite.sprite = spriteDictionary[recipeGoals[i].enemyType];
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

        veggieGoalsUIs = GetComponentsInChildren<VeggieGoalsUI>();
        if (veggieGoalsUIs.Length >= 4)
        {
            veggieGoalsHoriLayout.spacing = -300;
        }
        else if (veggieGoalsUIs.Length == 3)
        {
            veggieGoalsHoriLayout.spacing = -400;
        }
        else if(veggieGoalsUIs.Length <= 2)
        {
            veggieGoalsHoriLayout.spacing = -500;
        }
    }
}
