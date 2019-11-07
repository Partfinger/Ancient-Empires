using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class HexMapStuffEditor : MonoBehaviour
{
    public List<int> playerSpawnPoints = new List<int>();
    public Dictionary<int, BuildingSpawner> playerBuildings = new Dictionary<int, BuildingSpawner>();
    public Dictionary<int,UnitSpawner> unitSpawners = new Dictionary<int, UnitSpawner>();
    public HexMapEditor editor;
    HexCell selectedCell;
    public int ActivePlayer = 0;
    public Text cellIndexText;
    public Dropdown players, unitsDrop;
    public UnitsArray unitsArray;

    public HexEditorGrid hexGrid;

    bool BuildingMode = false, UnitMode = false, buildingCapture, withCommander = true;
    int unitStartHP = 100, unitStartRank, activeUnit, activeBuilding = -1, activeBuildingLevel, spawnPointMode = -1;

    public HexCell SelectedCell
    {
        set
        {
            selectedCell = value;
            UpdateBuildingPanel();
        }
    }

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
        int index = selectedCell.Index;
        if (BuildingMode)
        {
            if (playerBuildings.ContainsKey(index))
            {
                playerBuildings.Remove(index);
            }
            BuildingSpawner spawner = new BuildingSpawner(ActivePlayer, index, buildingCapture);
            selectedCell.BuildingLevel = (byte)activeBuildingLevel;
            selectedCell.ActiveBuilding = (BuildingType)activeBuilding;
            //
            if (selectedCell.IsFeature)
            {
                selectedCell.FeatureLevel = 0;
                selectedCell.ActiveFeature = 0;
            }
            selectedCell.chunk.Refresh();
        }
        if (UnitMode && spawnPointMode == -1)
        {
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
        if (spawnPointMode > -1)
        {
            if (selectedCell.Unit)
                selectedCell.Unit.Remove();
            if (!playerSpawnPoints.Contains(index))
                playerSpawnPoints.Add(index);
            if (unitSpawners.ContainsKey(index))
                unitSpawners.Remove(index);
            if (spawnPointMode == 0)
            {
                UnitSpawner newUnit = new UnitSpawner(0, ActivePlayer, unitStartHP, 0, unitStartRank, index);
                unitSpawners.Add(selectedCell.Index, newUnit);
                playerSpawnPoints.Add(index);
                RenderUnit(ref activeUnit);
            }
            
        }
    }

    HexCell GetCellUnderCursor()
    {
        return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void RenderUnit(ref int id)
    {
        Unit unit = Instantiate(unitsArray.GetUnit(id), hexGrid.transform);
        selectedCell.Unit = unit;
        unit.Location = selectedCell;
    }

    public void SetSpawnPointMode(int newMode)
    {
        spawnPointMode = newMode;
    }

    public void SetCommandSpawner(bool toggle)
    {
        withCommander = toggle;
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

    public void SetActiveBuilding(int id)
    {
        activeBuilding = id;
    }

    private void UpdateBuildingPanel()
    {
        cellIndexText.text = selectedCell.Index.ToString();
    }

    public void Save(BinaryWriter writer)
    {
        List<Vector2Int> spPos = new List<Vector2Int>();
        Vector3 realPos;
        for (int i = 0; i < playerSpawnPoints.Count; i++)
        {
            realPos = hexGrid.GetCell(playerSpawnPoints[i]).Position;
            spPos.Add(new Vector2Int((int)realPos.x, (int)realPos.z));
        }
        editor.ActiveMap.SpawnPositions = spPos;
        SaveBuildingSpawner(writer);
        SaveUnitSpawner(writer);
    }

    public void Load(BinaryReader reader)
    {
        LoadBuildingSpawner(reader);
        LoadUnitSpawner(reader);
        selectedCell = null;
    }

    private void LoadBuildingSpawner(BinaryReader reader)
    {
        unitSpawners.Clear();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            BuildingSpawner newUnitSpawner = new BuildingSpawner(reader);
            playerBuildings.Add(newUnitSpawner.indexCell, newUnitSpawner);
        }
    }

    public void OldLoad(BinaryReader reader)
    {
        OldLoadUnitSpawner(reader);
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
        foreach (KeyValuePair<int, BuildingSpawner> keyValuePair in playerBuildings)
        {
            keyValuePair.Value.Save(writer);
        }
    }

    public void OldLoadUnitSpawner(BinaryReader reader)
    {
        unitSpawners.Clear();
        int unitCount = reader.ReadInt32();
        for (int i = 0; i < unitCount; i++)
        {
            UnitSpawner newUnitSpawner = new UnitSpawner
            {
                indexCell = reader.ReadInt32(),
                health = reader.ReadInt32(),
                experience = reader.ReadInt32(),
                rank = reader.ReadInt32()
            };
            HexCoordinates.Load(reader);
            reader.ReadSingle();
            unitSpawners.Add(newUnitSpawner.indexCell, newUnitSpawner);
            selectedCell = hexGrid.GetCell(newUnitSpawner.indexCell);
            RenderUnit(ref newUnitSpawner.unitID);
        }
    }

    public void LoadUnitSpawner(BinaryReader reader)
    {
        unitSpawners.Clear();
        int unitCount = reader.ReadInt32();
        for (int i =0; i < unitCount; i++)
        {
            UnitSpawner newUnitSpawner = new UnitSpawner(reader);
            unitSpawners.Add(newUnitSpawner.indexCell, newUnitSpawner);
            selectedCell = hexGrid.GetCell(newUnitSpawner.indexCell);
            RenderUnit(ref newUnitSpawner.unitID);
        }
    }
}
