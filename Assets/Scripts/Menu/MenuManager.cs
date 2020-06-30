using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;

public class MenuManager : MonoBehaviour
{
    //Menu screens
    public GameObject mainMenu;
    public GameObject profileSelect;
    public GameObject NewProfile;
    public GameObject newGame;
    public GameObject exchangeFavors;

    //API
    private PlayerAPI playerAPI;
    private ProfilesAPI profilesAPI;


    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(false);
        profileSelect.SetActive(false);
        NewProfile.SetActive(false);
        newGame.SetActive(false);
        exchangeFavors.SetActive(false);

        playerAPI = GetComponent<PlayerAPI>();
        profilesAPI = GetComponent<ProfilesAPI>();

        LoadFromAPI();

        //After it is loaded start the first screen
        if (PlayerPrefs.HasKey("ReturningFromGame") && PlayerPrefs.GetInt("ReturningFromGame") == 1)
        {
            string user = PlayerPrefs.GetString("Username");
            List<Profile> profiles = profilesAPI.Profiles;
            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i].Username.Equals(user))
                {
                    ChangeScreen("ProfileSelected", profiles[i]);
                    PlayerPrefs.SetInt("ReturningFromGame",0);
                }
            }
        }
        else
        {
            profileSelect.SetActive(true);
        }
    }

    void LoadFromAPI()
    {
        profilesAPI.LoadProfiles();
    }

    public void ChangeScreen(string action, Profile user = null)
    {
        if(action.Equals("ProfileCreated"))
        {
            NewProfile.SetActive(false);
            profileSelect.SetActive(true);
        }
        else if( action.Equals("NewProfile"))
        {
            profileSelect.SetActive(false);
            NewProfile.SetActive(true);
        }
        else if( action.Equals("ProfileSelected"))
        {
            profileSelect.SetActive(false);
            mainMenu.SetActive(true);
            mainMenu.GetComponent<MainMenu>().Configure(user);
        }
        else if (action.Equals("ReturnToProfile"))
        {
            mainMenu.SetActive(false);
            profileSelect.SetActive(true);
        }
        else if(action.Equals("DeleteProfile"))
        {
            profilesAPI.DeleteProfile(user);
            mainMenu.SetActive(false);
            profileSelect.SetActive(true);
        }
        else if(action.Equals("StartNewGame"))
        {
            Player player = mainMenu.GetComponent<MainMenu>().Player;

            mainMenu.SetActive(false);
            newGame.SetActive(true);

            newGame.GetComponent<NewGame>().Configure(player,this);
        }
        else if(action.Equals("ReturnFromNewGame"))
        {
            newGame.SetActive(false);
            mainMenu.SetActive(true);
        }
        else if(action.Equals("GoToExchangeFavors"))
        {
            Player player = mainMenu.GetComponent<MainMenu>().Player;

            mainMenu.SetActive(false);
            exchangeFavors.SetActive(true);

            exchangeFavors.GetComponent<ExchangeFavors>().Configure(player, this);
        }
        else if(action.Equals("ReturnFromExchangeFavors"))
        {
            exchangeFavors.SetActive(false);
            mainMenu.SetActive(true);
        }
    }



}
