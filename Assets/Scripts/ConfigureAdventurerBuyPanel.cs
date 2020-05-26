using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ConfigureAdventurerBuyPanel : MonoBehaviour
{
    public Text adventurerInfo;
    public Button buyAdventurer;
    public Adventurer adventurer;
    private int price;

    private void Start()
    {
        buyAdventurer.onClick.AddListener(BuyAdventurer);
    }

    public void ConfigureInfo(Adventurer adv)
    {
        adventurer = adv;
        price = GameObject.Find("GameManager").GetComponent<AdventurerAPI>().CalculatePrice(adv);
        adventurerInfo.text = "Name:" + adv.FirstName + adv.LastName + (adv.Gender.Equals("Male") ? " M" : " F") + "\n" + "HP: " + adv.Hp.ToString() +
                      " Stamina: " + adv.Stamina.ToString() + "\n" + "STR: " + adv.Strength.ToString() + " AGI: " + adv.Agility.ToString()
                      + " INT: " + adv.Intelligence.ToString() + "\n" + "Price: " + price.ToString() + " GOLD";
        

    }

    public void BuyAdventurer()
    {
        GameObject.Find("AdventurerFinderTabCanvas(Clone)").GetComponent<FindAdventurerTab>().BuyAdventurer(adventurer, price);
    }
}
