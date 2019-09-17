using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Capture Ability", menuName = "Castle Capture Ability", order = 53)]
public class CastleCapture : Ability
{
    public override bool IsUsable()
    {
        if (gui.currentCell.Building is HexCastle)
        {
            return true;
        }
        return false;
    }

    public override void Selected()
    {
        gui.currentCell.Building.Owner = 0;
        gui.AbilityCompleted();
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
