using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class AdventureScreen : MonoBehaviour
{
    //GamObjects to spawn and use
    public GameObject ChooseAdventurerPanel; // prefab - screen to choose adventures from
    public GameObject adventurerSlot; // prefab - button to chose adventure with
    public GameObject slotsList; // the object which holds all adventureSlot
    public List<GameObject> slots; // all AdventureSlots

    //UI 
    public Text adventureDescription;
    public Button startButton, lowSupply, mediumSupply, highSupply;

    //VVS (venture veil structures)
    private GetInstance getInstance;
    private Quest quest;
    private List<Adventurer> chosenAdventurers; // current party
    private List<Adventurer> remainingAdventurers;

    //MISC
    private GameObject currentSlot; // selected slot to be filled by an adventurer
    private Vector3 questPos; // quest location
    private int lowSupplyCost, mediumSupplyCost, highSupplyCost, supplyLevel, finalSupplyCost;
    private float distance; // distance to quest;

    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        chosenAdventurers = new List<Adventurer>();
        remainingAdventurers = new List<Adventurer>();
        slots = new List<GameObject>();
        distance = Vector3.Distance(getInstance.Guild.transform.position, questPos);

        remainingAdventurers = getInstance.AdventurerAPI.getAdventurers();
        startButton.onClick.AddListener(StartAdventure);

        lowSupply.onClick.AddListener(delegate { ChangeSupplyLevel(-1); });
        mediumSupply.onClick.AddListener(delegate { ChangeSupplyLevel(1); });
        highSupply.onClick.AddListener(delegate { ChangeSupplyLevel(2); });

        CalculateSupplyCost();
        supplyLevel = -1;
        finalSupplyCost = lowSupplyCost;
    }

    public void ConfigureScreen(Quest q, Vector3 questLocation)
    {
        quest = q;
        questPos = questLocation;

        adventureDescription.text = q.Description + "\n" +"STR:" + q.RequiredSTR + " AGY:" + q.RequiredAGY + " INT:" + q.RequiredINT + "STA: " + q.RequiredSTA + "\n Reward " + q.Reward;

        for (int i = 0; i < quest.Slots; i++)
        {
            slots.Add(Instantiate(adventurerSlot, slotsList.transform)); //Instantiate a new slot and add it to the slots list
            slots[slots.Count - 1].GetComponent<Button>().onClick.AddListener(SelectAdventurer);
        }
    }

    public void AddAdventurer(Adventurer adv)
    {
        if(adv == null) // empty adventurer, take one back from the adventure
        {
            string currentAdv = currentSlot.GetComponentInChildren<Text>().text;
            Adventurer a = chosenAdventurers.Find(x => x.Equals(currentAdv));
            remainingAdventurers.Add(a);
            chosenAdventurers.Remove(a);
            currentSlot.GetComponentInChildren<Text>().text = "Choose Adventurer";

        }
        else if (currentSlot.GetComponentInChildren<Text>().text == "Choose Adventurer")// chose an adventurer in an empty slot
        {
            print("simple");
            remainingAdventurers.Remove(adv);
            chosenAdventurers.Add(adv);

            currentSlot.GetComponentInChildren<Text>().text = adv.getInfo();
        }
        else// choose an adventurer in a already occupied slot
        {
            print("complex");
            string currentAdv = currentSlot.GetComponentInChildren<Text>().text;
            Adventurer a = chosenAdventurers.Find(x => x.Equals(currentAdv));
            remainingAdventurers.Add(a);
            remainingAdventurers.Remove(adv);
            chosenAdventurers.Remove(a);
            chosenAdventurers.Add(adv);

            currentSlot.GetComponentInChildren<Text>().text = adv.getInfo();
        }

        //After each  change redo the supply costs
        CalculateSupplyCost();

    }

    public List<Adventurer> GetAdventurers()
    {
        return remainingAdventurers;
    }

    void CalculateSupplyCost()
    {
        /**
         * Calculates the suply costs based on the distance, the party total strenght and the party total intelligence
        **/

        int baseSupplyCost = (int)distance * 2 + chosenAdventurers.Count;
        int partyStrenght = 0, partyIntelligence = 0;

        for(int i = 0;i< chosenAdventurers.Count; i++)
        {
            partyStrenght += chosenAdventurers[i].Strength;
            partyIntelligence += chosenAdventurers[i].Intelligence;
        }

        baseSupplyCost += partyStrenght - (partyIntelligence * 2);
        if( baseSupplyCost <= 0)
        {
            lowSupplyCost = 1;
            mediumSupplyCost = 1;
            highSupplyCost = 1;
        }
        else
        {
            lowSupplyCost = baseSupplyCost / 2;
            mediumSupplyCost = baseSupplyCost;
            highSupplyCost = baseSupplyCost + baseSupplyCost / 2;
        }

        lowSupply.GetComponentInChildren<Text>().text = "Low supply cost " + lowSupplyCost;
        mediumSupply.GetComponentInChildren<Text>().text = "Medium supply cost " + mediumSupplyCost;
        highSupply.GetComponentInChildren<Text>().text = "High supply cost " + highSupplyCost;


    }

    void ChangeSupplyLevel(int level)
    {
        supplyLevel = level;
        if (level == -1)
            finalSupplyCost = lowSupplyCost;
        else if (level == 1)
            finalSupplyCost = mediumSupplyCost;
        else
            finalSupplyCost = highSupplyCost;
    }

    void SelectAdventurer()
    {
        if (remainingAdventurers.Count > 0)
        {
            currentSlot = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject; // get the game object that was pressed
            GameObject panel = Instantiate(ChooseAdventurerPanel, new Vector3(0, 0, 0), Quaternion.identity);
            panel.GetComponent<ChooseAdventurerScreen>().SetScript(this); 
            panel.GetComponent<ChooseAdventurerScreen>().PopulateTab();
        }
    }

    void StartAdventure()
    {
        getInstance.GameManager.StartAdventure(chosenAdventurers, quest, questPos, supplyLevel, finalSupplyCost);

        getInstance.GameManager.ChangeMouseMode(MouseModes.WORLD);
        Destroy(gameObject);
    }

}
