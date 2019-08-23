using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFeature : MonoBehaviour, IHexFeature
{
    [SerializeField]
    Buff buff;

    public Buff Buff { get { return buff; } }

    public void Click()
    {
        throw new System.NotImplementedException();
    }

    public void UnitAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
