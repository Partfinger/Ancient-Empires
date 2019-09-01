using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityBuff : Buff
{
    public override void Apply(Unit unit)
    {
        unit.BonusMobility += buff;
    }

    public override void Cancel(Unit unit)
    {
        unit.BonusMobility -= buff;
    }
}
