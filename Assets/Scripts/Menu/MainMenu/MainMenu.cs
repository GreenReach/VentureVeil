using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VentureVeilStructures;

public class MainMenu : MonoBehaviour
{
    public MenuManager menuManager;

    //UI
    public Button newGame;
    public Button resumeGame;
    public Button changeProfile;
    public Button quit;
    public Button deleteProfile;
    public Button exchangeFavors;
    public Text welcomeText;
    public Text courtierText, nobilityText, royalText;

    //VVS
    private Profile profile;
    public Player Player { get; private set; }

    //API
    PlayerAPI playerAPI;

    private void Start()
    {

        //Configure buttons
        newGame.onClick.AddListener(NewGame);
        resumeGame.onClick.AddListener(ResumeGame);
        exchangeFavors.onClick.AddListener(ExchangeFavors);
        changeProfile.onClick.AddListener(ChangeProfile);
        quit.onClick.AddListener(Quit);
        deleteProfile.onClick.AddListener(DeleteProfile);
    }

    private void OnEnable()
    {
        if (Player != null)
        {
            courtierText.text = Player.courtierFavors.ToString();
            nobilityText.text = Player.nobilityFavors.ToString();
            royalText.text = Player.royalFavors.ToString();
        }
    }

    public void Configure(Profile p)
    {
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();

        profile = p;
        playerAPI.LoadPlayer(profile.Username);
        Player = playerAPI.Player;

        welcomeText.text = "Welcome,\n   " + profile.Username;
        courtierText.text = Player.courtierFavors.ToString();
        nobilityText.text = Player.nobilityFavors.ToString();
        royalText.text = Player.royalFavors.ToString();

        //Check if there is a game to resume to ( kingdom != 0)

        if (Player.kingdom == 0)
            resumeGame.interactable = false;
    }

    void NewGame()
    {
        PlayButtonSound();
        PlayerPrefs.SetInt("Resume", 0);
        menuManager.ChangeScreen("StartNewGame");
    }

    void ResumeGame()
    {
        PlayButtonSound();
        PlayerPrefs.SetInt("Resume", 1);
        PlayerPrefs.SetString("Username", profile.Username);

        if(Player.kingdom == 1)
            SceneManager.LoadScene("EasyKingdom");
        else if(Player.kingdom == 2)
            SceneManager.LoadScene("MediumKingdom");
        else if (Player.kingdom == 3)
            SceneManager.LoadScene("HardKingdom");

    }

    void ExchangeFavors()
    {
        PlayButtonSound();
        menuManager.ChangeScreen("GoToExchangeFavors");
    }

    void ChangeProfile()
    {
        PlayButtonSound();
        menuManager.ChangeScreen("ReturnToProfile");
    }

    void Quit()
    {
        PlayButtonSound();
        Application.Quit();
    }

    void DeleteProfile()
    {
        PlayButtonSound();
        menuManager.ChangeScreen("DeleteProfile", profile);
    }

    void PlayButtonSound()
    {
        GameObject.Find("MenuUI").GetComponent<SoundManager>().Play("button");
    }
}
