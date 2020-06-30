using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ChooseAdventurerScreen : MonoBehaviour
{
    public GameObject adventurerListContent;
    public GameObject adventurerSelectPanel;

    private AdventureScreen adventureScreen;
    private List<Adventurer> adventurers; //list from AdventureScreen with all the players not chosen adventurers
    private Quest quest;

    public void PopulateTab()
    {
        adventurers = adventureScreen.GetAdventurers(); 
        for (int i = 0; i < adventurers.Count; i++)
        {
            GameObject advSelectPanel = Instantiate(adventurerSelectPanel, new Vector3(0, 0, 0), Quaternion.identity);
            advSelectPanel.GetComponent<ConfigureAdventurerSelectPanel>().ConfigureInfo(adventurers[i]);
            advSelectPanel.GetComponent<ConfigureAdventurerSelectPanel>().Configure(this,quest);
            advSelectPanel.transform.SetParent(adventurerListContent.transform);
        }

        //Add empty panel 
        GameObject emptyPanel = Instantiate(adventurerSelectPanel, new Vector3(0, 0, 0), Quaternion.identity);
        emptyPanel.GetComponent<ConfigureAdventurerSelectPanel>().ConfigureEmpty();
        emptyPanel.GetComponent<ConfigureAdventurerSelectPanel>().Configure(this,quest);
        emptyPanel.transform.SetParent(adventurerListContent.transform);
    }

    public void SelectAdventurer(Adventurer adv)
    {
        adventureScreen.AddAdventurer(adv);
        CloseWindow();
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }

    public void Configure(AdventureScreen script, Quest q)
    {
        adventureScreen = script;
        quest = q;
    }


    
}
