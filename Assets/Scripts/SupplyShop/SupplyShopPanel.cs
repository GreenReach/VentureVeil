using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupplyShopPanel : MonoBehaviour
{
    public TextMeshPro supplyCountText, currentPriceText;
    public SupplyShop supplyShop;

    private GetInstance getInstance;
    private int supplyCount;
    private int currentPrice;
    private int pricePerSupply;

    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        supplyCount = 0;
        currentPrice = 0;
        supplyCountText.text = supplyCount.ToString() + " S";
        currentPriceText.text = currentPrice + " G";
    }

    private void OnEnable()
    {
        pricePerSupply = supplyShop.price;
    }

    public void ButtonAction(int x)
    {
        getInstance.SoundManager.Play("button");

        if (x != 0)
        {
            supplyCount += x;
            if (supplyCount < 0)
                supplyCount = 0;
            currentPrice = pricePerSupply * supplyCount;
            supplyCountText.text = supplyCount.ToString() + " S";
            currentPriceText.text = currentPrice + " G";

        }
        else
        {
            getInstance.SoundManager.Play("doorClose");

            getInstance.GameManager.BuySupplies(supplyCount, currentPrice);
            gameObject.SetActive(false);
        }
    }
}
