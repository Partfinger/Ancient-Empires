using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HexMapScriptEditor : MonoBehaviour
{
    [SerializeField]
    Dictionary<int, int> playerBuildings = new Dictionary<int, int>();
    Dictionary<int, Vector2Int> playerUnits = new Dictionary<int, Vector2Int>();
    Unit selectedUnit;
    HexCell selectedCell;
    HexBuilding selectedBuilding;

    [SerializeField]
    Image unitPanel, buildingPanel;

    [SerializeField]
    Dropdown dropdownOwner;

    public HexGrid hexGrid;

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
                return;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DestroyUnit();
                }
                else
                {
                    CreateUnit();
                }
                return;
            }
        }
        selectedCell = null;
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

    void HandleInput()
    {
        selectedCell = hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        selectedUnit = selectedCell.Unit;
        if (selectedUnit)
            UpdateUnitPanel();
        selectedBuilding = selectedCell.Building;
        if (selectedBuilding)
            UpdateBuildingPanel();
    }

    private void UpdateBuildingPanel()
    {
        buildingPanel.gameObject.SetActive(true);

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
