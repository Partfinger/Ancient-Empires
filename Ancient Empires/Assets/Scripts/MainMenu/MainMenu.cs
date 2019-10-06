using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuContent, singlePlayerContent;
    public SkirmishMenu skirmishMenu;

    private void Awake()
    {
        MapManager.UpdatePath();
    }

    public void OpenMapEditor()
    {
        SceneManager.LoadScene("Scenes/EditorScene");
    }

    public void OpenSkirmishMenu()
    {
        gameObject.SetActive(false);
        skirmishMenu.gameObject.SetActive(true);
    }

    public void SingleClick()
    {
        mainMenuContent.SetActive(false);
        singlePlayerContent.SetActive(true);
    }

    public void SingleBackClick()
    {
        singlePlayerContent.SetActive(false);
        mainMenuContent.SetActive(true);
    }

    public void ExitClick()
    {
        Application.Quit(0);
    }
}
