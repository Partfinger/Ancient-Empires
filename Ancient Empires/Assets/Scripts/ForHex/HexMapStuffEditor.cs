using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HexMapStuffEditor : MonoBehaviour
{
    [SerializeField]
    List<Vector2Int> playerSpawnPoints = new List<Vector2Int>();
    Dictionary<int, int> playerBuildings = new Dictionary<int, int>();
    Dictionary<int, Vector2Int> playerUnits = new Dictionary<int, Vector2Int>();
    public HexMapEditor editor;
    Unit selectedUnit;
    HexCell selectedCell;
    HexBuilding selectedBuilding;
    [SerializeField]
    int ActivePlayer = 0;

    [SerializeField]
    Text cellIndexText;

    [SerializeField]
    Dropdown players;

    public HexCell SelectedCell
    {
        set
        {
            selectedCell = value;
            _Update();
        }
    }

    [SerializeField]
    Image unitPanel, buildingPanel;

    public HexGrid hexGrid;

    void _Update()
    {
        selectedUnit = selectedCell.Unit;
        selectedBuilding = selectedCell.Building;
        UpdateBuildingPanel();
        UpdateUnitPanel();
    }

    public void UpdateOwnerDropdown()
    {
        if (players.options.Count > 0)
        {
            players.options.Clear();
        }

        List<string> owners = new List<string>();
        for (int i =1; i < editor.ActiveMap.MaxPlayers + 1; i++)
        {
            owners.Add($"Player {i}");
        }
        players.AddOptions(owners);
    }

    public void SetEditMode(bool toggle)
    {
        gameObject.SetActive(toggle);
        enabled = toggle;
    }

    public void SetBuildingCaptured(bool toggle)
    {
        selectedBuilding.IsCaption = toggle;
    }

    public void SetBuildingOwner(bool toggle)
    {
        selectedBuilding.IsCaption = toggle;
    }

    public void SetUnitOwner(int id)
    {
        selectedUnit.Owner = id;
    }

    public void SetUnitStartHP(int newHP)
    {
        selectedUnit.Health = newHP;
    }

    public void SetUnitStartRank(int rank)
    {
        selectedUnit.Rank = rank;
    }

    public void SetUnitSpawnID(int id)
    {

    }

    public void UpdateBuildingSettings(Toggle toggle)
    {
        if (selectedBuilding)
        {
            selectedBuilding.IsCaption = toggle.isOn;
            selectedBuilding.Owner = ActivePlayer;
        }
    }

    public void UpdateUnitSettings()
    {
        if (selectedUnit)
        {
        }
    }

    private void UpdateBuildingPanel()
    {
        cellIndexText.text = selectedCell.Index.ToString();
    }

    private void UpdateUnitPanel()
    {
        unitPanel.gameObject.SetActive(true);
    }

    void CreateUnit()
    {
        if (selectedCell && !selectedCell.Unit)
        {
            /*hexGrid.AddUnit(
                Instantiate(unitsArray.GetUnit(1)), cell, Random.Range(0f, 360f)
            );*/
        }
    }

    void DestroyUnit()
    {
        if (selectedCell && selectedCell.Unit)
        {
            //hexGrid.RemoveUnit(cell.Unit);
        }
    }

    public void SetPlayerBuilding(int index, int player)
    {
        playerBuildings.Add(index, player);
    }

    public void RemovePlayerBuilding(int index)
    {
        playerBuildings.Remove(index);
    }

    public void SetSpawnUnitPos(int index, int player, int unitID)
    {
        playerUnits.Add(index, new Vector2Int(player, unitID));
    }

    public void RemoveSpawnPos(int index)
    {
        playerUnits.Remove(index);
    }
}
