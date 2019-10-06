using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkirmishMenu : MonoBehaviour
{
    public MainMenu mainMenu;
    public MapManager map;
    [SerializeField]
    Image minimap, playersPanel;
    [SerializeField]
    Text mapName;
    [SerializeField]
    Button SetMap, ApplyMap; 

    public void ExitToMain()
    {
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void SelectMapOnclick()
    {
        SetMap.gameObject.SetActive(false);
        ApplyMap.gameObject.SetActive(true);
        playersPanel.rectTransform.sizeDelta = new Vector2(560, 260); //840
    }

    public void SelectNewMap()
    {
        SetMap.gameObject.SetActive(true);
        ApplyMap.gameObject.SetActive(false);
        playersPanel.rectTransform.sizeDelta = new Vector2(840, 260);
    }
}
