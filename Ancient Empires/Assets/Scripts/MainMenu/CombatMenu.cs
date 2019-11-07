using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatMenu : MonoBehaviour, ILevelSelector
{
    public MainMenu mainMenu;
    public MapManager CurrentMap;
    MapManager selectedMapInList;
    [SerializeField]
    RectTransform playersPanel, mapListPanel, playersListContent;
    [SerializeField]
    Image minimap;
    [SerializeField]
    Text mapName;
    [SerializeField]
    Button SetMap, CancelButton;
    [SerializeField]
    Sprite emptyMini;
    bool notMap = true;
    List<PlayerForm> playerBuilders = new List<PlayerForm>();
    public PlayerForm playerFormBuilder;
    public Color[] colors;

    public SaveLoadItem itemPrefab;

    public RectTransform listContent;

    private void Start()
    {
        FillList();
        MapManager.EmptyMiniMap = emptyMini;
        PlayerForm currentPlayer = Instantiate(playerFormBuilder);
        currentPlayer.transform.SetParent(playersListContent.transform, false);
        currentPlayer.SetName("Pathfinder");
        playerBuilders.Add(currentPlayer);
        for (int i=0; i< 7; i++)
        {
            PlayerForm anyPlayer = Instantiate(playerFormBuilder);
            playerBuilders.Add(anyPlayer);
            anyPlayer.transform.SetParent(playersListContent.transform, false);
        }
    }

    public void ExitToMain()
    {
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void OpenMapListClick()
    {
        SetMap.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(true);
        playersPanel.sizeDelta = new Vector2(730, 260); //840
        mapListPanel.gameObject.SetActive(true);
    }

    public void CancelMapListClick()
    {
        CancelMapList();
        CurrentMap.GetMinimap(ref minimap);
    }

    void CancelMapList()
    {
        SetMap.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(false);
        mapListPanel.gameObject.SetActive(false);
        playersPanel.sizeDelta = new Vector2(985, 260);
    }

    public void SelectNewMap()
    {
        CancelMapList();
        CurrentMap = selectedMapInList;
        CurrentMap.GetMinimap(ref minimap);
        mapName.text = CurrentMap.Name;
        notMap = false;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }
        string[] paths =
            Directory.GetFiles(MapManager.path, "*.mapd");
        Array.Sort(paths);
        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
            item.menu = this;
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
            item.transform.SetParent(listContent, false);
        }
    }

    public void SelectItem(string name)
    {
        selectedMapInList = new MapManager(name);
        selectedMapInList.GetMinimap(ref minimap);
    }

    public void PlayClick()
    {
        if (!notMap)
        {
            PartieBridge.manager = CurrentMap;
            PartieBridge.players = new List<PlayerSpawner>();
            for (int i =0; i < playerBuilders.Count; i++)
            {
                PartieBridge.players.Add(playerBuilders[i].GetSpawner());
            }
            SceneManager.LoadScene("Scenes/GameScene");
        }
        else
            {
            Debug.LogWarning("You should select any map in order to start game.");
        }
    }
}
