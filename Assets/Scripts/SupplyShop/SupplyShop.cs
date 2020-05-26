using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyShop : MonoBehaviour
{
    public Sprite defaultSprite, hoverSprite;
    public GameObject supplyShopPanel;

    private bool isBuying;
    private SpriteRenderer objectSprite;

    private void Start()
    {
        isBuying = false;
        objectSprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        objectSprite.sprite = hoverSprite;
    }

    private void OnMouseExit()
    { 
        if(!isBuying)
         objectSprite.sprite = defaultSprite;
    }

    private void OnMouseDown()
    {
        isBuying = !isBuying;
        supplyShopPanel.SetActive(isBuying);
    }

}
