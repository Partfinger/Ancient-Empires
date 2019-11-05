using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexGameUI : MonoBehaviour, IPlayerInterface
{
    public HexGameGrid grid;
    public HexCell currentCell;
    public Unit selectedUnit;
    List<Ability> usableAbility = new List<Ability>();
    Ability selectedAbility;
    public Image AbilitySelectPanel;
    public AbilityLogicUI AbilitySelectorPrefub;
    public Image AbilitySelectPanelPrefub;
    public Image MarketPanel;
    public RectTransform MarketContent;
    public GameObject abilityContent;
    public GameObject abilityContentPrefub;
    public AbilityManager abilityManager;
    public UnitMarket market;
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
            if (selectedAbility is UnitAbility)
            {
                selectedAbility.Selected(ref selectedUnit);
                if (selectedAbility is EndTurnAbility)
                {
                    abilityCompleted = true;
                    selectedAbility = null;
                }
            }
            else
                selectedAbility.Selected(this);
            HideAbilityPanel();
        }
    }

    public Player Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        Ability.grid = grid;
        grid.gui = Unit.gui = AbilityLogicUI.UI = this;
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

                DoSelection();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (selectedAbility)
                {
                    UpdateCurrentCell();
                    selectedAbility.TriggerAbility(ref selectedUnit, ref currentCell);
                    if (abilityCompleted)
                        abilityCompleted = false;
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
                abilityCompleted = false;
                currentUpdate = DefUpdate;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                selectedAbility = null;
                usableAbility = abilityManager.GetAbilitiesAfterTravel(this, ref selectedUnit);
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
        usableAbility = abilityManager.GetAbilitiesAfterTravel(this, ref selectedUnit);
        ShowAbilityPanel();
        abilityCompleted = false;
        currentUpdate = SelectAbilityAfterMove;
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

    void HideAbilityPanel()
    {
        AbilitySelectPanel.gameObject.SetActive(false);
        Destroy(abilityContent);
        usableAbility.Clear();
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
                    usableAbility = abilityManager.GetAbilitiesForCell(this, ref currentCell);
                }
                else
                {
                    selectedUnit = null;
                }
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
        selectedAbility = abilityManager.GetMovement();
        currentUpdate = SelectUnitMove;
    }

    public void OpenUnitMarket()
    {
        throw new System.NotImplementedException();
    }

    public void HideUnitMarket()
    {
        throw new System.NotImplementedException();
    }
}
