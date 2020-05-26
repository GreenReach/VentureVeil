using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupplyShopPanel : MonoBehaviour
{
    public TextMeshPro supplyCountText, currentPriceText;

    private GetInstance getInstance;
    private int supplyCount;
    private int currentPrice;
    private int pricePerSupply = 5;

    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        supplyCount = 0;
        currentPrice = 0;
        supplyCountText.text = supplyCount.ToString();
        currentPriceText.text = "PRICE: " + currentPrice;
    }

    public void ButtonAction(int x)
    {
        if (x != 0)
        {
            supplyCount += x;
            if (supplyCount < 0)
                supplyCount = 0;
            currentPrice = pricePerSupply * supplyCount;
            supplyCountText.text = supplyCount.ToString();
            currentPriceText.text = "PRICE: " + currentPrice;

        }
        else
        {
            getInstance.GameManager.BuySupplies(supplyCount, currentPrice);
            gameObject.SetActive(false);
        }
    }
}
