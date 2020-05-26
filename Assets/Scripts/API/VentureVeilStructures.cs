using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VentureVeilStructures
{

    public class Adventurer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Hp { get; set; }
        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int StrengthXP { get; set; }
        public int AgilityXP { get; set; }
        public int IntelligenceXP { get; set; }

        public Adventurer(string fname = "", string lname = "", string gender = "", int hp = 0, int stamina = 0, int strength = 0, int agility = 0, int intelligence = 0)
        {
            FirstName = string.Copy(fname);
            LastName = string.Copy(lname);
            Gender = string.Copy(gender);
            Hp = hp;
            Stamina = stamina;
            Strength = strength;
            Agility = agility;
            Intelligence = intelligence;
            StrengthXP = 0;
            AgilityXP = 0;
            IntelligenceXP = 0;
        }

        public bool Equals(Adventurer adv)
        {
            if (FirstName.Equals(adv.FirstName) && LastName.Equals(adv.LastName) &&
                Gender.Equals(adv.Gender) && Hp == adv.Hp && Stamina == adv.Stamina &&
                Strength == adv.Strength && Agility == adv.Agility && Intelligence == adv.Intelligence
                && StrengthXP == adv.StrengthXP && AgilityXP == adv.AgilityXP && IntelligenceXP == adv.IntelligenceXP)
                return true;
            else
                return false;
        }

        public bool Equals(string info)
        {
            if (info.Equals(getInfo()))
                return true;
            else
                return false;
        }

        public void CheckLevelUp()
        {
            if (StrengthXP == 100 && Strength < 10)
            {
                StrengthXP = 0;
                Strength++;
            }

            if (AgilityXP == 100 && Agility < 10)
            {
                AgilityXP = 0;
                Agility++;
            }

            if (IntelligenceXP == 100 && Intelligence < 10)
            {
                IntelligenceXP = 0;
                Intelligence++;
            }
        }

        public string getInfo()
        {
            string info;
            info = FirstName + LastName + '\n' + "HP:" + Hp.ToString() + " STA:" + Stamina.ToString() + "\n\n" + "STR:" + Strength.ToString() + " AGY:" + Agility.ToString() + " INT:" + Intelligence.ToString();
            return info;
        }



    }

    //TO DO: lucreaza la statusurile de care are nevoie
    public class Quest
    {
        public string Description { get; set; }
        public int Reward { get; set; }
        public int Difficulty { get; set; }
        public int RequiredSTA { get; set; }
        public int RequiredSTR { get; set; }
        public int RequiredAGY { get; set; }
        public int RequiredINT { get; set; }
        public int Slots { get; set; }

        public Quest(string dsc, int STA, int STR, int AGY, int INT, int rwd, int slots, int diff)
        {
            Description = string.Copy(dsc);
            Reward = rwd;
            RequiredSTA = STA;
            RequiredSTR = STR;
            RequiredAGY = AGY;
            RequiredINT = INT;
            Slots = slots;
            Difficulty = diff;
        }
    }

    public class Player
    {
        public string Username { get; set; }
        public int Gold { get; set; }
        public int Supply { get; set; }
        public int NumberOfRooms { get; set; }
        public List<Room> Rooms { get; set; } //Room Type and its level
        public int adventurersLimit { get; set; }
        public int passiveStrengthGain { get; set; }
        public int passiveAgilityGain { get; set; }
        public int passiveIntelligenceGain { get; set; }


    }

    public class Room
    {
        public Room(int t = 0, int l = 0)
        {
            Type = t;
            Level = l;
        }

        public int Type { get; set; }
        public int Level { get; set; }
    }


    public enum MouseModes
    {
        UI,
        WORLD
    };

    public struct VVC // Venture veil constants
    {
        public static int buildPrice = 100;
        public static int upgradeLevelPrice = 50;
        public static int upgradeBasePrice = 50;
    }
}
