using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexGameUI : MonoBehaviour
{
    public HexGrid grid;
    HexCell currentCell;
    Unit selectedUnit;
    List<Ability> usableAbility;
    Ability selectedAbility;
    public Image AbilitySelectPanel;
    public AbilityLogicUI AbilitySelectorPrefub;
    public Image AbilitySelectPanelPrefub;
    public Canvas childCanvas;

    private void Awake()
    {
        Ability.grid = grid;
        AbilityLogicUI.UI = this;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedAbility = null;
                DoSelection();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (selectedAbility)
                {
                    UpdateCurrentCell();
                    selectedAbility.TriggerAbility(ref selectedUnit, ref currentCell);
                    selectedAbility = null;
                }
            }
        }
    }

    public void SelectedAbility(ref Ability ab)
    {
        ab.Selected(ref selectedUnit);
        selectedAbility = ab;
        HideAbilityPanel();
    }

    public void ClearSelectedAbility()
    {
        selectedAbility = null;
    }

    void DoPathfinding()
    {
        if (UpdateCurrentCell())
        {
            if (currentCell && selectedUnit.IsValidDestination(currentCell))
            {
                grid.FindPath(selectedUnit.Location, currentCell, selectedUnit.Speed);
            }
            else
            {
                grid.ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (grid.HasPath)
        {
            selectedUnit.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }

    void ShowAbilityPanel()
    {
        AbilitySelectPanel = Instantiate(AbilitySelectPanelPrefub, childCanvas.transform);
        int count;
        if ((count = usableAbility.Count) > 0)
        {
            AbilitySelectPanel.gameObject.SetActive(true);
            AbilitySelectPanel.rectTransform.sizeDelta = new Vector2(65 * count + 15, 65);
            for (int i = 0; i < count; i++)
            {
                AbilityLogicUI abilityLogic = Instantiate(AbilitySelectorPrefub, AbilitySelectPanel.transform);
                abilityLogic.transform.localPosition = new Vector3(-(15 + i * 65), 32.5f);
                abilityLogic.Ability = usableAbility[i];
            }
        }
    }

    void HideAbilityPanel()
    {
        if (AbilitySelectPanel)
        {
            Destroy(AbilitySelectPanel.gameObject);
            usableAbility.Clear();
        }
    }

    void DoSelection()
    {
        //grid.ClearPath();
        HideAbilityPanel();
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.Unit;
            if (selectedUnit)
            {
                usableAbility = selectedUnit.GetUsableAbility();
                ShowAbilityPanel();
            }
        }
    }

    bool UpdateCurrentCell()
    {
        HexCell cell =
            grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell)
        {
            currentCell = cell;
            return true;
        }
        return false;
    }

    public void SetEditMode(bool toggle)
    {
        enabled = !toggle;
        grid.ShowUI(!toggle);
        grid.ClearPath();
    }
}
