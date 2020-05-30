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
    public Text welcomeText;
    public Text favorsText;

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
        changeProfile.onClick.AddListener(ChangeProfile);
        quit.onClick.AddListener(Quit);
        deleteProfile.onClick.AddListener(DeleteProfile);
    }

    public void Configure(Profile p)
    {
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();

        profile = p;
        playerAPI.LoadPlayer(profile.Username);
        Player = playerAPI.Player;

        welcomeText.text = "Welcome, " + profile.Username;
        favorsText.text = "CF: " + Player.courtierFavors + " NF: " + Player.nobilityFavors + " RF: " + Player.royalFavors;
    }

    void NewGame()
    {
        menuManager.ChangeScreen("StartNewGame");
    }

    void ResumeGame()
    {
        PlayerPrefs.SetString("Username", profile.Username);
        SceneManager.LoadScene("EasyKingdom");
    }

    void ChangeProfile()
    {
        menuManager.ChangeScreen("ReturnToProfile");
    }

    void Quit()
    {
        Application.Quit();
    }

    void DeleteProfile()
    {
        menuManager.ChangeScreen("DeleteProfile", profile);
    }
}
