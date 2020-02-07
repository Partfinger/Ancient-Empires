using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public string Desc = "Short description";
    public Sprite Sprite;
    public static HexGameGrid grid;

    public virtual bool IsUsable(Unit unit)
    {
        return false;
    }

    public abstract bool Selected(Unit unit);

    public abstract bool Selected(IPlayerInterface iplayer);

    public virtual bool TriggerAbility(Unit unit, HexCell cell)
    {
        return true;
    }

    public virtual void Canceled()
    {
        return;
    }

    public new string ToString()
    {
        return Desc;
    }
}
