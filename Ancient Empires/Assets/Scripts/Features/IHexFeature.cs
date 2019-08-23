using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHexFeature
{
    Buff Buff { get; }
    void UnitAction(Unit unit);
    void Click();
}
