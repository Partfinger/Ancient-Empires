using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenceBuff : Buff
{
    public override void Apply()
    {
        unit.BonusOffence += buff;
    }

    public override void Cancel()
    {
        unit.BonusOffence -= buff;
    }
}
