using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitActions
{
    empty, finish, move, attack, mend, capture
}

public abstract class Unit : MonoBehaviour
{
    public HexCell cell;

    /// BaseOffenceMin - 0, BaseOffenceMax - 1, BaseDefence - 2, BaseMobility - 3, HealthMax - 4, OffenceMin - 5,
    /// OffenceMax - 6, defence - 7, Mobility - 8, health -9, experience - 10, nextRankExperience - 11, rank - 12
    [SerializeField]
    int[] stats;
    public Buff buff = new Buff();

    public int Health { get { return stats[9]; } }

    private void Awake()
    {
        stats[9] = stats[4] * 1;
        RecountStats();
    }

    public int Offence
    {
        get
        {
            return Random.Range(stats[5], stats[6]) + buff.stats[0];
        }
    }

    public int Defence
    {
        get
        {
            return stats[7] + buff.stats[1];
        }
    }

    public int QualitySum
    {
        get { return stats[5] + stats[6] + stats[7]; }
    }

    public void GetMotionSpace()
    {
        int mobility = stats[8] + buff.stats[2];


    }

    public void Move(HexCell[] path)
    {

    }

    void CheckNextRank()
    {
        if (stats[10] > stats[11])
        {
            SetRank(stats[12]++);
        }
    }

    public void SetRank(int newRank)
    {
        stats[12] = newRank;
        stats[10] -= stats[11];
        RecountStats();
    }

    void RecountStats()
    {
        int bonus = stats[12] << 1;

        stats[5] = stats[0] + bonus;
        stats[6] = stats[1] + bonus;
        stats[7] = stats[2] + bonus;
        stats[8] = stats[3] + (stats[12] / 6);

        stats[11] = GetNextRankExperience();
    }

    public int GetNextRankExperience()
    {
        return QualitySum * 66;
    }


    public void Attack(Unit victim)
    {
        int hit = (Offence - victim.Defence) * Health / 100;

        if (hit < 0)
        {
            hit = 0;
        }
        else if (hit > victim.Health)
        {
            hit = victim.Health;
        }
        victim.Hit(hit);
        stats[10] += victim.QualitySum * hit;
        CheckNextRank();
    }

    public bool CheckIsDead()
    {
        if (Health == 0)
        {
            Die();
            return true;
        }
        return false;
    }

    void Die()
    {

    }

    public void Hit(int hit)
    {
        stats[9] -= hit;
    }

    public void Cure(int hp)
    {
        int delta = stats[4] - stats[9];
        if (delta < hp)
        {
            stats[9] += delta;
        }
        else
        {
            stats[9] += hp;
        }
    }

    public abstract void Battle(Unit attacker, Unit victim);

    public abstract HexCell[] GetAttackCells();

    public virtual UnitActions Skill()
    {
        return 0;
    }
}
