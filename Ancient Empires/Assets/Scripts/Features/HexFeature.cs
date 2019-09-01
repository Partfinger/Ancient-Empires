using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFeature : MonoBehaviour
{
    [SerializeField]
    Buff buff;

    public Buff Buff { get { return buff; } }

    public virtual void Click()
    {
        throw new System.NotImplementedException();
    }

    public virtual void UnitAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
