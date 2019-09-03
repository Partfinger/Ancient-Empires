using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : MonoBehaviour
{
    [SerializeField]
    protected bool isCaption;

    [SerializeField]
    protected int owner;

    [SerializeField]
    protected Buff buff;

    public HexCell cell;

    public bool IsCaption { get { return IsCaption; } }

    public int Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public Buff Buff => buff;

    public abstract UnitActions GetUnitActions(ref Unit unit);

    public abstract UserAction GetUserAction();
}
