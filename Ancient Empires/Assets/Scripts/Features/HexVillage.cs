using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVillage : HexBuilding
{
    protected override void ApplyBuff()
    {
        cell.buff = Buffs.Castle();
    }
}
