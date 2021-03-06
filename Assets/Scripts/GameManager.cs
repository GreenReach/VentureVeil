﻿using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject RoomsScreen, pauseMenu;

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
    public Text courtierText, nobilityText, royalText;

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
    private int gameDuration;
    private bool isPaused;

    //API & Scripts & VVS
    private GetInstance getInstance;
    private AdventurerAPI adventurerAPI;
    private PlayerAPI playerAPI;
    private QuestAPI questAPI;
    private Player player;
    private FinnishedQuestAPI finnishedQuestAPI;

    //GamoObject List
    private List<GameObject> gameObjects = new List<GameObject>();

    //Lists 
    private List<Adventurer> playerAdventurers;
    private List<FinnishedQuest> finnishedQuests;

    //Random chances
    private int wonderingAdventurerChance;


    private void Start()
    {
        //default configuration
        Time.timeScale = 1;

        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        getInstance.Load();
        LoadFromAPI();

        //calculate daily supply cost
        CalculateDailySupplyCost();
        DurationSetup();
        UISetup();

        mouseMode = MouseModes.WORLD;
        isPaused = false;

        //Misc
        if(PlayerPrefs.GetInt("Resume") == 1)
        {
            getInstance.Guild.transform.position = player.guildLocation;
            daysPassed = player.currentDay;
            hoursPassed = player.currentHour;
            gameDuration = player.currentGameDuration;
        }
        else
        {
            player.guildLocation = getInstance.Guild.transform.position;
        }

        player.kingdom = PlayerPrefs.GetInt("Kingdom");

        wonderingAdventurerChance = 10;

        InvokeRepeating("CreateRandomObjects", 0, VVC.spawnObjectsInterval);
        InvokeRepeating("UpdateHour", 0, VVC.hourDuration);
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
                  CreateQuest(CreateRandomMapPoint());
                  CreateWonderingAdventurer(CreateRandomMapPoint());
                 CreateBuilder(CreateRandomMapPoint());
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
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

        //Once every 6 hours the adventurers gain 1 stamina
        if (hoursPassed % 6 == 0)
            RestAdventurers();

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

        if(daysPassed == gameDuration)
        {
            playerAPI.UpdatePlayer(player);
            PlayerPrefs.SetInt("ReturningFromGame", 1);
            PlayerPrefs.SetString("Username", player.Username);
            SceneManager.LoadScene("Menu");
        }
        player.Supply -= supplyDailyCost;

        //Adventurers get bonus XP
        PassiveXPGain();
    }
    void DurationSetup()
    {
        int duration = PlayerPrefs.GetInt("GameDuration");
        if (duration == 0)
            gameDuration = 100;
        else if (duration == 1)
            gameDuration = 200;
        else if (duration == 2)
            gameDuration = 365;
        else if (duration == 3)
            gameDuration = 100000000;
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
    void RestAdventurers()
    {
        for (int i = 0; i < playerAdventurers.Count; i++)
            playerAdventurers[i].CurrentStamina = Mathf.Min(playerAdventurers[i].Stamina, playerAdventurers[i].CurrentStamina + 1);
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
        adventurerCount.text = playerAdventurers.Count + " / " + player.adventurersLimit;
        courtierText.text = player.courtierFavors.ToString();
        nobilityText.text = player.nobilityFavors.ToString();
        royalText.text = player.royalFavors.ToString();
    }
    void RefreshUI()
    {
        goldText.text = "Gold " + player.Gold.ToString();
        supplyText.text = "Supply " + player.Supply.ToString() + "(-" + supplyDailyCost + ")";
        dateText.text = "Day " + daysPassed + "  Hour " + hoursPassed;
        adventurerCount.text = player.NumberOfAdventurers + " / " + player.adventurersLimit;
        courtierText.text = player.courtierFavors.ToString();
        nobilityText.text = player.nobilityFavors.ToString();
        royalText.text = player.royalFavors.ToString();
    }
    public void PauseGame()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    void LoadFromAPI()
    {
        string username = PlayerPrefs.GetString("Username");

        //Player
        playerAPI = getInstance.PlayerAPI;
        playerAPI.LoadPlayer(username);
        player = playerAPI.getPlayer();

        //Adventurer
        adventurerAPI = getInstance.AdventurerAPI;
        adventurerAPI.LoadAdventurers(username);
        playerAdventurers = adventurerAPI.getAdventurers();
        
        //Quest
        questAPI = getInstance.QuestAPI;

        //Finnished Quest
        finnishedQuestAPI = getInstance.FinnishedQuestAPI;
        finnishedQuestAPI.LoadFinnishedQuests(username);
        finnishedQuests = finnishedQuestAPI.FinnishedQuests;
    }

    public void SaveGame()
    {
        adventurerAPI.SaveAdventurers(player.Username);
        playerAPI.UpdatePlayer(player);
        finnishedQuestAPI.SaveFinnishedQuest(player.Username);
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
        if (price > player.Gold || player.adventurersLimit == player.NumberOfAdventurers)
            return false;

        player.Gold -= price;
        player.NumberOfAdventurers++;
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
    public bool BuyFavor(string action)
    {
        bool result = false;
        if (action.Equals("courtier") && player.Gold >= VVC.courtierFavorBasePrice)
        {
            player.Gold -= VVC.courtierFavorBasePrice;
            player.courtierFavors++;
            result = true;
        }
        else if (action.Equals("nobility") && player.Gold >= VVC.nobilityFavorBasePrice)
        {
            player.Gold -= VVC.nobilityFavorBasePrice;
            player.nobilityFavors++;
            result = true;
        }
        else if (action.Equals("royal") && player.Gold >= VVC.royalFavorBasePrice)
        {
            player.Gold -= VVC.royalFavorBasePrice;
            player.royalFavors++;
            result = true;
        }

        RefreshUI();
        return result;
    }

    public bool TellTales()
    {
        //calculate the total rating of the stories
        int totalRating = 0;
        int royal = VVC.royalTalesRating;
        int nobility = VVC.nobilityTalesRating;
        int courtier = VVC.courtierTalesRating; 

        for(int  i= 0; i < finnishedQuests.Count; i++)
        {
            if(!finnishedQuests[i].TaleTold)
            {
                totalRating += finnishedQuests[i].Rating;
                finnishedQuests[i].TaleTold = true;
            }
        }

        while(totalRating > 0)
        {
            //calculate the favors gained after telling this story
            if (totalRating >= royal && Random.Range(1, 5) >= 4) // 25% change to gain a royal favor when having a rating big enough
            {
                player.royalFavors++;
                totalRating -= royal;
            }
            if (totalRating >= nobility && Random.Range(1, 5) >= 3) // 50% change to gain a nobility favor when having a rating big enough
            {
                player.nobilityFavors++;
                totalRating -= nobility;
            }
            if (totalRating >= royal && Random.Range(1, 5) >= 2) // 75% change to gain a courtier favor when having a rating big enough
            {
                player.courtierFavors++;
                totalRating -= courtier;
            }

            totalRating--;
        }

        RefreshUI();
        return true;
    }

    public void ConfigureAdventure(Quest quest, Vector3 questPos, GameObject questSign)
    {
        GameObject adventure = Instantiate(AdventureScreen, new Vector3(0, 0, 0), Quaternion.identity);
        adventure.GetComponent<AdventureScreen>().ConfigureScreen(quest, questPos, questSign);
    }
    public void StartAdventure(List<Adventurer> party, Quest quest, Vector3 questPos,int supplyLevel, int supplyCost)
    {
        if (player.Supply >= supplyCost)
        {
            player.Supply -= supplyCost;
            for (int i = 0; i < party.Count; i++)
                party[i].CurrentStamina -= quest.RequiredSTA;

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

            finnishedQuests.Add(new FinnishedQuest(quest, returningParty, false));
        }
        else
        {
            player.Gold += reward;
            CalculateQuestXPReward(returningParty, quest, true);

            finnishedQuests.Add(new FinnishedQuest(quest, returningParty, true));
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
