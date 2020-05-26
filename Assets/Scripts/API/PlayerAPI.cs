using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class PlayerAPI : MonoBehaviour
{
    public Player Player { get; set; }


    public void  LoadPlayer()
    {
        Player = new Player();
        LoadFromXML();
    }

    private void LoadFromXML()
    {
        Player = new Player();

        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/PlayerInfo.xml");
        XmlNode node = doc.SelectSingleNode("/Player");


        //citire
        Player.Username = node.SelectSingleNode("Username").InnerText;
        Player.Gold = int.Parse(node.SelectSingleNode("Gold").InnerText);
        Player.Supply = int.Parse(node.SelectSingleNode("Supply").InnerText);
        Player.NumberOfRooms = int.Parse(node.SelectSingleNode("NumberOfRooms").InnerText);
        Player.adventurersLimit = 2; // default value;
        string[] roomTypesStr = node.SelectSingleNode("RoomType").InnerText.Split(' ');
        List<Room> rooms = new List<Room>();
        for (int i = 0; i < roomTypesStr.Length; i += 2)
        {
            Room room = new Room(int.Parse(roomTypesStr[i]), int.Parse(roomTypesStr[i + 1]));
            rooms.Add(room);

            if (room.Type == 1)
                Player.adventurersLimit +=room.Level + 1; // calculte the limit like so: first level is 2 adv, the next ones are one adv per level
        }
        Player.Rooms = rooms;

    }


    public Player getPlayer() { return Player; }
    public void Test() { Debug.Log("TEST"); }

    public void UpdateRooms()
    {
        //After a room has been built/upgraded this function updates the data
        int advLimit = 2; // base number
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
}
