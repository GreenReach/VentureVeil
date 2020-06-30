using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyShopButton : MonoBehaviour
{
    public Sprite defaultSprite, hoverSprite;
    public int buttonActionValue = 1; // if it is 0 then buys the supplies, else adds the suplies to the total

    private SpriteRenderer objectSprite;
    private SupplyShopPanel supplyShopPanel;

    private void Start()
    {
        objectSprite = gameObject.GetComponent<SpriteRenderer>();
        supplyShopPanel = gameObject.GetComponentInParent<SupplyShopPanel>();
    }

    private void OnMouseEnter()
    {
        objectSprite.sprite = hoverSprite;
    }

    private void OnMouseExit()
    {
        objectSprite.sprite = defaultSprite;
    }

    private void OnMouseDown()
    {
        supplyShopPanel.ButtonAction(buttonActionValue);
    }


}
