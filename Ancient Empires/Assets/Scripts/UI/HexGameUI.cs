using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexGameUI : MonoBehaviour, IPlayerInterface
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

    public Unit SelectedUnit
    {
        get
        {
            return selectedUnit;
        }
        set
        {
            selectedUnit = value;
            currentCell = selectedUnit.Location;
            usableAbility.Clear();
            usableAbility.AddRange(selectedUnit.GetUsableAbility());
            ShowAbilityPanel();
        }
    }

    public Ability SelectedAbility
    {
        get
        {
            return selectedAbility;
        }
        set
        {
            selectedAbility = value;
            selectedAbility.Selected();
            HideAbilityPanel();
        }
    }

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
                currentUpdate = DefUpdate;
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
                if (!selectedUnit.enabled)
                {
                    usableAbility.AddRange(selectedUnit.GetUsableAbility());
                }
                else
                {
                    selectedUnit = null;
                }
            }
            if (currentCell.Building is HexCastle && Market.IsUsable())
            {
                usableAbility.Add(Market);
            }
            if (usableAbility.Count > 0)
                ShowAbilityPanel();
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

    public void MovementInt(ref Unit unit)
    {
        selectedUnit = unit;
        selectedAbility = unit.GetOnlyMove();
        currentUpdate = SelectUnitMove;
    }
}
