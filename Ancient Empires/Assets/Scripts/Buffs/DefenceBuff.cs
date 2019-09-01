using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBuff : Buff
{
    public override void Apply(Unit unit)
    {
        unit.BonusDefence += buff;
    }

    public override void Cancel(Unit unit)
    {
        unit.BonusDefence -= buff;
    }
}
