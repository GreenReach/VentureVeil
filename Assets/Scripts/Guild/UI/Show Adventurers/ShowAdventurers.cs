using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;
public class ShowAdventurers : MonoBehaviour
{
    public GameObject InfoPanel; //The panels in which the adventurers will be shown
    public GameObject ScrollContent; //The scroll list content where the panels will go 

    //VVS
    private GetInstance getInstance;
    private AdventurerAPI adventurerAPI;
    private List<Adventurer> playerAdventurers;

    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        adventurerAPI = getInstance.AdventurerAPI;
        playerAdventurers = adventurerAPI.getAdventurers();

        PopulateTab();
    }

    private void PopulateTab()
    {
        for (int i = 0; i < playerAdventurers.Count; i++)
        {
            GameObject advPanel = Instantiate(InfoPanel, new Vector3(0, 0, 0), Quaternion.identity);
            advPanel.GetComponent<AdventurerInfoPanel>().ConfigureInfo(playerAdventurers[i]);
            advPanel.transform.SetParent(ScrollContent.transform);
        }
    }
}
