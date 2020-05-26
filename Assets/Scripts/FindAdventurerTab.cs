using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class FindAdventurerTab : MonoBehaviour
{
    public GameObject adventurerListContent;
    public GameObject adventurerBuyPanel;
    public Button closeButton;

    private List<Adventurer> availableAdventurers;
    private AdventurerAPI adventurerAPI;

    private List<GameObject> buyPanels;

    private GetInstance getInstance;

    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        availableAdventurers = new List<Adventurer>();
        adventurerAPI = getInstance.AdventurerAPI;

        buyPanels = new List<GameObject>();

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
        for (int i = 0; i < Random.Range(3, 7); i++)
        {
            Adventurer adv = adventurerAPI.CreateAdventurer();
            availableAdventurers.Add(adv);

            GameObject buyPanel = Instantiate(adventurerBuyPanel, new Vector3(0, 0, 0), Quaternion.identity);
            buyPanel.GetComponent<ConfigureAdventurerBuyPanel>().ConfigureInfo(adv);
            buyPanel.transform.SetParent(adventurerListContent.transform);

            buyPanels.Add(buyPanel);
        }
    }

    private void DeleteFromTab(Adventurer adv)
    {
        for( int i = 0; i< availableAdventurers.Count;i++)
            if(availableAdventurers[i].Equals(adv))
            {
                Destroy(buyPanels[i]);
                buyPanels.RemoveAt(i);

                availableAdventurers.RemoveAt(i);
                break;
            }
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    public void BuyAdventurer(Adventurer adventurer, int price)
    {
        DeleteFromTab(adventurer);
        getInstance.GameManager.BuyAdventurer(adventurer, price);
    }
}
