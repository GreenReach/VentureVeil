﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using VentureVeilStructures;

public class AdventurerAPI : MonoBehaviour
{
    private List<Adventurer> adventurers;
    private int[] attrProbabilities = { 20, 30, 30, 15, 5 };
    private int[] healthProbabilities = { 0, 0, 0, 40, 30, 20, 10 };
    private int[] staminaProbabilities = { 0, 0, 40, 30, 20, 10 };
    public List<Adventurer> getAdventurers() { return adventurers; }

    public void LoadAdventurers()
    {
        LoadFromXML();
    }

    private void LoadFromXML()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/Adventurers.xml");
        XmlNodeList nodeList = doc.SelectNodes("/Adventurers/Adventurer");
        adventurers = new List<Adventurer>();


        //citire
        foreach (XmlNode node in nodeList)
        {
            Adventurer adv = new Adventurer();

            adv.FirstName = node.SelectSingleNode("FIRSTNAME").InnerText;
            adv.LastName = node.SelectSingleNode("LASTNAME").InnerText;
            adv.Gender = node.SelectSingleNode("GENDER").InnerText;
            adv.Hp = int.Parse(node.SelectSingleNode("HP").InnerText);
            adv.Stamina = int.Parse(node.SelectSingleNode("STAMINA").InnerText);
            adv.Strength = int.Parse(node.SelectSingleNode("STRENGTH").InnerText);
            adv.Agility = int.Parse(node.SelectSingleNode("AGILITY").InnerText);
            adv.Intelligence = int.Parse(node.SelectSingleNode("INTELLIGENCE").InnerText);
            adv.StrengthXP = int.Parse(node.SelectSingleNode("STRENGTHXP").InnerText);
            adv.AgilityXP = int.Parse(node.SelectSingleNode("AGILITYXP").InnerText);
            adv.IntelligenceXP = int.Parse(node.SelectSingleNode("INTELLIGENCEXP").InnerText);
            adventurers.Add(adv);

        }
    }

    private int CalculateStat(int[] probabilities)
    {
        int stat = Random.Range(1, 101);
        int currentProb = 0;
        for(int i = 0;i<probabilities.Length;i++)
        {
            currentProb += probabilities[i];
            if (stat <= currentProb)
                return i + 1;
        }
        return probabilities.Length + 1;
        
    }


    public Adventurer CreateAdventurer()
    {
        int hp = CalculateStat(healthProbabilities) * Random.Range(5,10);
        int stamina = CalculateStat(staminaProbabilities);
        int strength = CalculateStat(attrProbabilities);
        int agility = CalculateStat(attrProbabilities);
        int intelligence = CalculateStat(attrProbabilities);

        string gender, firstName, lastName;
        if (Random.Range(1, 3) == 1)
            gender = "Female";
        else
            gender = "Male";

        //Extrag un nume si prenume random din AdventurerData
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/Data/AdventurerData.xml");

        int count;
        if (gender.Equals("Male"))
        {
            count = doc.SelectNodes("/Names/FirstName/MaleFirstName/Surname").Count;
            firstName = doc.SelectSingleNode("/Names/FirstName/MaleFirstName/Surname[position() = " + (Random.Range(1,count)).ToString() + "]").InnerText;
        }
        else
        {
            count = doc.SelectNodes("/Names/FirstName/FemaleFirstName/Surname").Count;
            firstName = doc.SelectSingleNode("/Names/FirstName/FemaleFirstName/Surname[position() = " + (Random.Range(1, count)).ToString() + "]").InnerText;
        }

        count = doc.SelectNodes("/Names/LastName/Name").Count;
        lastName = doc.SelectSingleNode("/Names/LastName/Name[position() = " + Random.Range(1, count).ToString() + "]").InnerText;

        return new Adventurer(firstName,lastName,gender,hp,stamina,strength,agility,intelligence);
    }

    public int CalculatePrice(Adventurer adv)
    {
        return (int)(adv.Hp * 0.5f + adv.Strength * 10 + adv.Agility * 10 + adv.Intelligence * 10);
    }

}
