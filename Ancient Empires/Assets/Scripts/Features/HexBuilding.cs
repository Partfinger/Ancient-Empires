using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : MonoBehaviour, IHexFeature
{
    [SerializeField]
    protected Buff buff;

    [SerializeField]
    protected bool isCaption;

    [SerializeField]
    protected int owner;

    public Buff Buff { get { return buff; } }
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

    public void Click()
    {
        return;
    }

    public void UnitAction(Unit unit)
    {
        if (true && isCaption)
        {
            
        }
    }
}
