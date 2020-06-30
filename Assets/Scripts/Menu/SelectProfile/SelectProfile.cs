using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class SelectProfile : MonoBehaviour
{
    //prefabs
    public GameObject profileButton;

    //manager
    MenuManager menuManager;

    //UI
    public GameObject profilesListContent;
    public Button addProfile;

    //API
    private ProfilesAPI profilesAPI;

    //VVS
    private List<Profile> profiles;

    //profile buttons
    private List<GameObject> profileObjects;

    private bool isConfigured = false;

    private void OnEnable()
    {
        if(!isConfigured)
        {
            Configure();
            isConfigured = true;
        }

        for (int i = 0; i < profileObjects.Count; i++)
            Destroy(profileObjects[i]);
        profileObjects = new List<GameObject>();

        profilesAPI.LoadProfiles();
        profiles = profilesAPI.Profiles;

        PopulateList();
    }

    void Configure()
    {
        menuManager = GameObject.Find("MenuUI").GetComponent<MenuManager>();
        profilesAPI = GameObject.Find("MenuUI").GetComponent<ProfilesAPI>();
        profilesAPI.LoadProfiles();
        profiles = profilesAPI.Profiles;
        profileObjects = new List<GameObject>();

        addProfile.onClick.AddListener(NewProfile);
    }

    void PopulateList()
    {
        for (int i = 0; i < profiles.Count; i++)
        {
            GameObject p = Instantiate(profileButton, profilesListContent.transform);
            p.GetComponent<ProfileButton>().Configure(profiles[i], this);
            profileObjects.Add(p);
        }
    }

    void NewProfile()
    {
        GameObject.Find("MenuUI").GetComponent<SoundManager>().Play("button");
        menuManager.ChangeScreen("NewProfile");
    }

    public void ProfileSelect(Profile p)
    {
        menuManager.ChangeScreen("ProfileSelected", p);
    }


}
