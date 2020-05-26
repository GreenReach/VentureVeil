using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject GuildPrefab;

    private GetInstance getInstance;
    private GameObject guild;

    void Start()
    {
        guild = Instantiate(GuildPrefab, Input.mousePosition, Quaternion.identity);
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit);
        guild.transform.position = hit.point;


        if(Input.GetMouseButton(0) && hit.transform.name == "Plane")
        {
            guild.transform.position = hit.point;
            getInstance.Guild = guild;
            gameManager.enabled = true;
            gameManager.guild = guild;
            Destroy(this);
        }

    }
}

