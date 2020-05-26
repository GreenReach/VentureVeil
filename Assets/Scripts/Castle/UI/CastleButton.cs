using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleButton : MonoBehaviour
{
    public Sprite defaultSprite, hoverSprite;
    public CastleMenu castleMenu;
    public string buttonAction;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro buttonText;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buttonText = GetComponentInChildren<TextMeshPro>();
        if (buttonAction.Equals("royal"))
            buttonText.text = "Price/n 1000 GOLD";
        else if(buttonAction.Equals("nobility"))
            buttonText.text = "Price/n 800 GOLD";
        else if (buttonAction.Equals("courtier"))
            buttonText.text = "Price/n 600 GOLD";
        else if (buttonAction.Equals("tale"))
            buttonText.text = "Tell Tales";

    }

    private void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnMouseDown()
    {
        castleMenu.ButtonPressed(buttonAction);
    }
}
