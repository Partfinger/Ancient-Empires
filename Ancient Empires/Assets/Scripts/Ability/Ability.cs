using UnityEngine;
public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public string Desc = "Short description";
    public Sprite Sprite;
    public static HexGrid grid;
    public static HexGameUI gui;

    public abstract bool IsUsable();

    public virtual void Selected()
    {
        return;
    }

    public virtual void Canceled()
    {
        return;
    }

    public virtual void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        return;
    }

    public static Ability GetCastleAbility()
    {
        return new UnitMarket();
    }

    public new abstract string ToString();
}