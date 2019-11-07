﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Capture Ability", menuName = "Castle Capture Ability", order = 53)]
public class CastleCapture : UnitAbility
{
    public override bool IsUsable(ref Unit unit)
    {
        HexCastle castle = unit.Location.Building as HexCastle;
        if (castle)
        {
            if (castle.Owner)
                return !unit.Owner.IsAlly(castle.Owner);
            else
                return true;
        }
        return false;
    }

    public override void Selected(ref Unit unit)
    {
        (unit.Location.Building as HexCastle).Owner = unit.Owner;
        unit.enabled = true;
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
