using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public Sprite defaultSprite, hoverSprite;
    public GameObject castleMenu;

    private SpriteRenderer spriteRenderer;
    private bool isOpen;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        isOpen = false;
    }
    private void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
            castleMenu.SetActive(true);
        }
        else
        {
            isOpen = false;
            castleMenu.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        if (isOpen)
        {
            isOpen = false;
            castleMenu.SetActive(true);
        }
    }

    public void OpenMenu()
    {
        if (!isOpen)
        {
            isOpen = true;
            castleMenu.SetActive(true);
        }
    }
}