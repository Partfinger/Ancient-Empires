using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityBuff : Buff
{
    public override void Apply()
    {
        unit.BonusMobility += buff;
    }

    public override void Cancel()
    {
        unit.BonusMobility -= buff;
    }
}
