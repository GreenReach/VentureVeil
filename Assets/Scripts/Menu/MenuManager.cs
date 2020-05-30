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

    //API
    private PlayerAPI playerAPI;
    private ProfilesAPI profilesAPI;


    // Start is called before the first frame update
    void Start()
    {
        playerAPI = GetComponent<PlayerAPI>();
        profilesAPI = GetComponent<ProfilesAPI>();

        LoadFromAPI();

        //After it is loaded start the first screen
       profileSelect.SetActive(true);

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
    }



}
