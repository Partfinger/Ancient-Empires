using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public HexCell cell;
    public static HexGrid grid;

    public int Health { get { return health; } }

    [SerializeField]
    int BaseOffenceMin, BaseOffenceMax, BaseDefence, BaseMobility, HealthMax;
    public int BonusOffence, BonusDefence, BonusMobility;
    int OffenceMin, OffenceMax, defence, Mobility;

    int health;
    int experience = 0;
    int nextRankExperience;
    protected byte rank = 1;

    private void Awake()
    {
        health = HealthMax * 1;
        RecountStats();
    }

    public int Offence
    {
        get
        {
            return Random.Range(OffenceMin, OffenceMax) + BonusOffence;
        }
    }

    public int Defence
    {
        get
        {
            return defence + BonusDefence;
        }
    }

    public int QualitySum
    {
        get { return OffenceMin + OffenceMax + defence; }
    }

    public void GetMotionSpace()
    {
        int mobility = Mobility + BonusMobility;


    }

    public void Move(HexCell[] path)
    {

    }

    void CheckNextRank()
    {
        if (experience > nextRankExperience)
        {
            SetRank(rank++);
        }
    }

    public void SetRank(byte newRank)
    {
        rank = newRank;
        experience -= nextRankExperience;
        RecountStats();
    }

    void RecountStats()
    {
        int bonus = rank << 1;

        OffenceMin = BaseOffenceMin + bonus;
        OffenceMax = BaseOffenceMin + bonus;
        defence = BaseDefence + bonus;
        Mobility = BaseMobility + (rank / 6);

        nextRankExperience = GetNextRankExperience();
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
        experience += victim.QualitySum * hit;
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
        health -= hit;
    }

    public void Cure(int hp)
    {
        int delta = HealthMax - health;
        if (delta < hp)
        {
            health += delta;
        }
        else
        {
            health += hp;
        }
    }

    public abstract void Battle(Unit attacking, Unit victim);

    public abstract HexCell[] GetAttackCells();
}
