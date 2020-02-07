using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexGameUI : MonoBehaviour, IPlayerInterface
{
    [Header("Current update:")]
    [SerializeField] UpdateStatus status = 0;
    public HexGameGrid grid;
    public HexCell currentCell;
    public Unit selectedUnit;
    List<Ability> usableAbility = new List<Ability>();
    [SerializeField]
    Ability selectedAbility;
    public Image AbilitySelectPanel;
    public AbilityLogicUI AbilitySelectorPrefub;
    public Image AbilitySelectPanelPrefub;
    public Image MarketPanel;
    public GameObject abilityContent;
    public GameObject abilityContentPrefub;
    public AbilityManager abilityManager;
    public UnitManager manager;
    [SerializeField]
    bool abilityCompleted = true, abilityShow = false, unitAfterTravel = false;
    [SerializeField]
    Player currentPlayer;

    public enum UpdateStatus : byte { def, aterMove};

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
            abilityCompleted = ((selectedAbility = value) is UnitAbility) ? selectedAbility.Selected(selectedUnit) : selectedAbility.Selected(this);
            HideAbilityPanel();
        }
    }

    public Player Player
    {
        get
        {
            return currentPlayer;
        }
        set
        {
            currentPlayer = value;
            currentPlayer.ui = this;
        }
    }

    private void Awake()
    {
        Ability.grid = grid;
        grid.gui = Unit.gui = AbilityLogicUI.UI = this;
        currentUpdate = DefaultUpdate;
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

    void DefaultUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedAbility && !abilityCompleted)
                {
                    UpdateCurrentCellForAbility();
                    selectedAbility.TriggerAbility(selectedUnit, currentCell);
                    abilityCompleted = true;
                    selectedAbility = null;
                }
                else
                    DoSelection();
            }
        }
    }

    void SelectUnitMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UpdateCurrentCell();
            selectedAbility.TriggerAbility(selectedUnit, currentCell);
            if (abilityCompleted)
            {
                abilityCompleted = false;
                //currentUpdate = DefUpdate;
            }
        }
    }

    void SelectAbilityAfterMove()
    {
        if (selectedAbility)
        {
            if (abilityCompleted)
            {
                selectedAbility = null;
                currentUpdate = DefaultUpdate;
                status = UpdateStatus.def;
                return;
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                UpdateCurrentCellForAbility();
                abilityCompleted = selectedAbility.TriggerAbility(selectedUnit, currentCell);
                if (abilityCompleted)
                {
                    selectedAbility = null;
                    currentUpdate = DefaultUpdate;
                    status = UpdateStatus.def;
                    return;
                }
                else
                {
                    selectedAbility = null;
                    abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
                    ShowAbilityPanel();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HideAbilityPanel();
            selectedUnit.movement.Back(selectedUnit);
            selectedAbility = selectedUnit.movement;
            abilityCompleted = selectedAbility.Selected(selectedUnit);
            currentUpdate = DefaultUpdate;
            status = UpdateStatus.def;
            return;
        }
        /*if (selectedAbility)
        {
            if (abilityCompleted)
            {
                selectedAbility = null;
                currentUpdate = DefaultUpdate;
                status = UpdateStatus.def;
            }
            if (Input.GetMouseButtonDown(1))
            {
                UpdateCurrentCellForAbility();
                selectedAbility.TriggerAbility(selectedUnit, currentCell);
                abilityCompleted = true;
                selectedAbility = null;
                currentUpdate = DefaultUpdate;
                status = UpdateStatus.def;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                selectedAbility = null;
                abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
                ShowAbilityPanel();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0))
            {
                HideAbilityPanel();
                selectedUnit.movement.Back(selectedUnit);
                selectedAbility = selectedUnit.movement;
                abilityCompleted = selectedAbility.Selected(selectedUnit);
                currentUpdate = DefaultUpdate;
                status = UpdateStatus.def;
            }
        }*/
    }

    public void WaitForEndOfTravel()
    {
        enabled = false;
    }

    public void SelectUnitAfterMove()
    {
        enabled = true;
        selectedAbility = null;
        abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
        ShowAbilityPanel();
        currentUpdate = SelectAbilityAfterMove;
        status = UpdateStatus.aterMove;
    }

    void ShowAbilityPanel()
    {
        int count;
        if ((count = usableAbility.Count) > 0)
        {
            AbilitySelectPanel.gameObject.SetActive(true);
            AbilitySelectPanel.rectTransform.sizeDelta = new Vector2(65 * count + 15, 65);
            abilityContent = Instantiate(abilityContentPrefub, AbilitySelectPanel.transform);
            for (int i = 0; i < count; i++)
            {
                AbilityLogicUI abilityLogic = Instantiate(AbilitySelectorPrefub, abilityContent.transform);
                abilityLogic.transform.localPosition = new Vector3(-(15 + i * 65), 32.5f);
                abilityLogic.Ability = usableAbility[i];
            }
        }
    }

    public void AddBoughtUnit(int id)
    {
        manager.AddBoughtUnit(currentPlayer, id, currentCell, 0f);
    }

    void HideAbilityPanel()
    {
        AbilitySelectPanel.gameObject.SetActive(false);
        Destroy(abilityContent);
        usableAbility.Clear();
        abilityShow = false;
    }

    void DoSelection()
    {
        if (abilityShow)
        {
            HideAbilityPanel();
        }
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.Unit;
            if (selectedUnit && selectedUnit.Owner != currentPlayer)
            {
                selectedUnit = null;
            }
            if (abilityCompleted)
            {
                abilityManager.GetAbilitiesForCell(currentCell, selectedUnit, this, ref usableAbility);
                if (usableAbility.Count > 0)
                {
                    ShowAbilityPanel();
                    abilityShow = true;
                }
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
            if (!abilityCompleted)
            {
                selectedAbility.Canceled();
                abilityCompleted = true;
            }
            return true;
        }
        return false;
    }

    void UpdateCurrentCellForAbility()
    {
        HexCell cell =
            grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell)
        {
            currentCell = cell;
        }
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
        selectedAbility = abilityManager.GetMovement();
        currentUpdate = SelectUnitMove;
    }

    public void OpenUnitMarket()
    {
        MarketPanel.gameObject.SetActive(true);
    }

    public void HideUnitMarket()
    {
        MarketPanel.gameObject.SetActive(false);
    }
}
