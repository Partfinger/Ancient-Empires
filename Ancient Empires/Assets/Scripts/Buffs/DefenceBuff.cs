using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBuff : Buff
{
    public override void Apply()
    {
        unit.BonusDefence += buff;
    }

    public override void Cancel()
    {
        unit.BonusDefence -= buff;
    }
}
