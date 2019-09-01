using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBuilding : HexFeature
{
    [SerializeField]
    protected bool isCaption;

    [SerializeField]
    protected int owner;

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

    public override void Click()
    {
        return;
    }

    public override void UnitAction(Unit unit)
    {
        if (true && isCaption)
        {
            
        }
    }
}
