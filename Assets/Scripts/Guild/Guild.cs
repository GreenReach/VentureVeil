using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;

public class Guild : MonoBehaviour
{
    public Sprite guildDefault;
    public Sprite guildOpenDoor;
    public GameObject guildMenu;

    private SpriteRenderer guildSprite;
    private GetInstance getInstance;

    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        guildSprite =  gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        getInstance.GameManager.ChangeMouseMode(MouseModes.UI);
        Instantiate(guildMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void OnMouseEnter()
    {
        guildSprite.sprite = guildOpenDoor;  
    }

    private void OnMouseExit()
    {
        guildSprite.sprite = guildDefault;
    }
}
