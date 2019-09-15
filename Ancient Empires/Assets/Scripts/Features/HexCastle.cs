using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCastle : HexBuilding
{
    protected override void ApplyBuff()
    {
        cell.buff = Buffs.Castle();
    }

    public override int GetUserAction(/*Player p*/)
    {
        // if this.owner == p.id them return UserAction.BuyOnCastle;
        return 0;
    }

}
