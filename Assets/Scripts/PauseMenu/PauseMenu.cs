using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button resume, saveGame, exitToMenu, exitToDesktop;

    private GetInstance getInstance;
    private bool isConfigured;

    private void Awake()
    {
        if (!isConfigured)
            Configure();
    }

    void Configure()
    {
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();

        resume.onClick.AddListener(Resume);
        saveGame.onClick.AddListener(SaveGame);
        exitToMenu.onClick.AddListener(ExitToMenu);
        exitToDesktop.onClick.AddListener(ExitToDesktop);

        isConfigured = true;
    }

    void Resume()
    {
        getInstance.SoundManager.Play("button");
        getInstance.GameManager.PauseGame();
        
    }

    void SaveGame()
    {
        getInstance.SoundManager.Play("button");
        getInstance.GameManager.SaveGame();
    }

    void ExitToMenu()
    {
        getInstance.SoundManager.Play("button");
        SceneManager.LoadScene("Menu");
    }

    void ExitToDesktop()
    {
        getInstance.SoundManager.Play("button");
        Application.Quit();
    }
}
