using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Ability", menuName = "Movement Ability", order = 54)]
public class Movement : Ability
{

    PathCostManager.Calculate calculate;

    List<int> Space;
    public override bool IsUsable()
    {
        return true;
    }

    public override void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        if (unit.Location != cell && !cell.Unit)
        {
            calculate = PathCostManager.GetMovement(unit.unitData.MovementType);
            grid.FindAnyPath(unit.Location, cell, ref calculate, unit.Speed);
            unit.Travel(grid.GetPath());
            grid.ClearPath();
            gui.AbilityCompleted();
        }
    }


    ////public int rep tile

    public override string ToString()
    {
        return "This unit can move. Current status ";
    }

    public override void Canceled()
    {
        HexCell current;
        for (int i = 0; i < Space.Count; i++)
        {
            current = grid.GetCell(Space[i]);
            current.Distance = 0;
            current.DisableHighlight();
        }
        Space.Clear();
    }
}
