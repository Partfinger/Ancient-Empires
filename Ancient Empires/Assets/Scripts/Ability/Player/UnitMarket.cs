using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit Market", menuName = "Unit Market", order = 57)]

public class UnitMarket : PlayerAbility
{
    public override bool IsUsable(Player player, HexBuilding building)
    {
        if (player == building.Owner)
        {
            return true;
        }
        return false;
    }

    public override bool Selected(IPlayerInterface iplayer)
    {
        iplayer.OpenUnitMarket();
        return true;
    }
}
