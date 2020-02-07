using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : UnitAbility
{
    protected List<int> Space = new List<int>();
    protected List<HexCell> Frontier = new List<HexCell>();
    public static byte EmptyCost = 10;
    protected static HexCell fromCell;

    public override bool IsUsable(Unit unit)
    {
        return true;
    }

    public override void Canceled()
    {
        Finish();
        fromCell = null;
    }

    public void Back(Unit unit)
    {
        unit.Location = fromCell;
    }

    protected void Finish()
    {
        HexCell current;
        for (int i = 0; i < Space.Count; i++)
        {
            current = grid.GetCell(Space[i]);
            current.Distance = -1;
            current.DisableHighlight();
            current.WasChecked = false;        }
        Space.Clear();
    }

    protected List<HexCell> GetPath(HexCell from, HexCell to)
    {
        List<HexCell> path = ListPool<HexCell>.Get();
        for (HexCell c = to; c != from; c = c.PathFrom)
        {
            path.Add(c);
        }
        path.Add(from);
        path.Reverse();
        return path;
    }
}
