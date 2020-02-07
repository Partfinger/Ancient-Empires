using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Capture Ability", menuName = "Castle Capture Ability", order = 53)]
public class CastleCapture : UnitAbility
{
    public override bool IsUsable(Unit unit)
    {
        HexCastle castle = unit.Location.Building as HexCastle;
        if (castle && castle.IsCaption)
        {
            if (castle.Owner)
                return !unit.Owner.IsAlly(castle.Owner);
            else
                return true;
        }
        return false;
    }

    public override bool Selected(Unit unit)
    {
        (unit.Location.Building as HexCastle).Owner = unit.Owner;
        unit.enabled = true;
        return true;
    }
}
