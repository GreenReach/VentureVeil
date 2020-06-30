using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class PlayerAPI : MonoBehaviour
{
    public Player Player { get; set; }


    public void  LoadPlayer(string username)
    {
        Player = new Player();
        LoadFromXML(username);
        
        CalculateUnlocks();
        ApplyUnlocks();
    }

    public void CreatePlayer(Profile p)
    {
        Directory.CreateDirectory(VVC.userDataDirPath + p.Username);
        CreateXML(p);
    }

    public void UpdatePlayer(Player p) // can update everything except username ( for now)
    {
        UpdateXML(p);
    }

    private void LoadFromXML(string username)
    {
        Player = new Player();
        string filepath = VVC.playerInfoPath + username;

        XmlDocument doc = new XmlDocument();
        doc.Load(filepath + "/PlayerInfo.xml");
        XmlNode node = doc.SelectSingleNode("/Player");


        //citire
        Player.Username = node.SelectSingleNode("Username").InnerText;
        Player.Gold = int.Parse(node.SelectSingleNode("Gold").InnerText);
        Player.Supply = int.Parse(node.SelectSingleNode("Supply").InnerText);
        Player.NumberOfRooms = int.Parse(node.SelectSingleNode("NumberOfRooms").InnerText);
        Player.adventurersLimit = 2; // default value;

        //Guild Location 
        if (node.SelectSingleNode("GuildLocation").InnerText != "0")
        {
            string[] pos = node.SelectSingleNode("GuildLocation").InnerText.Split(' ');
            Player.guildLocation = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
        }

        //RoomType
        string[] roomTypesStr = node.SelectSingleNode("RoomType").InnerText.Split(' ');
        List<Room> rooms = new List<Room>();
        for (int i = 0; i < roomTypesStr.Length; i += 2)
        {
            Room room = new Room(int.Parse(roomTypesStr[i]), int.Parse(roomTypesStr[i + 1]));
            rooms.Add(room);

            if (room.Type == 1)
                Player.adventurersLimit +=room.Level + 1; // calculte the limit like so: first level is 2 adv, the next ones are one adv per level
        }

        //Unlocks
        string[] unlocks = node.SelectSingleNode("Unlocks").InnerText.Split(' ');
        for(int i = 0; i< VVC.unlocksNumber; i++ )
            Player.Unlocks[i] = bool.Parse(unlocks[i]);

        Player.courtierFavors = int.Parse(node.SelectSingleNode("CourtierFavors").InnerText);
        Player.nobilityFavors = int.Parse(node.SelectSingleNode("NobilityFavors").InnerText);
        Player.royalFavors = int.Parse(node.SelectSingleNode("RoyalFavors").InnerText);
        Player.kingdom = int.Parse(node.SelectSingleNode("Kingdom").InnerText);
        Player.currentDay = int.Parse(node.SelectSingleNode("Day").InnerText);
        Player.currentHour = int.Parse(node.SelectSingleNode("Hour").InnerText);
        Player.currentGameDuration = int.Parse(node.SelectSingleNode("GameDuration").InnerText);

        Player.Rooms = rooms;
        UpdateRooms();

    }
    private void CreateXML(Profile p)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(declaration);

        XmlNode root = doc.CreateElement("Player");

        XmlNode user = doc.CreateElement("Username");
        user.InnerText = p.Username;

        XmlNode gold = doc.CreateElement("Gold");
        gold.InnerText = VVC.defaultGold.ToString();

        XmlNode supply = doc.CreateElement("Supply");
        supply.InnerText = VVC.defaultSupply.ToString();

        XmlNode numberOfRoom = doc.CreateElement("NumberOfRooms");
        numberOfRoom.InnerText = VVC.defaultNumberOfRooms.ToString();

        XmlNode roomType = doc.CreateElement("RoomType");
        roomType.InnerText = "1 1 0 0 0 0 0 0";

        XmlNode courtierFavors = doc.CreateElement("CourtierFavors");
        courtierFavors.InnerText = "0";

        XmlNode nobilityFavors = doc.CreateElement("NobilityFavors");
        nobilityFavors.InnerText = "0";

        XmlNode royalFavors = doc.CreateElement("RoyalFavors");
        royalFavors.InnerText = "0";

        XmlNode unlocks = doc.CreateElement("Unlocks");
        unlocks.InnerText = "";
        for(int i = 0; i<VVC.unlocksNumber; i++)
            unlocks.InnerText += "False ";

        XmlNode guildLoc = doc.CreateElement("GuildLocation");
        guildLoc.InnerText = "0";

        XmlNode kingdom = doc.CreateElement("Kingdom");
        kingdom.InnerText = "0";

        XmlNode day = doc.CreateElement("Day");
        day.InnerText = "-1";

        XmlNode hour = doc.CreateElement("Hour");
        hour.InnerText = "-1";

        XmlNode gameDuration = doc.CreateElement("GameDuration");
        gameDuration.InnerText = "0";

        root.AppendChild(user);
        root.AppendChild(gold);
        root.AppendChild(supply);
        root.AppendChild(numberOfRoom);
        root.AppendChild(roomType);
        root.AppendChild(courtierFavors);
        root.AppendChild(nobilityFavors);
        root.AppendChild(royalFavors);
        root.AppendChild(unlocks);
        root.AppendChild(guildLoc);
        root.AppendChild(kingdom);
        root.AppendChild(day);
        root.AppendChild(hour);
        root.AppendChild(gameDuration);

        doc.AppendChild(root);
        doc.Save("Assets/Data/ProfilesData/" + p.Username + "/PlayerInfo.xml");
    }

    private void UpdateXML(Player p)
    {
        string filepath = VVC.playerInfoPath + p.Username;
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath + "/PlayerInfo.xml");
        XmlNode node = doc.SelectSingleNode("/Player");

        node.SelectSingleNode("Gold").InnerText = p.Gold.ToString();
        node.SelectSingleNode("CourtierFavors").InnerText = p.courtierFavors.ToString();
        node.SelectSingleNode("NobilityFavors").InnerText = p.nobilityFavors.ToString();
        node.SelectSingleNode("RoyalFavors").InnerText = p.royalFavors.ToString();
        node.SelectSingleNode("GuildLocation").InnerText = p.guildLocation.x + " " + p.guildLocation.y + " " + p.guildLocation.z;
        node.SelectSingleNode("Kingdom").InnerText = p.kingdom.ToString();
        node.SelectSingleNode("Day").InnerText = p.currentDay.ToString();
        node.SelectSingleNode("Hour").InnerText = p.currentHour.ToString();
        node.SelectSingleNode("GameDuration").InnerText = p.currentGameDuration.ToString();

        string unlocks = "";
        for (int i = 0; i < VVC.unlocksNumber; i++)
            unlocks += p.Unlocks[i].ToString() + " ";
        node.SelectSingleNode("Unlocks").InnerText = unlocks;

        doc.Save(filepath + "/PlayerInfo.xml");
    }

    public Player getPlayer() { return Player; }

    public void Test() { Debug.Log("TEST"); }

    public void UpdateRooms()
    {
        //After a room has been built/upgraded this function updates the data
        int advLimit = 2 + Player.adventurerLimitBonus; // base number
        int passiveSTR = 0, passiveAGY = 0, passiveINT = 0;

        for (int i = 0; i < Player.NumberOfRooms; i++)
        {
            switch (Player.Rooms[i].Type)
            {
                case 1:
                    //Calculate the adventurers limit
                    advLimit += Player.Rooms[i].Level + 1;
                    break;
                case 2:
                    //Calculate the passive strength
                    passiveSTR = 5 + Player.Rooms[i].Level;
                    break;
                case 3:
                    //Calculate the passive agility
                    passiveAGY = 5 + Player.Rooms[i].Level;
                    break;
                case 4:
                    //Calculate the passive intelligence
                    passiveINT = 5 + Player.Rooms[i].Level;
                    break;

            }

        }

        Player.adventurersLimit = advLimit;
        Player.passiveStrengthGain = passiveSTR;
        Player.passiveAgilityGain = passiveAGY;
        Player.passiveIntelligenceGain = passiveINT;

    }

    public void UpdateHall()
    {
        if (Player.NumberOfRooms > Player.Rooms.Count)
            for (int i = 0; i < Player.NumberOfRooms - Player.Rooms.Count; i++)
                Player.Rooms.Add(new Room());
    }

    void CalculateUnlocks()
    {
        if (Player.Unlocks[(int)UnlockedFeature.greatPlanner])
            Player.adventurerLimitBonus += 3;
        if (Player.Unlocks[(int)UnlockedFeature.negociator])
            Player.adventurerLimitBonus += 10;
    }

    void ApplyUnlocks()
    {
        Player.adventurersLimit += Player.adventurerLimitBonus;
    }
}
