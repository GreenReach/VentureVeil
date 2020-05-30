using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class QuestRecord : MonoBehaviour
{
    public Text questInfo;
    private FinnishedQuest quest;

    public void Configure(FinnishedQuest q)
    {
        quest = q;
        questInfo.text = quest.Quest.Description + " Rewaward got:" + quest.Quest.Reward + " Party count:" + quest.Party.Count + "Was succssfull: " + quest.Successfull + " Told at court:" + quest.TaleTold ;

    }
}
