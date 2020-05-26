using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class Rooms : MonoBehaviour
{
    public List<GameObject> rooms;
    public Sprite emptyRoom, bedroom, gym, obstacleCourse, library;
    public Text RoomInfo;
    public GameObject upgradeHallButton;
    public GameObject closeButton; //ONLY FOR UPGRADES

    private GetInstance getInstance;
    private Player player;
    private bool isConfigured = false;


    //builder - ONLY FOR UPGRADES
    private GameObject builder;
    // Start is called before the first frame update
    void Start()
    {
        if (!isConfigured)
        {
            Configure();
            isConfigured = true;
        }
    }

    void Configure()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        player = getInstance.PlayerAPI.getPlayer();

        for (int i = 0; i < player.NumberOfRooms; i++)
        {
            rooms[i].SetActive(true);
            rooms[i].GetComponent<RoomScript>().Configure(player.Rooms[i].Type, player.Rooms[i].Level, RoomInfo, i, this);

            switch(player.Rooms[i].Type)
            {
                case 0:
                    rooms[i].GetComponent<Image>().sprite = emptyRoom;
                    break;
                case 1:
                    rooms[i].GetComponent<Image>().sprite = bedroom;
                    break;
                case 2:
                    rooms[i].GetComponent<Image>().sprite = gym;
                    break;
                case 3:
                    rooms[i].GetComponent<Image>().sprite = obstacleCourse;
                    break;
                case 4:
                    rooms[i].GetComponent<Image>().sprite = library;
                    break;
            }

        }
    }

    public void BuilderUpgrade(GameObject b)
    {
        if (!isConfigured)
        {
            Configure();
            isConfigured = true;
        }

        builder = b;

        closeButton.SetActive(true);
        closeButton.GetComponent<Button>().onClick.AddListener(delegate { Delete(false); });

        for (int i = 0; i < player.NumberOfRooms; i++)
        {
            GameObject roomButton = rooms[i].transform.GetChild(0).gameObject;
            roomButton.SetActive(true);

            if(player.Rooms[i].Type == 0)
            {
                roomButton.GetComponentInChildren<Text>().text = "Build: " + VVC.buildPrice.ToString() +  "Gold";
            }
            else if(player.Rooms[i].Level < 3)
            {
                roomButton.GetComponentInChildren<Text>().text = "Upgrade:" + (VVC.upgradeBasePrice + player.Rooms[i].Level * VVC.upgradeLevelPrice).ToString() + "Gold";
            }
            else
                roomButton.GetComponentInChildren<Text>().text = "Max Level";

        }

        if (player.NumberOfRooms < rooms.Count)
        {
            upgradeHallButton.SetActive(true);
            upgradeHallButton.GetComponent<Button>().onClick.AddListener(UpgradeHall);
        }

    }

    public void UpgradeHall()
    {
        player.NumberOfRooms++;
        getInstance.GameManager.UpdateGuildHall();
        Delete(true);
    }

    public void UpgradeRoom(int number)
    {
        player.Rooms[number].Level++;
        getInstance.GameManager.UpdateGuildRooms();
    }

    public void BuildRoom(int number, int type)
    {
        player.Rooms[number].Type = type;
        player.Rooms[number].Level = 1;
        getInstance.GameManager.UpdateGuildRooms();

    }

    public void Delete(bool hasUpgraded)
    {
        if (hasUpgraded)
            Destroy(builder);
        else
            builder.GetComponent<Builder>().BuildingUnpause();

        getInstance.GameManager.ChangeMouseMode(MouseModes.WORLD);
        Destroy(gameObject);

    }

}
