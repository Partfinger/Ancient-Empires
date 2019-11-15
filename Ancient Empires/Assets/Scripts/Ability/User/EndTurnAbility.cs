using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EndTurn Ability", menuName = "EndTurn Ability", order = 58)]

public class EndTurnAbility : UnitAbility
{
    public override bool IsUsable(ref Unit unit)
    {
        HexCell cell = unit.Location;
        if (cell.Building is HexCastle && (cell.Unit != unit))
            return false;
        else
            return true;
    }

    public override void Selected(ref Unit unit)
    {
        unit.enabled = true;
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
