using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageRoad : MonoBehaviour
{

    public LineRenderer road;
    public Transform flagPos;
    public GameObject partyObj;

    public void DrawRoad( GameObject flagStart)
    {
        road.SetPosition(0, flagStart.transform.position);
        road.SetPosition(1, flagPos.position);

        GameObject party = Instantiate(partyObj, flagStart.transform.position, Quaternion.identity);
    }
}
