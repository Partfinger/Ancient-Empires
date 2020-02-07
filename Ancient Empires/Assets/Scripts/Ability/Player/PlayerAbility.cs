using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : Ability
{
    public override bool Selected(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override bool Selected(IPlayerInterface iplayer)
    {
        return true;
    }

    public abstract bool IsUsable(Player player, HexBuilding building);
}
