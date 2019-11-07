using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Unit> units = new List<Unit>();
    [SerializeField]
    protected Unit commander;
    [SerializeField]
    public int ID, fractionID, Gold, goldIncome, commanderCounter = 1;
    public IPlayerInterface ui;
    public Color color;

    public Unit Commander
    {
        get
        {
            return commander;
        }
    }

    internal void Apply(PlayerSpawner spawner)
    {
        ID = spawner.ID;
        fractionID = spawner.teamID;
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
            return commander.IsDead;
        }
    }

    public void AddUnit(ref Unit newUnit)
    {
        units.Add(newUnit);
    }

    public void AddBoughtUnit(int id)
    {

    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void CommanderDie()
    {
        commanderCounter++;
    }

    public static bool operator ==(Player p1, Player p2)
    {
        return p1.ID == p2.ID;
    }

    public static bool operator !=(Player p1, Player p2)
    {
        return p1.ID != p2.ID;
    }

    public static bool IsAllies(Player p1, Player p2)
    {
        if (p1.fractionID > 0 && p2.fractionID > 0)
            if (p1.fractionID == p2.fractionID)
                return true;
        return false;
    }

    public bool IsAlly(Player any)
    {
        if (fractionID > 0 && any.fractionID > 0)
            if (fractionID == any.fractionID)
                return true;
        return false;
    }
}
