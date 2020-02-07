using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EndTurn Ability", menuName = "EndTurn Ability", order = 58)]

public class EndTurnAbility : UnitAbility
{
    public override bool IsUsable(Unit unit)
    {
        return unit.Location.Unit == unit;
    }

    public override bool Selected(Unit unit)
    {
        return true;
    }
}
