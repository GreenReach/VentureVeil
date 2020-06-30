using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBuyButton : MonoBehaviour
{
    public Sprite defaultButton, hoverButton;
    public Builder builder;

    private SpriteRenderer myObj;

    private void Start()
    {
        myObj = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        myObj.sprite = hoverButton;
        builder.Pause();
    }

    private void OnMouseExit()
    {
        myObj.sprite = defaultButton;
        builder.Unpause();
    }

    //The builder is bought
    private void OnMouseDown()
    {
        builder.BuyBuilder();
    }
}
