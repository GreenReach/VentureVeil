using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;
public class ProfileButton : MonoBehaviour
{
    public Text text;
    public Button button;

    private Profile profile;
    private SelectProfile selectProfile;

    public void Configure(Profile p, SelectProfile sp)
    {
        profile = p;
        selectProfile = sp;

        text.text = profile.Username;
        button.onClick.AddListener(delegate {
            GameObject.Find("MenuUI").GetComponent<SoundManager>().Play("button");
            selectProfile.ProfileSelect(profile);
        });

    }
}
