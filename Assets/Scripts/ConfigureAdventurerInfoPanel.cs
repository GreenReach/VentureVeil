using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ConfigureAdventurerInfoPanel : MonoBehaviour
{

    public Text adventurerInfo;

    private void Start()
    {
        adventurerInfo = gameObject.GetComponentInChildren<Text>();
    }

    public void ConfigureInfo(Adventurer adv)
    {
        adventurerInfo.text = "Name:" +  adv.FirstName + " " + adv.LastName + (adv.Gender.Equals("Male") ? " M" : " F") + "\n" + "HP: " + adv.Hp.ToString() +
                              " Stamina: " + adv.Stamina.ToString() + "\n" + "STR: " + adv.Strength.ToString() + " AGI: " + adv.Agility.ToString()
                              + " INT: " + adv.Intelligence.ToString();
    }

}
