using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;

public class QuestStartButton : MonoBehaviour
{
    public Sprite defaultButton, hoverButton;
    public QuestSign questSign;

    private SpriteRenderer myObj;
    private GetInstance getInstance;

    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        myObj = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        myObj.sprite = hoverButton;
    }

    private void OnMouseExit()
    {
        myObj.sprite = defaultButton;
    }

    //The quest starts
    private void OnMouseDown()
    {
        getInstance.GameManager.ChangeMouseMode(MouseModes.UI);
        questSign.StartAdventure();  
    }
}
