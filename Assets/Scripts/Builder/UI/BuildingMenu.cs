using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VentureVeilStructures;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingRoomOption;
    public GameObject panel;
    public Button closeButton;

    private RoomScript roomScript;
    private GetInstance getInstance;

    // Start is called before the first frame update
    void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        closeButton.onClick.AddListener(Delete);

        GameObject emptyRoom = Instantiate(buildingRoomOption, panel.transform);
        emptyRoom.GetComponent<BuildingRoomOption>().Configure(0, "Empty Room", this);

        GameObject bedroom = Instantiate(buildingRoomOption, panel.transform);
        bedroom.GetComponent<BuildingRoomOption>().Configure(1, "Bedroom", this);

        GameObject gym = Instantiate(buildingRoomOption, panel.transform);
        gym.GetComponent<BuildingRoomOption>().Configure(2, "Gym", this);

        GameObject obstacleCourse = Instantiate(buildingRoomOption, panel.transform);
        obstacleCourse.GetComponent<BuildingRoomOption>().Configure(3, "Obstacle Course", this);

        GameObject library = Instantiate(buildingRoomOption, panel.transform);
        library.GetComponent<BuildingRoomOption>().Configure(4, "Library", this);
    }

    public void Configure(RoomScript rs)
    {
        roomScript = rs;
    }

    public void BuildRoom(int type)
    {
        if (getInstance.GameManager.CheckGold(VVC.buildPrice))
        {
            getInstance.GameManager.PayGold(VVC.buildPrice);
            roomScript.BuildRoom(type);
            Destroy(gameObject);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
