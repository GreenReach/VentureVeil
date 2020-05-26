using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerBuyButton : MonoBehaviour
{

    public Sprite defaultButton, hoverButton;
    public WanderingAdventurer wanderingAdventurer;

    private SpriteRenderer myObj;

    private void Start()
    {
        myObj = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        myObj.sprite = hoverButton;
        wanderingAdventurer.Pause();
    }

    private void OnMouseExit()
    {
        myObj.sprite = defaultButton;
        wanderingAdventurer.Unpause();
    }

    //The adventurer is bought
    private void OnMouseDown()
    {
        wanderingAdventurer.BuyAdventurer();
    }
}
