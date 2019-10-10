using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit Market", menuName = "Unit Market", order = 57)]

public class UnitMarket : PlayerAbility
{
    public override void Selected(IPlayerInterface @interface)
    {
        @interface.OpenUnitMarket();
    }

    public override void TriggerAbility(IPlayerInterface @interface, dynamic param1)
    {
        Canceled(@interface);
        @interface.Player.AddBoughtUnit(param1);
    }

    public override bool IsUsable(IPlayerInterface @interface, ref HexCell cell)
    {
        HexCastle castle = cell.Building as HexCastle;
        return castle && castle.Owner == @interface.Player.ID;
    }

    public override void Canceled(IPlayerInterface @interface)
    {
        @interface.HideUnitMarket();
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
