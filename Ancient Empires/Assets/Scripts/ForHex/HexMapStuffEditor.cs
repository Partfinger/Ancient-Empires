using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class HexMapStuffEditor : MonoBehaviour
{
    public List<Vector2Int> playerSpawnPoints = new List<Vector2Int>();
    List<BuildingSpawner> playerBuildings = new List<BuildingSpawner>();
    List<UnitSpawner> unitSpawners = new List<UnitSpawner>();
    public HexMapEditor editor;
    HexCell selectedCell;
    public int ActivePlayer = 0;
    public Text cellIndexText;
    public Dropdown players;


    bool BuildingMode = false, UnitMode = false, buildingCapture;
    int unitStartHP = 100, unitStartRank, activeUnit, activeBuilding, activeBuildingLevel;

    public UnitSpawner unitSpawnerPrefab;

    public HexCell SelectedCell
    {
        set
        {
            selectedCell = value;
            UpdateBuildingPanel();
        }
    }

    public HexGrid hexGrid;

    private void Start()
    {
        UpdateOwnerDropdown();
        enabled = false;
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                HexCell currentCell = GetCellUnderCursor();
                if (!currentCell)
                    return;
                else if (currentCell != selectedCell)
                    SelectedCell = currentCell;
                EditCell();
            }
        }
    }

    private void EditCell()
    {
        if (BuildingMode)
        {
            selectedCell.BuildingLevel = (byte)activeBuildingLevel;
            selectedCell.ActiveBuilding = (BuildingType)activeBuilding;
            selectedCell.Building.IsCaption = buildingCapture;
            //
            if (selectedCell.IsFeature)
            {
                selectedCell.FeatureLevel = 0;
                selectedCell.ActiveFeature = 0;
            }
        }
        if (UnitMode)
        {
            if (selectedCell.Unit)
                selectedCell.Unit.Remove();
            UnitSpawner newUnit = Instantiate(unitSpawnerPrefab);
            newUnit.unitID = activeUnit;
            newUnit.owner = ActivePlayer;
            newUnit.health = unitStartHP;
            newUnit.experience = 0;
            newUnit.rank = unitStartRank;
            newUnit.indexCell = selectedCell.Index;
        }
    }

    HexCell GetCellUnderCursor()
    {
        return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    public void UpdateOwnerDropdown()
    {
        if (players)
        {
            if (players.options.Count > 0)
            {
                players.options.Clear();
            }

            List<string> owners = new List<string>();
            for (int i = 1; i < editor.ActiveMap.MaxPlayers + 1; i++)
            {
                owners.Add($"Player {i}");
            }
            players.AddOptions(owners);
        }
    }

    public void SetEditMode(bool toggle)
    {
        gameObject.SetActive(toggle);
        enabled = toggle;
    }

    public void SetBuildingCaptured(bool toggle)
    {
        buildingCapture = toggle;
    }

    public void SetUnitStartHP(int newHP)
    {
        unitStartHP = newHP;
    }

    public void SetUnitStartRank(int rank)
    {
        unitStartRank = rank;
    }

    public void SetUnitSpawnID(int id)
    {

    }

    private void UpdateBuildingPanel()
    {
        cellIndexText.text = selectedCell.Index.ToString();
    }

    public void Save(BinaryWriter writer)
    {
        editor.ActiveMap.SpawnPositions = playerSpawnPoints;
        SaveUnitSpawner(writer);
        SaveBuildingSpawner(writer);
    }

    void SaveUnitSpawner(BinaryWriter writer)
    {
        writer.Write(unitSpawners.Count);
        for (int i =0; i < unitSpawners.Count; i++)
        {
            //public int unitID, owner, health, experience, rank, indexCell;
            writer.Write(unitSpawners[i].indexCell);
            writer.Write(unitSpawners[i].owner);
            writer.Write(unitSpawners[i].unitID);
            writer.Write(unitSpawners[i].health);
            writer.Write(unitSpawners[i].rank);
            writer.Write(unitSpawners[i].experience);
        }
    }

    public void SaveBuildingSpawner(BinaryWriter writer)
    {
        //public int buildingID, owner, number, status, indexCell;
        writer.Write(playerBuildings.Count);
        for (int i = 0; i < unitSpawners.Count; i++)
        {
            writer.Write(playerBuildings[i].indexCell);
            writer.Write(playerBuildings[i].owner);
            writer.Write(playerBuildings[i].buildingID);
            writer.Write(playerBuildings[i].number);
            writer.Write(playerBuildings[i].status);
        }
    }
}
