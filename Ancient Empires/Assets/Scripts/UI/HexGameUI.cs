using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexGameUI : MonoBehaviour
{
    public HexGrid grid;
    public HexCell currentCell;
    public Unit selectedUnit;
    List<Ability> usableAbility = new List<Ability>();
    Ability selectedAbility;
    public Image AbilitySelectPanel;
    public AbilityLogicUI AbilitySelectorPrefub;
    public UnitMarket Market;
    public Image AbilitySelectPanelPrefub;
    public Image MarketPanel;
    public RectTransform MarketContent;
    public Canvas childCanvas;
    [SerializeField]
    bool abilityCompleted = false;

    delegate void _Update();

    _Update currentUpdate;

    private void Awake()
    {
        Ability.grid = grid;
        grid.gui = Ability.gui = AbilityLogicUI.UI = Unit.gui = this;
        Market._Awake();
        currentUpdate = DefUpdate;
    }

    void Update()
    {
        currentUpdate();
    }

    public void AbilityCompleted()
    {
        abilityCompleted = true;
        selectedAbility = null;
        usableAbility.Clear();
    }

    void DefUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedAbility is UnitMarket)
                    MarketPanel.gameObject.SetActive(false);
                selectedAbility = null;

                DoSelection();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (selectedAbility)
                {
                    UpdateCurrentCell();
                    selectedAbility.TriggerAbility(ref selectedUnit, ref currentCell);
                    if (abilityCompleted)
                    {
                        abilityCompleted = false;
                    }
                }
            }
        }
    }

    void SelectUnitMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UpdateCurrentCell();
            selectedAbility.TriggerAbility(ref selectedUnit, ref currentCell);
            if (abilityCompleted)
            {
                abilityCompleted = false;
            }
        }
    }

    void SelectAbilityAfterMove()
    {
        if (abilityCompleted)
        {
            abilityCompleted = false;
            currentUpdate = DefUpdate;
        }
        else if (selectedAbility)
        {
            if (Input.GetMouseButtonDown(1))
            {

                UpdateCurrentCell();
                selectedAbility.TriggerAbility(ref selectedUnit, ref currentCell);
                if (abilityCompleted)
                {
                    abilityCompleted = false;
                    currentUpdate = DefUpdate;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                selectedAbility = null;
                usableAbility.AddRange(selectedUnit.GetUsableAbilityAfterTravel());
                ShowAbilityPanel();
            }
        }

    }

    public void WaitForEndOfTravel()
    {
        enabled = false;
    }

    public void SelectUnitAfterMove()
    {
        enabled = true;
        selectedAbility = null;
        usableAbility.AddRange(selectedUnit.GetUsableAbilityAfterTravel());
        ShowAbilityPanel();
        abilityCompleted = false;
        currentUpdate = SelectAbilityAfterMove;
    }

    public void NewUnitMove(ref Unit unit)
    {
        selectedUnit = unit;
        selectedAbility = unit.GetOnlyMove();
        currentUpdate = SelectUnitMove;
    }

    public void SelectedAbility(ref Ability ab)
    {
        selectedAbility = ab;
        ab.Selected();
        HideAbilityPanel();
    }

    public void ClearSelectedAbility()
    {
        selectedAbility = null;
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
        HideAbilityPanel();
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.Unit;
            if (selectedUnit)
            {
                usableAbility.AddRange(selectedUnit.GetUsableAbility());
            }
            if (currentCell.Building is HexCastle && Market.IsUsable())
            {
                usableAbility.Add(Market);
            }
            if (usableAbility.Count > 0)
                ShowAbilityPanel();
        }
    }

    public void SelectedUnit(ref Unit unit)
    {
        selectedUnit = unit;
        currentCell = unit.Location;
        usableAbility.Clear();
        usableAbility.AddRange(selectedUnit.GetUsableAbility());
        ShowAbilityPanel();
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
