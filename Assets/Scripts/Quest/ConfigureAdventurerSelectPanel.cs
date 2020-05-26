using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ConfigureAdventurerSelectPanel : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Text Health;
    public Text Stamina;
    public Text Strenght;
    public Text Agility;
    public Text Intelligence;
    public Button selectAdventurer;
    public Adventurer adventurer;
    public RectTransform strenghtXPMask, agilityXPMask, intelligenceXPMask;

    private ChooseAdventurerScreen chooseAdventurerScreen;
    private int price;

    private void Start()
    {
        selectAdventurer.onClick.AddListener(SelectAdventurer);
    }

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

    public void ConfigureEmpty()
    {
        
        adventurer = null;
        Name.text = "None";
        Health.text = "-";
        Stamina.text = "-";
        Strenght.text = "-";
        Agility.text = "-";
        Intelligence.text = "-";
    }

    public void SetScript(ChooseAdventurerScreen script)
    {
        chooseAdventurerScreen = script;
    }

    public void SelectAdventurer()
    {
        chooseAdventurerScreen.SelectAdventurer(adventurer);
    }
}
