using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VentureVeilStructures;

public class NewGame : MonoBehaviour
{
    public Button startGameButton, returnButton;
    public Text favorsCostText, playerFavorsText;
    private int totalCost;
    private MenuManager menuManager;

    //Duration
    public Button days100Button, days200Button, year1Button, unlimitedButton;
    private List<Button> durationButtons;
    private int currentSelectedDuration, currentDurationCost, finalDuration;// final duration -> 0 = 100D(default), 1 = 200D, 3 = 1Y, 4 = unlimited

    //Location
    public Button easyKButton, mediumKButton, hardKButton;
    private List<Button> locationButtons;
    private int currentSelectedLocation, finalLocation;// finalLocation 0 - easy, 1 - medium, 2 - hard

    //Starting gold
    public Text goldText;
    public Button addGold, subtractGold;
    private int finalStartingGold; // finalStartingGold = 100(default)

    //API
    PlayerAPI playerAPI;

    //VVS
    Player player;


    private void Start()
    {
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();

        startGameButton.onClick.AddListener(StartGame);
        returnButton.onClick.AddListener(ReturnToMainMenu);

        //Duration
        durationButtons = new List<Button> { days100Button, days200Button, year1Button, unlimitedButton };

        days100Button.onClick.AddListener(delegate { ChangeDuration(0); });
        days200Button.onClick.AddListener(delegate { ChangeDuration(1); });
        year1Button.onClick.AddListener(delegate { ChangeDuration(2); });
        unlimitedButton.onClick.AddListener(delegate { ChangeDuration(3); });

        //Starting gold
        addGold.onClick.AddListener(delegate { ChangeGold(0); });
        subtractGold.onClick.AddListener(delegate { ChangeGold(1); });

        //Location
        locationButtons = new List<Button> { easyKButton, mediumKButton, hardKButton};

        easyKButton.onClick.AddListener(delegate { ChangeLocation(0); });
        mediumKButton.onClick.AddListener(delegate { ChangeLocation(1); });
        hardKButton.onClick.AddListener(delegate { ChangeLocation(2); });

    }

    private void OnEnable()
    {
        totalCost = 0;

        //Duration
        finalDuration = 0;
        currentDurationCost = 0;
        currentSelectedDuration = 0;
        days100Button.interactable = false;

        //Gold
        finalStartingGold = 100;
        goldText.text = "100";

        //Location
        finalLocation = 0;
        easyKButton.interactable = false;
    }

    public void Configure(Player p, MenuManager mm)
    {
        menuManager = mm;
        player = p;
        playerFavorsText.text = "CF: " + player.courtierFavors + " NF: " + player.nobilityFavors + " RF: " + player.royalFavors;

        UpdateCost();

    }

    void ChangeDuration(int buttonNumber)
    {
        durationButtons[currentSelectedDuration].interactable = true;
        durationButtons[buttonNumber].interactable = false;
        currentSelectedDuration = buttonNumber;

        totalCost -= currentDurationCost;
        currentDurationCost = VVC.durationPrices[buttonNumber];
        totalCost += currentDurationCost;

        finalDuration = buttonNumber;
        UpdateCost();
    }
    void ChangeGold(int buttonNumber)
    {
        if(buttonNumber == 1 && finalStartingGold > 100)
        {
            finalStartingGold -= VVC.goldIncrement;
            totalCost -= VVC.goldIncrementCost;
        }
        else if( buttonNumber == 0)
        {
            finalStartingGold += VVC.goldIncrement;
            totalCost += VVC.goldIncrementCost;
        }

        goldText.text = finalStartingGold.ToString();
        UpdateCost();
    }
    void ChangeLocation(int buttonNumber)
    {
        locationButtons[currentSelectedLocation].interactable = true;
        locationButtons[buttonNumber].interactable = false;
        currentSelectedLocation = buttonNumber;

        finalLocation = buttonNumber;
    }

    void StartGame()
    {
       if(player.courtierFavors >= totalCost)
        {
            player.courtierFavors -= totalCost;
            player.Gold = finalStartingGold;
            playerFavorsText.text = "CF: " + player.courtierFavors + " NF: " + player.nobilityFavors + " RF: " + player.royalFavors;
            playerAPI.UpdatePlayer(player);
            //PlayerPrefs.SetString("Username", player.Username);
            //SceneManager.LoadScene("EasyKingdom");
        }
    }
    void ReturnToMainMenu()
    {
        menuManager.ChangeScreen("ReturnFromNewGame");
    }

    void UpdateCost()
    {
        favorsCostText.text = "Price: " + totalCost + " Courtier Favors";
    }




}
