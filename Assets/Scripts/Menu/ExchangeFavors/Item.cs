using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class Item : MonoBehaviour
{
    public Button buyButton;
    public int price;
    public int unlockedIndex;

    private SoundManager soundManager;
    private PlayerAPI playerAPI;
    private Player player;
    private bool isConfigured = false;

    private void Configure()
    {
        playerAPI = GameObject.Find("MenuUI").GetComponent<PlayerAPI>();
        soundManager = GameObject.Find("MenuUI").GetComponent<SoundManager>();

        buyButton.onClick.AddListener(Buy);
    }

    private void OnEnable()
    {
        if (!isConfigured)
        {
            Configure();
            isConfigured = true;
        }

        player = playerAPI.Player;

        if (player.Unlocks[unlockedIndex] == true)
            buyButton.interactable = false;
    }

    private void Buy()
    {
        soundManager.Play("button");

        if (player.nobilityFavors >= price)
        {
            player.nobilityFavors -= price;
            player.Unlocks[unlockedIndex] = true;
            buyButton.interactable = false;
            GameObject.Find("ExchangeFavors").GetComponent<ExchangeFavors>().UpdateFavors();
        }
    }
}
