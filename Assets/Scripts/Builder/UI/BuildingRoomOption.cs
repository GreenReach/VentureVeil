using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRoomOption : MonoBehaviour
{
    public Button buildButton;
    public Text description;

    private BuildingMenu buildingMenu;
    private int type;

    public void Configure(int t, string d, BuildingMenu bm)
    {
        type = t;
        description.text = d;
        buildingMenu = bm;
        buildButton.onClick.AddListener(BuildRoom);
    }

    void BuildRoom()
    {
        buildingMenu.BuildRoom(type);
    }
    
}
