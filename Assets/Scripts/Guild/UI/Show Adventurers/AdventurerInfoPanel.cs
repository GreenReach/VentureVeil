using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class AdventurerInfoPanel : MonoBehaviour
{

    public TextMeshProUGUI Name;
    public Text Health;
    public Text Stamina;
    public Text Strenght;
    public Text Agility;
    public Text Intelligence;
    public RectTransform strenghtXPMask, agilityXPMask, intelligenceXPMask;

    private Adventurer adventurer;


    public void ConfigureInfo(Adventurer adv)
    {
        adventurer = adv;
        Name.text = adventurer.FirstName + " " + adventurer.LastName;
        Health.text = adventurer.Hp.ToString();
        Stamina.text = adventurer.Stamina.ToString();
        Strenght.text = adventurer.Strength.ToString();
        Agility.text = adventurer.Agility.ToString();
        Intelligence.text = adventurer.Intelligence.ToString();
        strenghtXPMask.sizeDelta = new Vector2(strenghtXPMask.sizeDelta.x - (strenghtXPMask.sizeDelta.x * adv.StrengthXP / 100), strenghtXPMask.sizeDelta.y);
        agilityXPMask.sizeDelta = new Vector2(agilityXPMask.sizeDelta.x - (agilityXPMask.sizeDelta.x * adv.AgilityXP / 100), agilityXPMask.sizeDelta.y);
        intelligenceXPMask.sizeDelta = new Vector2(intelligenceXPMask.sizeDelta.x - (intelligenceXPMask.sizeDelta.x * adv.IntelligenceXP / 100), intelligenceXPMask.sizeDelta.y);

    }
}
