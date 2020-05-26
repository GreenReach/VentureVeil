using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInstance : MonoBehaviour
{
    public GameObject WorldMap { get; private set; }
    public GameManager GameManager { get; private set; }
    public AdventurerAPI AdventurerAPI { get; private set; }
    public QuestAPI QuestAPI { get; private set; }
    public PlayerAPI PlayerAPI { get; private set; }
    public GameObject Guild { get; set; }
    public GameObject UICanvas { get; set; }

    private string scriptsObj;

    public void Load()
    {
        scriptsObj = "GameManager";
        WorldMap = GameObject.Find("Plane");
        UICanvas = GameObject.Find("Map UI Canvas");
        GameManager = GameObject.Find(scriptsObj).GetComponent<GameManager>();
        AdventurerAPI = GameObject.Find(scriptsObj).GetComponent<AdventurerAPI>();
        QuestAPI = GameObject.Find(scriptsObj).GetComponent<QuestAPI>();
        PlayerAPI = GameObject.Find(scriptsObj).GetComponent<PlayerAPI>();
    }
}
