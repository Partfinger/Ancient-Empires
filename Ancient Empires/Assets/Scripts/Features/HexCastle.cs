using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCastle : HexBuilding
{

    private void Awake()
    {
        buff = Buff.Castle();
    }

    public override UnitActions GetUnitActions(ref Unit unit)
    {
        // if unit.type == commander && (this.owner != unit.owner && !IsAllies(this.owner) && isCaption) them return UnitActions.capture;
        return 0;
    }

    public override UserAction GetUserAction(/*Player p*/)
    {
        // if this.owner == p.id them return UserAction.BuyOnCastle;
        return 0;
    }

}
