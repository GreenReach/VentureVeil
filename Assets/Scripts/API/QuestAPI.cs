using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class QuestAPI : MonoBehaviour
{
    ///TO DO: lucreaza la statusurile de care are nevoie 
    public Quest CreateQuest()
    {
        int reward, diff, STA, STR, AGY, INT, slots;
        string description;

        //Extrag un quest din QuestData
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/QuestData.xml");

        int count = doc.SelectNodes("/Quests/Quest").Count; ;
        string path = "/Quests/Quest[position() = " + Random.Range(1,count).ToString() + "]";

        description = doc.SelectSingleNode(path + "/Description").InnerText;
        slots = int.Parse(doc.SelectSingleNode(path + "/Slots").InnerText);
        STA =Random.Range( int.Parse(doc.SelectSingleNode(path + "/MinStamina").InnerText), int.Parse(doc.SelectSingleNode(path + "/MaxStamina").InnerText));
        STR = Random.Range(int.Parse(doc.SelectSingleNode(path + "/MinStrenght").InnerText), int.Parse(doc.SelectSingleNode(path + "/MaxStrenght").InnerText));
        AGY = Random.Range(int.Parse(doc.SelectSingleNode(path + "/MinAgility").InnerText), int.Parse(doc.SelectSingleNode(path + "/MaxAgility").InnerText));
        INT = Random.Range(int.Parse(doc.SelectSingleNode(path + "/MinIntelligence").InnerText),int.Parse(doc.SelectSingleNode(path + "/MaxIntelligence").InnerText));
        reward = CalculateReward(STA, STR, AGY, INT);
        diff = CalculateDifficulty(STR, AGY, INT);

        return new Quest(description, STA, STR, AGY, INT, reward, slots, diff);

    }

    private int CalculateReward(int STA, int STR, int AGY, int INT)
    {
        int reward = STA * 10 + STR * 15 + AGY * 15 + INT * 15;
        return reward;
    }

    public int CalculateDifficulty(int STR, int AGY, int INT)
    {
        int atrDiff = Mathf.Max(STR, Mathf.Max(INT, AGY))/3 + 2*(STR + AGY + INT)/3;
        return atrDiff / 3; 
    }
}
