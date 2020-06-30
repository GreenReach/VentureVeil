using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class NewProfile : MonoBehaviour
{

    public Button confirmButton;
    public InputField input;
    public GameObject errorMessage;

    private ProfilesAPI profilesAPI;
    private PlayerAPI playerAPI;
    private AdventurerAPI adventurerAPI;
    private FinnishedQuestAPI finnishedQuestAPI;

    private MenuManager menuManager;

    private void Update()
    {
        if(input.isFocused && Input.anyKeyDown)
        {
            GameObject.Find("MenuUI").GetComponent<SoundManager>().Play("writing");
        }
    }

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
        GameObject.Find("MenuUI").GetComponent<SoundManager>().Play("button");
        Profile newProfile = new Profile(input.text);
        if (profilesAPI.SaveProfile(newProfile))
        {
            //Create the necesary directors and files
            playerAPI.CreatePlayer(newProfile); //directory created here
            adventurerAPI.CreateAdventurersFile(newProfile);
            finnishedQuestAPI.CreateFinnishedQuestFile(newProfile);

            menuManager.ChangeScreen("ProfileCreated");
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
}
