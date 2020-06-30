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
    public void SaveFinnishedQuest(string username)
    {
        SaveToXML(username);
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

    void SaveToXML(string username)
    {
        string filepath = VVC.playerInfoPath + username;
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath + "/FinnishedQuests.xml");

        XmlNode nodeList = doc.SelectSingleNode("/FinnishedQuests");
        nodeList.RemoveAll();

        XmlNode root = doc.SelectSingleNode("/FinnishedQuests");
        for(int i = 0; i < FinnishedQuests.Count; i++)
        {
            XmlNode fq = doc.CreateElement("FinnishedQuest");

            XmlNode succ = doc.CreateElement("Successfull");
            succ.InnerText = FinnishedQuests[i].Successfull.ToString();
            XmlNode rating = doc.CreateElement("Rating");
            rating.InnerText = FinnishedQuests[i].Rating.ToString();
            XmlNode taleTold = doc.CreateElement("TaleTold");
            taleTold.InnerText = FinnishedQuests[i].TaleTold.ToString();

            fq.AppendChild(succ);
            fq.AppendChild(rating);
            fq.AppendChild(taleTold);

            XmlNode quest = SaveQuestXML(FinnishedQuests[i].Quest,doc);
            fq.AppendChild(quest);

            XmlNode party = doc.CreateElement("Party");
            for (int j =0; j < FinnishedQuests[i].Party.Count; j++)
            {
                XmlNode adv = SaveAdventurerXML(FinnishedQuests[i].Party[j], doc);
                party.AppendChild(adv);
            }
            fq.AppendChild(party);

            root.AppendChild(fq);
        }

        doc.Save(filepath + "/FinnishedQuests.xml");
    }
    XmlNode SaveQuestXML(Quest q, XmlDocument doc)
    {
        XmlNode quest = doc.CreateElement("Quest");

        XmlNode desc = doc.CreateElement("Description");
        desc.InnerText = q.Description;
        XmlNode rSta = doc.CreateElement("RequiredSTA");
        rSta.InnerText = q.RequiredSTA.ToString();
        XmlNode rStr = doc.CreateElement("RequiredSTR");
        rStr.InnerText = q.RequiredSTR.ToString();
        XmlNode rAgy = doc.CreateElement("RequiredAGY");
        rAgy.InnerText = q.RequiredAGY.ToString();
        XmlNode rInt = doc.CreateElement("RequiredINT");
        rInt.InnerText = q.RequiredINT.ToString();
        XmlNode diff = doc.CreateElement("Difficulty");
        diff.InnerText = q.Difficulty.ToString();
        XmlNode reward = doc.CreateElement("Reward");
        reward.InnerText = q.Reward.ToString();
        XmlNode slots = doc.CreateElement("Slots");
        slots.InnerText = q.Slots.ToString();

        quest.AppendChild(desc);
        quest.AppendChild(rSta);
        quest.AppendChild(rStr);
        quest.AppendChild(rAgy);
        quest.AppendChild(rInt);
        quest.AppendChild(diff);
        quest.AppendChild(reward);
        quest.AppendChild(slots);

        return quest;

    }
    XmlNode SaveAdventurerXML(Adventurer a, XmlDocument doc)
    {
        XmlNode adv = doc.CreateElement("Adventurer");

        XmlNode fName = doc.CreateElement("FirstName");
        fName.InnerText = a.FirstName;
        XmlNode lName = doc.CreateElement("LastName");
        lName.InnerText = a.LastName;
        XmlNode gender = doc.CreateElement("Gender");
        gender.InnerText = a.Gender;
        XmlNode hp = doc.CreateElement("Hp");
        hp.InnerText = a.Hp.ToString();
        XmlNode stamina = doc.CreateElement("Stamina");
        stamina.InnerText = a.Stamina.ToString();
        XmlNode str = doc.CreateElement("Strength");
        str.InnerText = a.Strength.ToString();
        XmlNode agy = doc.CreateElement("Agility");
        agy.InnerText = a.Agility.ToString();
        XmlNode intt = doc.CreateElement("Intelligence");
        intt.InnerText = a.Intelligence.ToString();
        XmlNode strXP = doc.CreateElement("StrengthXP");
        strXP.InnerText = a.StrengthXP.ToString();
        XmlNode agyXP = doc.CreateElement("AgilityXP");
        agyXP.InnerText = a.AgilityXP.ToString();
        XmlNode inttXP = doc.CreateElement("IntelligenceXP");
        inttXP.InnerText = a.IntelligenceXP.ToString();
        XmlNode cSta = doc.CreateElement("CurrentStamina");
        cSta.InnerText = a.CurrentStamina.ToString();

        adv.AppendChild(fName);
        adv.AppendChild(lName);
        adv.AppendChild(gender);
        adv.AppendChild(hp);
        adv.AppendChild(stamina);
        adv.AppendChild(str);
        adv.AppendChild(agy);
        adv.AppendChild(intt);
        adv.AppendChild(strXP);
        adv.AppendChild(agyXP);
        adv.AppendChild(inttXP);
        adv.AppendChild(cSta);

        return adv;
    }
    public int CalculateRating( FinnishedQuest q)
    {
        return 10;
    }

}
