using UnityEngine;
public abstract class UnitAbility : Ability
{
    public override bool Selected(IPlayerInterface iplayer)
    {
        throw new System.NotImplementedException();
    }

    public override bool Selected(Unit unit)
    {
        return true;
    }
}