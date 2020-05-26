using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ChooseAdventurerTab : MonoBehaviour
{
    public GameObject adventurerBuyContent;
    public GameObject infoPanel;
    public Button closeButton;

    private List<Adventurer> playerAdventurers;
    private AdventurerAPI adventurerAPI;

    private GetInstance getInstance;

    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        adventurerAPI = getInstance.AdventurerAPI;

        playerAdventurers = adventurerAPI.getAdventurers();
        closeButton.onClick.AddListener(CloseWindow);
        PopulateTab();

     /*   GameObject testText = new GameObject();
        testText.transform.parent = adventurerListScrollView.transform;
        Text tt = testText.AddComponent<Text>();
        tt.color = new Color(0, 0, 0);
        tt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font; ;
        tt.text = "Dynamically created text";
        */
    }

    private void PopulateTab()
    {
        for (int i = 0; i < playerAdventurers.Count; i++)
        {
            GameObject advPanel = Instantiate(infoPanel, new Vector3(0, 0, 0), Quaternion.identity);
            advPanel.GetComponent<ConfigureAdventurerInfoPanel>().ConfigureInfo(playerAdventurers[i]);
            advPanel.transform.SetParent(adventurerBuyContent.transform);
        }
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }


}
