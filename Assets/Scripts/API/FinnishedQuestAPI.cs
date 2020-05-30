using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class FinnishedQuestAPI : MonoBehaviour
{
    public List<FinnishedQuest> FinnishedQuests { get; set; }

    public void LoadFinnishedQuests(string username)
    {
        FinnishedQuests = new List<FinnishedQuest>();
        LoadFromXML(username);
    }

    public void CreateFinnishedQuestFile(Profile p)
    {
        CreateXML(p);
    }

    public void AddQuest(FinnishedQuest fq)
    {
        FinnishedQuests.Add(fq);
    }

    void LoadFromXML(string username)
    {
        string filepath = VVC.playerInfoPath + username;

        XmlDocument doc = new XmlDocument();
        doc.Load(filepath + "/FinnishedQuests.xml");
        XmlNodeList nodeList = doc.SelectNodes("/FinnishedQuests/FinnishedQuest");


        //citire
        foreach (XmlNode node in nodeList)
        {
            FinnishedQuest fq = new FinnishedQuest();

            fq.Successfull = bool.Parse(node.SelectSingleNode("Successfull").InnerText);
            fq.Rating = int.Parse(node.SelectSingleNode("Rating").InnerText);
            fq.TaleTold = bool.Parse(node.SelectSingleNode("TaleTold").InnerText);

            fq.Quest = LoadQuestXML(node);
            fq.Party = LoadPartyXML(node);

            FinnishedQuests.Add(fq);
        }
    }
    Quest LoadQuestXML(XmlNode node)
    {
        XmlNode questNode = node.SelectSingleNode("Quest");
        Quest q = new Quest();

        q.Description = questNode.SelectSingleNode("Description").InnerText;
        q.RequiredSTA = int.Parse(questNode.SelectSingleNode("RequiredSTA").InnerText);
        q.RequiredSTR = int.Parse(questNode.SelectSingleNode("RequiredSTR").InnerText);
        q.RequiredAGY = int.Parse(questNode.SelectSingleNode("RequiredAGY").InnerText);
        q.RequiredINT = int.Parse(questNode.SelectSingleNode("RequiredINT").InnerText);
        q.Difficulty = int.Parse(questNode.SelectSingleNode("Difficulty").InnerText);
        q.Reward = int.Parse(questNode.SelectSingleNode("Reward").InnerText);
        q.Slots = int.Parse(questNode.SelectSingleNode("Slots").InnerText);

        return q;
    }
    List<Adventurer> LoadPartyXML(XmlNode node)
    {
        List<Adventurer> advs = new List<Adventurer>();
        XmlNodeList partyNodes = node.SelectNodes("Party/Adventurer");

        foreach(XmlNode advNode in partyNodes)
        {
            Adventurer adv = new Adventurer();

            adv.FirstName = advNode.SelectSingleNode("FirstName").InnerText;
            adv.LastName = advNode.SelectSingleNode("LastName").InnerText;
            adv.Gender = advNode.SelectSingleNode("Gender").InnerText;
            adv.Hp = int.Parse(advNode.SelectSingleNode("Hp").InnerText);
            adv.Stamina = int.Parse(advNode.SelectSingleNode("Stamina").InnerText);
            adv.Strength = int.Parse(advNode.SelectSingleNode("Strength").InnerText);
            adv.Agility = int.Parse(advNode.SelectSingleNode("Agility").InnerText);
            adv.Intelligence = int.Parse(advNode.SelectSingleNode("Intelligence").InnerText);
            adv.StrengthXP = int.Parse(advNode.SelectSingleNode("StrengthXP").InnerText);
            adv.AgilityXP = int.Parse(advNode.SelectSingleNode("AgilityXP").InnerText);
            adv.IntelligenceXP = int.Parse(advNode.SelectSingleNode("IntelligenceXP").InnerText);
            advs.Add(adv);

        }
        return advs;

    }

    void CreateXML(Profile p)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(declaration);

        XmlNode root = doc.CreateElement("FinnishedQuests");

        doc.AppendChild(root);
        doc.Save("Assets/Data/ProfilesData/" + p.Username + "/FinnishedQuests.xml");
    }

    public int CalculateRating( FinnishedQuest q)
    {
        return 10;
    }

}
