using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public UnitsArray setup;
    protected List<Unit> units = new List<Unit>();
    protected Unit commander;
    protected int ID;
    protected int fractionID;
    protected int Gold;
    protected int goldIncome;
    protected int commanderCounter = 1;
    public IPlayerInterface ui;

    public Unit Commander
    {
        get
        {
            return commander;
        }
    }

    public bool UnitLimit
    {
        get
        {
            return units.Count < 10;
        }
    }

    public bool HasCommander
    {
        get
        {
            return commander != null;
        }
    }

    public void AddUnit(ref Unit newUnit)
    {
        units.Add(newUnit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void CommanderDie()
    {
        commanderCounter++;
    }
}
