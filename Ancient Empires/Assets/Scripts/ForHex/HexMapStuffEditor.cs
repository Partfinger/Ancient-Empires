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
    Dictionary<int,UnitSpawner> unitSpawners = new Dictionary<int, UnitSpawner>();
    public HexMapEditor editor;
    HexCell selectedCell;
    public int ActivePlayer = 0;
    public Text cellIndexText;
    public Dropdown players, unitsDrop;
    public UnitsArray unitsArray;


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
        UpdateUnitDropdown();
        enabled = false;
    }

    private void UpdateUnitDropdown()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < unitsArray.Length; i++)
        {
            names.Add(unitsArray.GetUnit(i).name);
        }
        unitsDrop.AddOptions(names);
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
            int index = selectedCell.Index;
            if (activeUnit > -1)
            {
                if (selectedCell.Unit)
                    selectedCell.Unit.Remove();
                if (unitSpawners.ContainsKey(index))
                    unitSpawners.Remove(index);
                UnitSpawner newUnit = new UnitSpawner(activeUnit, ActivePlayer, unitStartHP,0, unitStartRank, index);
                unitSpawners.Add(selectedCell.Index, newUnit);
                RenderUnit(ref activeUnit);
            }
            else
            {
                if (unitSpawners.ContainsKey(index))
                {
                    unitSpawners.Remove(index);
                    selectedCell.Unit.Remove();
                }
            }
        }
    }

    HexCell GetCellUnderCursor()
    {
        return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void RenderUnit(ref int id)
    {
        Debug.Log("Unit");
        Unit unit = Instantiate(unitsArray.GetUnit(id), hexGrid.transform);
        selectedCell.Unit = unit;
        unit.Location = selectedCell;
    }

    public void SetUnitMode(bool toggle)
    {
        UnitMode = toggle;
    }

    public void SetBuildingMode(bool toggle)
    {
        BuildingMode = toggle;
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

    public void SetUnitStartHP(string newHP)
    {
        unitStartHP = int.Parse(newHP);
    }

    public void SetUnitStartRank(string rank)
    {
        unitStartRank = int.Parse(rank);
    }

    public void SetActiveUnit(int id)
    {
        activeUnit = id - 1;
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
        foreach (KeyValuePair<int, UnitSpawner> keyValuePair in unitSpawners)
        {
            keyValuePair.Value.Save(writer);
        }
    }

    public void SaveBuildingSpawner(BinaryWriter writer)
    {
        writer.Write(playerBuildings.Count);
        for (int i = 0; i < playerBuildings.Count; i++)
        {
            writer.Write(playerBuildings[i].indexCell);
            writer.Write(playerBuildings[i].owner);
            writer.Write(playerBuildings[i].buildingID);
            writer.Write(playerBuildings[i].number);
            writer.Write(playerBuildings[i].status);
        }
    }
}
