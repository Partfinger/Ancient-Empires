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
    bool abilityCompleted = true, abilityShow = false, NewUnit = false;
    [SerializeField]
    Player currentPlayer;

    public enum UpdateStatus : byte { def, aterMove, MoveNewUnit, afterMarketAfteMove };

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
        grid.gui = AbilityLogicUI.UI = this;
        currentUpdate = DefaultUpdate;
    }

    void Update()
    {
        currentUpdate();
    }

    void TriggerAbility()
    {
        UpdateCurrentCellForAbility();
        abilityCompleted = selectedAbility.TriggerAbility(selectedUnit, currentCell); ;
        selectedAbility = null;
    }

    void BackToMove()
    {
        HideAbilityPanel();
        selectedUnit.movement.Back(selectedUnit, NewUnit);
        selectedAbility = selectedUnit.movement;
        abilityCompleted = selectedAbility.Selected(selectedUnit);
    }

    void DefaultUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedAbility && !abilityCompleted)
                {
                    TriggerAbility();
                    abilityCompleted = true;
                }
                else
                    DoSelection();
            }
        }
    }

    void MoveNewUnit()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateCurrentCellForAbility();
            if (currentCell != selectedUnit.Location)
            {
                abilityCompleted = (selectedAbility as Movement).Trigger( ref selectedUnit, ref currentCell);
                if (abilityCompleted)
                {
                    selectedAbility = null;
                    currentUpdate = SelectAbilityAfterNewMove;
                    status = UpdateStatus.afterMarketAfteMove;
                }
            }
        }
    }

    void SelectAbilityAfterNewMove()
    {
        if (selectedAbility)
        {
            if (abilityCompleted)
            {
                NewUnit = false;
                selectedAbility = null;
                currentUpdate = DefaultUpdate;
                status = UpdateStatus.def;
            }
            else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                TriggerAbility();
                if (abilityCompleted)
                {
                    NewUnit = false;
                    currentUpdate = DefaultUpdate;
                    status = UpdateStatus.def;
                }
                else
                {
                    abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
                    ShowAbilityPanel();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            BackToMove();
            currentUpdate = MoveNewUnit;
            status = UpdateStatus.MoveNewUnit;
        }
        /*
        if ()
        {
            if (selectedAbility)
            {
                TriggerAbility();
                if (abilityCompleted)
                {
                    NewUnit = false;
                    currentUpdate = DefaultUpdate;
                    status = UpdateStatus.def;
                }
                else
                {
                    abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
                    ShowAbilityPanel();
                }
            }
            else
            {
                BackToMove();
                status = UpdateStatus.MoveNewUnit;
                currentUpdate = MoveNewUnit;
            }
        }
        */
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
                TriggerAbility();
                if (abilityCompleted)
                {
                    currentUpdate = DefaultUpdate;
                    status = UpdateStatus.def;
                }
                else
                {
                    abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
                    ShowAbilityPanel();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            BackToMove();
            currentUpdate = DefaultUpdate;
            status = UpdateStatus.def;
        }
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

    public void Wait()
    {
        enabled = false;
    }

    public void SelectUnitAfterMove()
    {
        enabled = true;
        selectedAbility = null;
        abilityManager.GetAbilitiesAfterTravel(selectedUnit, ref usableAbility);
        ShowAbilityPanel();
        if (!NewUnit)
        {
            currentUpdate = SelectAbilityAfterMove;
            status = UpdateStatus.aterMove;
        }
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
        NewUnit = manager.AddBoughtUnit(currentPlayer, id, currentCell, 0f);
        selectedAbility = selectedUnit.movement;
        if (NewUnit)
        {
            currentUpdate = MoveNewUnit;
            status = UpdateStatus.MoveNewUnit;
        }
        else
        {
            currentUpdate = DefaultUpdate;
            status = UpdateStatus.def;
        }
        abilityCompleted = selectedAbility.Selected(selectedUnit);
    }

    void HideAbilityPanel()
    {
        AbilitySelectPanel.gameObject.SetActive(false);
        Destroy(abilityContent);
        usableAbility.Clear();
        abilityShow = false;
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
                usableAbility.Clear();
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

    public void OpenUnitMarket()
    {
        MarketPanel.gameObject.SetActive(true);
    }

    public void HideUnitMarket()
    {
        MarketPanel.gameObject.SetActive(false);
    }
}
