using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class ExchangeFavors : MonoBehaviour
{
    public Button backButton;
    public Text courtierText, nobilityText, royalText;

    //Locations
    public Button mediumKingdomButton, hardKingdomButton;

    //Exchange Favors
    public Button buyRFavorsButton, buyNFavorsRButton, buyNFavorsCButton, buyCFavorsButton;


    private SoundManager soundManager;
    private MenuManager menuManager;
    private PlayerAPI playerAPI;
    private Player player;

    void Start()
    {
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();
        soundManager = GameObject.Find("MenuUI").GetComponent<SoundManager>();

        backButton.onClick.AddListener(Back);

        //Locations
        mediumKingdomButton.onClick.AddListener(delegate {Buy(0, VVC.mediumKingdomPrice, (int)UnlockedFeature.mediumKingdom); });

        hardKingdomButton.onClick.AddListener(delegate {Buy(0, VVC.hardKingdomPrice, (int)UnlockedFeature.hardKingdom); });

        //Favors Excahnge
        buyCFavorsButton.onClick.AddListener(ConvertToCourtierFavors);
        buyNFavorsCButton.onClick.AddListener(delegate { ConvertToNobilityFavors(0); });
        buyNFavorsRButton.onClick.AddListener(delegate { ConvertToNobilityFavors(1); });
        buyRFavorsButton.onClick.AddListener(ConvertToRoyalFavors);
    }


    public void Configure(Player p, MenuManager mm)
    {
        player = p;
        menuManager = mm;
        UpdateFavors();
        UpdateButtons();
    }

    void UpdateButtons()
    {
        //Location
        if (player.Unlocks[(int)UnlockedFeature.mediumKingdom] == true)
            mediumKingdomButton.interactable = false;
        if (player.Unlocks[(int)UnlockedFeature.hardKingdom] == true)
            hardKingdomButton.interactable = false;

        //Favors Excahnge
        if(player.royalFavors == 0)
            buyNFavorsRButton.interactable = false;
        else
            buyNFavorsRButton.interactable = true;

        if (player.nobilityFavors < 4)
            buyRFavorsButton.interactable = false;
        else
            buyRFavorsButton.interactable = true;

        if (player.nobilityFavors == 0)
            buyCFavorsButton.interactable = false;
        else
            buyCFavorsButton.interactable = true;

        if (player.courtierFavors < 10)
            buyNFavorsCButton.interactable = false;
        else
            buyNFavorsCButton.interactable = true;

    }

    void Buy(int favorType, int price, int unlockNumber )
    {
        soundManager.Play("button");

        if (favorType == 0 && player.royalFavors >= price)
        {
            player.royalFavors -= price;
            player.Unlocks[unlockNumber] = true;
            UpdateFavors();
            UpdateButtons();
        }
        else if(favorType == 1 && player.nobilityFavors >= price)
        {
            player.nobilityFavors -= price;
            player.Unlocks[unlockNumber] = true;
            UpdateFavors();
            UpdateButtons();
        }
    }

    void ConvertToCourtierFavors()
    {
        soundManager.Play("button");

        if (player.nobilityFavors >= 1)
        {
            player.nobilityFavors -= 1;
            player.courtierFavors += 5;
        }

        UpdateFavors();
        UpdateButtons();
    }

    void ConvertToNobilityFavors(int favorType)
    {
        soundManager.Play("button");

        if (favorType == 0 && player.courtierFavors >= 10)
        {
            player.courtierFavors -= 10;
            player.nobilityFavors += 1;
        }
        else if(favorType == 1 && player.nobilityFavors >= 1)
        {
            player.royalFavors -= 1;
            player.nobilityFavors += 2;
        }

        UpdateFavors();
        UpdateButtons();
    }

    void ConvertToRoyalFavors()
    {
        soundManager.Play("button");

        if (player.nobilityFavors >= 4)
        {
            player.nobilityFavors -= 4;
            player.royalFavors += 1;
        }

        UpdateFavors();
        UpdateButtons();
    }

    public void UpdateFavors()
    {
        courtierText.text = player.courtierFavors.ToString();
        nobilityText.text = player.nobilityFavors.ToString();
        royalText.text = player.royalFavors.ToString();
    }

    void Back()
    {
        soundManager.Play("button");
        playerAPI.UpdatePlayer(player);
        menuManager.ChangeScreen("ReturnFromExchangeFavors");
    }

}
