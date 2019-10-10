using UnityEngine;
public abstract class UnitAbility : Ability
{
    public override bool IsUsable(ref Unit unit)
    {
        return false;
    }
}