using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VentureVeilStructures;

public class QuestSign : MonoBehaviour
{
    public TextMeshPro description;
    public SpriteRenderer difficulty;
    public Sprite easy, medium, hard;

    private Quest quest;
    private GetInstance getInstance;
    private QuestAPI questAPI;


    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        questAPI = getInstance.QuestAPI;
        quest = questAPI.CreateQuest(getInstance.PlayerAPI.Player);

        description.text = quest.Description;
        if (quest.Difficulty <= 3)
            difficulty.sprite = easy;
        else if (quest.Difficulty <= 6)
            difficulty.sprite = medium;
        else
            difficulty.sprite = hard;

    }

    public void StartAdventure()
    {
        getInstance.GameManager.ConfigureAdventure(quest, transform.position, gameObject);
    }
}
