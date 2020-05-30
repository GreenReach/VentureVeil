using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;

public class QuestRecords : MonoBehaviour
{

    public GameObject questRecord;
    public GameObject contentPanel;

    private GetInstance getInstance;
    private List<FinnishedQuest> quests;
    private List<GameObject> questRecords;
    
    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        quests = getInstance.FinnishedQuestAPI.FinnishedQuests;

        GameObject record;
        questRecords = new List<GameObject>();
        for(int i = 0; i < quests.Count; i++)
        {
            record = Instantiate(questRecord, contentPanel.transform);
            record.GetComponent<QuestRecord>().Configure(quests[i]);

            questRecords.Add(record);
        }
    }

}
