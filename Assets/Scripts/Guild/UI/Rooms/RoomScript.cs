using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class RoomScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject buildingMenu;
    public Button button;

    private Text roomInfo;
    private int roomType, roomLevel, number;
    private GetInstance getInstance;
    private Rooms rooms;

    // Start is called before the first frame update
    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        button.onClick.AddListener(UpgradeRoom);
    }

    private void UpgradeRoom()
    {
        if(roomType == 0)
        {
            if (getInstance.GameManager.CheckGold(VVC.buildPrice))
            {
                print("Open room building menu");
                GameObject bMenu = Instantiate(buildingMenu, gameObject.transform.parent);
                bMenu.GetComponent<BuildingMenu>().Configure(this);
            }
        }
        else if(roomLevel < 3)
        {
            if (getInstance.GameManager.CheckGold(VVC.upgradeBasePrice + roomLevel * VVC.upgradeLevelPrice))
            {
                rooms.UpgradeRoom(number);
                getInstance.GameManager.PayGold(VVC.upgradeBasePrice + roomLevel * VVC.upgradeLevelPrice);
                rooms.Delete(true);
            }
        }

    }

    public void BuildRoom(int type)
    {
        rooms.BuildRoom(number, type);
        rooms.Delete(true);
    }

    public void Configure(int type, int level, Text info, int nr, Rooms script)
    {
        roomType = type;
        roomLevel = level;
        roomInfo = info;
        number = nr;
        rooms = script;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string roomName = "Empty room";

        switch (roomType)
        {
            case 0:
                roomName = "Empty room";
                break;
            case 1:
                roomName = "Bedroom";
                break;
            case 2:
                roomName = "Gym";
                break;
            case 3:
                roomName = "Obstacle Course";
                break;
            case 4:
                roomName = "Library";
                break;
        }

        roomInfo.text = roomName + "\nLevel: " + roomLevel;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        roomInfo.text = "Room Type \nRoom Level";
    }
}
