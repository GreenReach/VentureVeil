using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class NewProfile : MonoBehaviour
{

    public Button confirmButton;
    public InputField input;

    private ProfilesAPI profilesAPI;
    private PlayerAPI playerAPI;
    private AdventurerAPI adventurerAPI;
    private FinnishedQuestAPI finnishedQuestAPI;
    private MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("MenuUI").GetComponent<MenuManager>();
        profilesAPI = GameObject.Find("MenuUI").GetComponent<ProfilesAPI>();
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();
        adventurerAPI = GameObject.Find("MenuUI").GetComponent<AdventurerAPI>();
        finnishedQuestAPI = GameObject.Find("MenuUI").GetComponent<FinnishedQuestAPI>();

        confirmButton.onClick.AddListener(CreateUser);
    }

    void CreateUser()
    {
        Profile newProfile = new Profile(input.text);
        if (profilesAPI.SaveProfile(newProfile))
        {
            //Create the necesary directors and files
            playerAPI.CreatePlayer(newProfile); //directory created here
            adventurerAPI.CreateAdventurersFile(newProfile);
            finnishedQuestAPI.CreateFinnishedQuestFile(newProfile);

            menuManager.ChangeScreen("ProfileCreated");
        }
    }
}
