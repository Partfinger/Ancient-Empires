using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : UnitAbility 
{

    public enum AttackType : byte { melee, range };

    protected List<int> attackSpace = new List<int>();

    public AttackType attack;

    public override bool TriggerAbility(Unit attacker, HexCell cell)
    {
        bool status = cell.WasChecked;
        if (status)
        {
            Unit victim = cell.Unit;
            Strike(attacker, victim);
            bool live = !victim.IsDead;
            if (live)
                victim.attack.StrikeBack(attacker, victim, attack);

            if (!attacker.IsDead)
                attacker.CheckNextRank();
            if (live)
                victim.CheckNextRank();
        }
        Canceled();
        return status;
    }

    public override void Canceled()
    {
        HexCell current;
        for (int i = 0; i < attackSpace.Count; i++)
        {
            current = grid.GetCell(attackSpace[i]);
            current.DisableHighlight();
            current.WasChecked = false;
        }
        attackSpace.Clear();
    }

    public abstract void StrikeBack(Unit attaker, Unit victim, AttackType attackType);

    public virtual void Strike(Unit attaker, Unit victim)
    {
        int hit = (attaker.Offence - victim.Defence) * attaker.Health / 100;

        if (hit < 0)
        {
            hit = 0;
        }
        else if (hit > victim.Health)
        {
            hit = victim.Health;
        }
        victim.Hit(hit);
        attaker.AddExp(hit * victim.QualitySum);
    }
}
