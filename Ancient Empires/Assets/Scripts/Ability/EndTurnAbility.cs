using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EndTurn Ability", menuName = "EndTurn Ability", order = 58)]

public class EndTurnAbility : Ability
{
    public override bool IsUsable()
    {
        if (gui.currentCell.Building is HexCastle && gui.currentCell.Unit != gui.selectedUnit)
            return false;
        else
            return true;
    }

    public override void Selected()
    {
        gui.AbilityCompleted();
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
