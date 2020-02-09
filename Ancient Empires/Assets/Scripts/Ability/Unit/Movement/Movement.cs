using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : UnitAbility
{
    protected List<int> Space = ListPool<int>.Get();
    protected List<HexCell> Frontier = ListPool<HexCell>.Get();
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

    public void Back(Unit unit, bool isNew)
    {
        if (isNew)
            unit.SetLocationQuiet(fromCell);
        else
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
            current.SetLabel("");
            current.WasChecked = false;        
        }
        Space.Clear();
    }

    public override bool TriggerAbility(Unit unit, HexCell cell)
    {
        bool res = Trigger(ref unit, ref cell);
        if (!res)
            Canceled();
        return res;
    }

    public abstract bool Trigger(ref Unit unit, ref HexCell destination);

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
