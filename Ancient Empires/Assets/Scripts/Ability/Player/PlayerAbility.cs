using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : Ability
{
    public override bool IsUsable(IPlayerInterface @interface, ref HexCell cell)
    {
        return false;
    }
}
