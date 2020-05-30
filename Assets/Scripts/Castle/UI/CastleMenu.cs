using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleMenu : MonoBehaviour
{
    private GetInstance getInstance;
    private GameManager gameManager;
    private Castle castle;

    private void Start()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        gameManager = getInstance.GameManager;
    }
    public void ButtonPressed(string action)
    {
        bool result = false;
        if (action.Equals("tale"))
        {
            result = gameManager.TellTales();
        }
        else
        {
            result = gameManager.BuyFavor(action);
        }

    }
}
