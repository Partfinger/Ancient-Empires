using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Offenseless Attack Ability", menuName = "Offenseless Attack Ability", order = 63)]
public class Offenseless : AttackAbility
{
    public override bool IsUsable(Unit unit)
    {
        return false;
    }

    public override void StrikeBack(Unit attaker, Unit victim, AttackType attackType)
    {
        return;
    }
}
