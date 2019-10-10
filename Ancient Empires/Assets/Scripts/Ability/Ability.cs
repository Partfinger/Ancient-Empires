using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public string Desc = "Short description";
    public Sprite Sprite;
    public static HexGrid grid;

    public virtual bool IsUsable(ref Unit unit)
    {
        return false;
    }

    public virtual bool IsUsable(IPlayerInterface @interface, ref HexCell cell)
    {
        return false;
    }

    public virtual void Selected(IPlayerInterface @interface)
    {
        return;
    }

    public virtual void Selected(ref Unit unit)
    {
        return;
    }

    public virtual void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        return;
    }

    public virtual void TriggerAbility(IPlayerInterface @interface, dynamic data)
    {
        return;
    }

    public virtual void Canceled(IPlayerInterface @interface)
    {
        return;
    }

    public virtual void Canceled()
    {
        return;
    }

    public new abstract string ToString();
}
