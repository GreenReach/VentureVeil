using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VentureVeilStructures;

public class SupplyShop : MonoBehaviour
{ 
    public Sprite defaultSprite, hoverSprite;
    public GameObject supplyShopPanel;
    public TextMeshPro priceText;
    public int price { get; private set; }

    private bool isBuying;
    private SpriteRenderer objectSprite;
    private GetInstance getInstance;

    private void Start()
    {
        isBuying = false;
        objectSprite = GetComponent<SpriteRenderer>();
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        price = Mathf.RoundToInt(Random.Range(3, 10));
        priceText.text = price.ToString();

        InvokeRepeating("ChangePrice", 0, VVC.hourDuration * 24);
    }

    private void ChangePrice()
    {
        if(Random.Range(0,1) == 1)
        {
            price = Mathf.RoundToInt(Random.Range(3, 10));
            priceText.text = price.ToString();
        }
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
        if (isBuying)
            getInstance.SoundManager.Play("doorClose");
        else
            getInstance.SoundManager.Play("doorbell");

        isBuying = !isBuying;
        supplyShopPanel.SetActive(isBuying);
    }

}
