using UnityEngine;
public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public string Desc = "Short description";
    public Sprite Sprite;
    public static HexGrid grid;

    public abstract bool IsUsable(ref HexCell cell);

    public void Show()
    {

    }

    public virtual void Selected(ref Unit unit)
    {
        return;
    }
    public abstract void Canceled();
    public virtual void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        return;
    }

    public new abstract string ToString();
}