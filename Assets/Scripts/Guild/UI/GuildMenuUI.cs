using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class GuildMenuUI : MonoBehaviour
{
    public Button closeButton;
    public Button showAdventurersButton;
    public Button roomsButton;
    public Button questRecordsButton;

    public GameObject guildPanel;
    public GameObject roomsPanel;
    public GameObject showAdventurers;
    public GameObject questRecords;

    private GameObject currentPanel;
    private GetInstance getInstance;

    // Start is called before the first frame update
    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        closeButton.onClick.AddListener(Close);
        showAdventurersButton.onClick.AddListener(ShowAdventurersScreen);
        roomsButton.onClick.AddListener(RoomsScreen);
        questRecordsButton.onClick.AddListener(QuestRecords);
    }

    void ShowAdventurersScreen()
    {
        Destroy(currentPanel);
        currentPanel = Instantiate(showAdventurers,guildPanel.transform);
    }

    void RoomsScreen()
    {
        Destroy(currentPanel);
        currentPanel = Instantiate(roomsPanel, guildPanel.transform);
    }

    void QuestRecords()
    {
        Destroy(currentPanel);
        currentPanel = Instantiate(questRecords, guildPanel.transform);
    }

    void Close()
    {
        getInstance.GameManager.ChangeMouseMode(MouseModes.WORLD);
        Destroy(gameObject);
    }
}
