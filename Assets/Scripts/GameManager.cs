using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;
public class GameManager : MonoBehaviour
{

    //Prefabs
    public GameObject flagStart;
    public GameObject flagFinnish;
    public GameObject flagQuest;
    public GameObject wanderingAdventurer;
    public GameObject questSign;
    public GameObject AdventureScreen;
    public GameObject AdventurerParty;
    public GameObject Builder;

    //UI Prefabs
    public Sprite iconMorning, iconMidday, iconDusk, iconNight;
    public GameObject RoomsScreen;

    //UI
    public GameObject adventurerListMenu;
    public GameObject adventurerFinderMenu;
    public Image timeOfDay; 

    public Button adventurerListMenuButton;
    public Button adventurerFinderMenuButton;
    public Text goldText;
    public Text supplyText;
    public Text dateText;
    public Text adventurerCount;

    //World Map
    public GameObject worldMap;

    //Guild
    public GameObject guild { get; set; }

    //Misc
    private bool flagToPlace;
    private GameObject currentFlagStart;
    private Vector3 startPoint;
    private int hoursPassed;
    private int daysPassed;
    private int supplyDailyCost;
    private MouseModes mouseMode; // "UI" if in UI, "WORLD" if in world;

    //API & Scripts & VVS
    private GetInstance getInstance;
    private AdventurerAPI adventurerAPI;
    private PlayerAPI playerAPI;
    private QuestAPI questAPI;
    private Player player;

    //GamoObject List
    private List<GameObject> gameObjects = new List<GameObject>();

    //Lists 
    private List<Adventurer> playerAdventurers;

    //Random chances
    private int wonderingAdventurerChance;


    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        getInstance.Load();
        LoadFromAPI();

        //calculate daily supply cost
        CalculateDailySupplyCost();

        UISetup();

        mouseMode = MouseModes.WORLD;

        wonderingAdventurerChance = 10;

        InvokeRepeating("CreateRandomObjects",0, 3);
        InvokeRepeating("UpdateHour", 0, 1);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && mouseMode == MouseModes.WORLD)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                print(hit.transform.name);
            }

            if (hit.transform != null && hit.transform.name == "Plane")
            {
                //  CreateQuest();
                //  CreateWonderingAdventurer(CreateRandomMapPoint());
                CreateBuilder(CreateRandomMapPoint());
            }
        }
    }

    void CalculateDailySupplyCost()
    {
        supplyDailyCost = 5; // base cost
        for(int i = 0;i<playerAdventurers.Count;i++)
            supplyDailyCost += (playerAdventurers[i].Strength + playerAdventurers[i].Agility + playerAdventurers[i].Intelligence) / 6;
    
    }
    void UpdateDailySupplyCost(Adventurer adv)
    {
        supplyDailyCost += (adv.Strength + adv.Agility + adv.Intelligence) / 6;
    }

    void CreateRandomObjects()
    {
        //Create wandering adventurer
        if (Random.Range(1, 100) < wonderingAdventurerChance)
        {
            CreateWonderingAdventurer(CreateRandomMapPoint());
            wonderingAdventurerChance = 10;
        }
        else
            wonderingAdventurerChance++;

            
    }
    void UpdateHour()
    {
        hoursPassed++;

        if (hoursPassed == 5)
            timeOfDay.sprite = iconMorning;
        else if (hoursPassed == 10)
            timeOfDay.sprite = iconMidday;
        else if (hoursPassed == 18)
            timeOfDay.sprite = iconDusk;
        else if (hoursPassed == 21)
            timeOfDay.sprite = iconNight;

        if(hoursPassed > 23)
        {
            hoursPassed = 0;
            UpdateDay();
        }

        RefreshUI();
    }
    void UpdateDay()
    {
        daysPassed++;
        player.Supply -= supplyDailyCost;

        //Adventurers get bonus XP
        PassiveXPGain();
    }

    void PassiveXPGain()
    {
        //At the end of each day the adventurers get bonus XP
        for(int i = 0; i<playerAdventurers.Count; i++)
        {
            playerAdventurers[i].StrengthXP += player.passiveStrengthGain;
            playerAdventurers[i].AgilityXP += player.passiveAgilityGain;
            playerAdventurers[i].IntelligenceXP += player.passiveIntelligenceGain;

            playerAdventurers[i].CheckLevelUp();
        }
    }
    void CreateWonderingAdventurer(Vector3 startPoint )
    {
        GameObject wanderingAdv = Instantiate(wanderingAdventurer, startPoint, Quaternion.identity);
        gameObjects.Add(wanderingAdv);
    }

    void CreateQuest(Vector3 startPoint)
    {
        GameObject flag = (Instantiate(questSign, startPoint, Quaternion.identity));
        gameObjects.Add(flag);
    }
    void CreateBuilder(Vector3 startPoint)
    {
        GameObject builder = (Instantiate(Builder, startPoint, Quaternion.identity));
        gameObjects.Add(builder);
    }

    Vector3 CreateRandomMapPoint()
    {
        Mesh worldMapMesh = worldMap.GetComponent<MeshFilter>().mesh;

        float x = Random.Range(worldMap.transform.position.x - worldMap.transform.localScale.x * worldMapMesh.bounds.size.x * 0.5f,
                              worldMap.transform.position.x + worldMap.transform.localScale.x * worldMapMesh.bounds.size.x * 0.5f);
        float z = Random.Range(worldMap.transform.position.z - worldMap.transform.localScale.z * worldMapMesh.bounds.size.z * 0.5f,
                               worldMap.transform.position.z + worldMap.transform.localScale.z * worldMapMesh.bounds.size.z * 0.5f);

        Ray ray = new Ray(new Vector3(x, 10, z), Vector3.down);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 1<<8); //layer 8

        return hit.point;
    }

    void UISetup()
    {
        adventurerListMenuButton.onClick.AddListener(AdventurerListMenu);
        adventurerFinderMenuButton.onClick.AddListener(AdventurerFinderMenu);

        hoursPassed = 0;
        daysPassed = 0;

        goldText.text = "Gold " + player.Gold.ToString();
        supplyText.text = "Supply " + player.Supply.ToString() + "(-" + supplyDailyCost + ")";
        dateText.text = "Day " + daysPassed + "  Hour " + hoursPassed;
        adventurerCount.text = "Adventurers " + playerAdventurers.Count + " / " + player.adventurersLimit;

    }
    void RefreshUI()
    {
        goldText.text = "Gold " + player.Gold.ToString();
        supplyText.text = "Supply " + player.Supply.ToString() + "(-" + supplyDailyCost + ")";
        dateText.text = "Day " + daysPassed + "  Hour " + hoursPassed;
        adventurerCount.text = "Adventurers " + playerAdventurers.Count + " / " + player.adventurersLimit;

    }
    void LoadFromAPI()
    {
        //Player
        playerAPI = getInstance.PlayerAPI;
        playerAPI.LoadPlayer();
        player = playerAPI.getPlayer();

        //Adventurer
        adventurerAPI = getInstance.AdventurerAPI;
        adventurerAPI.LoadAdventurers();
        playerAdventurers = adventurerAPI.getAdventurers();

        //Quest
        questAPI = getInstance.QuestAPI;

    }


    public void UpdateGuildRooms()
    {
        playerAPI.UpdateRooms();
        RefreshUI();
    }
    public void UpdateGuildHall()
    {
        playerAPI.UpdateHall();
        RefreshUI();
    }

    public void AdventurerListMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Instantiate(adventurerListMenu, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void AdventurerFinderMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
             Instantiate(adventurerFinderMenu, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    public bool BuyAdventurer(Adventurer adv, int price)
    {
        if (price > player.Gold || player.adventurersLimit == playerAdventurers.Count)
            return false;

        player.Gold -= price;
        playerAdventurers.Add(adv);
        UpdateDailySupplyCost(adv);
        RefreshUI();
        return true;
    }
    public bool BuySupplies(int supply, int price)
    {
        if(price <= player.Gold)
        {
            player.Gold -= price;
            player.Supply += supply;
            RefreshUI();
            return true;
        }
        return false;
    }
    public void BuyBuilder(GameObject builder)
    {

        GameObject roomUpgradeScreen =  Instantiate(RoomsScreen, getInstance.UICanvas.transform);
        roomUpgradeScreen.GetComponent<Rooms>().BuilderUpgrade(builder);
    }

    public bool ConfigureAdventure(Quest quest, Vector3 questPos)
    {
        GameObject adventure = Instantiate(AdventureScreen, new Vector3(0, 0, 0), Quaternion.identity);
        adventure.GetComponent<AdventureScreen>().ConfigureScreen(quest, questPos);
        return true;
    }

    public void StartAdventure(List<Adventurer> party, Quest quest, Vector3 questPos,int supplyLevel, int supplyCost)
    {
        if (player.Supply >= supplyCost)
        {
            player.Supply -= supplyCost;
            GameObject advParty = Instantiate(AdventurerParty, guild.transform.position, Quaternion.identity);
            advParty.GetComponent<AdventurerParty>().ConfigureParty(party, quest, questPos, guild.transform.position, supplyLevel);
            CalculateDailySupplyCost();
        }
        else
        {
            for (int i = 0; i < party.Count; i++)
                playerAdventurers.Add(party[i]);
            print("not enough supply");
        }
    }

    public void EndAdventure(List<Adventurer> returningParty, int reward, Quest quest)
    {
        if (reward == -1)
        {
            print("QUEST WAS FAILED");
            CalculateQuestXPReward(returningParty, quest, false);
        }
        else
        {
            player.Gold += reward;
            CalculateQuestXPReward(returningParty, quest, true);
        }


        for (int i = 0; i < returningParty.Count; i++)
        {
            returningParty[i].CheckLevelUp();
            playerAdventurers.Add(returningParty[i]);

        }
        CalculateDailySupplyCost();
        RefreshUI();
    }

    private void CalculateQuestXPReward(List<Adventurer> advs, Quest quest, bool isSuccsessfull)
    {
        //TO DO: make it scale down with level.
        /**
         * Each member of the party receives the same amount of XP 
         **/
        int strengthXPReward = quest.RequiredSTR / advs.Count;
        int agilityXPReward = quest.RequiredAGY / advs.Count;
        int intelligenceXPReward = quest.RequiredINT / advs.Count;

        if (isSuccsessfull)
        {
            for (int i = 0; i < advs.Count; i++)
            {
                advs[i].StrengthXP += strengthXPReward;
                advs[i].AgilityXP += agilityXPReward;
                advs[i].IntelligenceXP += intelligenceXPReward;
            }
        }
        else
        {
            for (int i = 0; i < advs.Count; i++)
            {
                advs[i].StrengthXP += strengthXPReward/10;
                advs[i].AgilityXP += agilityXPReward/10;
                advs[i].IntelligenceXP += intelligenceXPReward/10;
            }
        }
    }


    public void ChangeMouseMode(MouseModes mode)
    {
        mouseMode = mode;
    }

    public bool CheckGold(int amount)
    {
        if (player.Gold >= amount)
            return true;
        return false;
    }

    public bool PayGold(int amount)
    {
        if (player.Gold >= amount)
        {
            player.Gold -= amount;
            RefreshUI();
            return true;
        }
        return false;
    }
}
