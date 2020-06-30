using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;

public class AdventurerParty : MonoBehaviour
{
    //Quest&Party info
    private Quest quest;
    private Vector3 questPos;
    private Vector3 guildPos;
    private List<Adventurer> party;
    private int supplyLevel;

    //API
    private GetInstance getInstance;

    //Quest state
    private int state; // state of quest, 0 - is not ready to start, 1 - going to questPos, 2 - doing quest, 3 - returning to guild , 4 - reached guild

    //MISC
    private bool isSuccessful;
    private float timeToCompleteQuest;
    private float partyAgility, partyStrenght, partyIntelligence;
    private Vector3 speed,distance;
    private float timeToDestination, statsSpeedFactor;
    private bool configured = false;
    public GameObject PartySize1, PartySize2, PartySize3, PartySize4;

    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        if (!configured)
            state = 0;
    }

    private void Update()
    {
        
        switch (state) {
            case 0: break;
            case 1: GoToQuest(); break;
            case 2: DoQuest(); break;
            case 3: ReturnFromQuest();  break;
            case 4: CompleteQuest();  break;
        }

    }
    public void ConfigureParty(List<Adventurer> advs, Quest q, Vector3 Qpos, Vector3 Gpos, int supLevel )
    {
        //Initialize data
        party = advs;
        quest = q;
        questPos = Qpos;
        guildPos = Gpos;
        state = 1;
        isSuccessful = false;
        configured = true;
        supplyLevel = supLevel;

        //Calculate GoToQuest variables
        CalculateQuestRoadVariables();
        state = 1;
        isSuccessful = false;
        timeToCompleteQuest = (quest.RequiredAGY + quest.RequiredSTR + quest.RequiredINT) / 3;
        configured = true;

        //Show the  correct gameobject
        if (party.Count == 1)
            PartySize1.SetActive(true);
        else if (party.Count == 2)
            PartySize2.SetActive(true);
        else if (party.Count == 3)
            PartySize3.SetActive(true);
        else PartySize4.SetActive(true);
    }

    private void CalculateQuestRoadVariables()
    {
        //calculates speed
        partyAgility = 0;
        partyStrenght = 0;
        for (int i = 0; i < party.Count; i++)
        {
            partyAgility += party[i].Agility;
            partyStrenght += party[i].Strength;
            partyIntelligence += party[i].Intelligence;
        }

        statsSpeedFactor = (partyAgility / party.Count - partyStrenght / party.Count + 10.0f) / 8.0f ;
        statsSpeedFactor += statsSpeedFactor * (supplyLevel/10.0f); // add the supply level modifier( -10% for low, +10% for medium, +20% for high)

        distance = questPos - transform.position;
        speed = statsSpeedFactor * distance.normalized; //calculate the speed based on statsSpeedFactor
        timeToDestination = distance.magnitude / speed.magnitude; // calculate time
        print(speed.magnitude);
    }
    private void CalculateQuestVariables()
    {
        /**
         * Calculates the sccess probability
         * If an attribute is not high enough then for each lacking point 10% is being deducted from the probability and +1 sec is added to the time
         * If an attribute is gihher then the requirement then for each bonus point 1% is being added to the probability and half a second is subtracted from the time
        **/

        int success = 100;
        float actualStrenght = partyStrenght - quest.RequiredSTR;
        float actualAgility = partyAgility - quest.RequiredAGY;
        float actualIntelligence = partyIntelligence - quest.RequiredINT;

        if (actualStrenght < 0)
        {
            success -= (int)actualStrenght * 10;
            timeToCompleteQuest += actualStrenght;
        }
        else
        {
            success += (int)actualStrenght;
            timeToCompleteQuest -= actualStrenght / 2;
        }

        if (actualAgility < 0)
        {
            success -= (int)actualAgility * 10;
            timeToCompleteQuest += actualAgility;

        }
        else
        {
            success += (int)actualAgility;
            timeToCompleteQuest -= actualAgility / 2;
        }

        if (actualIntelligence < 0)
        {
            success -= (int)actualIntelligence * 10;
            timeToCompleteQuest += actualIntelligence;
        }
        else
        {
            success += (int)actualIntelligence;
            timeToCompleteQuest -= actualIntelligence / 2;
        }

        success += supplyLevel * 10; // apply the supply level modifier

        if (success >= Random.Range(1, 101))
            isSuccessful = true;
    }
    private void CalculateReturnRoadVariables()
    {
        //calculates speed
        distance = guildPos - transform.position;
        speed = statsSpeedFactor * distance.normalized; //calculate the speed based on statsSpeedFactor
        timeToDestination = distance.magnitude / speed.magnitude; // calculate time
    }

    private void GoToQuest()
    {
        if (timeToDestination >= 0)
        {
            transform.position += speed * Time.deltaTime;
            timeToDestination -= Time.deltaTime;
        }
        else
        {
            CalculateQuestVariables();
            state = 2;
        }

    }
    private void DoQuest()
    {
        //Get quest results and wait until done
        if (timeToCompleteQuest >= 0)
        {
            timeToCompleteQuest -= Time.deltaTime;
        }
        else
        {
            //Calculate the ReturnFromQuest variables
            CalculateReturnRoadVariables();
            state = 3;
        }
    }
    private void ReturnFromQuest()
    {
        if (timeToDestination >= 0)
        {
            transform.position += speed * Time.deltaTime;
            timeToDestination -= Time.deltaTime;
        }
        else
        {
            state = 4;
        }
    }
    private void CompleteQuest()
    {
        //Show quest result
        print("QUEST HAS BEEN COMPLETED");
        if(isSuccessful)
            getInstance.GameManager.EndAdventure(party, quest.Reward, quest);
        else
            getInstance.GameManager.EndAdventure(party, -1, quest );

        Destroy(gameObject);
    }
}
