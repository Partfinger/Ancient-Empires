using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenceBuff : Buff
{
    public override void Apply(Unit unit)
    {
        unit.BonusOffence += buff;
    }

    public override void Cancel(Unit unit)
    {
        unit.BonusOffence -= buff;
    }
}
