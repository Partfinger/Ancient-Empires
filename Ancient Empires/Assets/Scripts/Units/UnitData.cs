using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitData", menuName = "Unit Data", order = 51)]
public class UnitData : ScriptableObject
{
    private int unitName = 0;

    [SerializeField]
    private string[] names;

    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int cost, baseOffenceMin, baseOffenceMax, baseDefence, baseMobility;
    private int offenceMin, offenceMax, defence, mobility, health, experience, nextRankExperience, healthMax = 100, rank = 1;

    public string Name
    {
        get
        {
            return names[unitName];
        }
    }

    public string Desc
    {
        get
        {
            return description;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }
    }

    public int Offence
    {
        get
        {
            return UnityEngine.Random.Range(offenceMin, offenceMax);
        }
    }

    public int Defence
    {
        get
        {
            return defence;
        }
    }

    public int Mobility
    {
        get
        {
            return mobility;
        }
    }

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            rank = value;
            RecountStats();
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int QualitySum
    {
        get { return offenceMin + offenceMax + defence; }
    }

    void UpdateName()
    {
        unitName = (rank >> 1);
    }

    public void CheckNextRank()
    {
        while (experience >= nextRankExperience)
        {
            rank++;
            experience -= nextRankExperience;
            RecountStats();
        }
    }

    public void AwakeUnit()
    {
        health = healthMax * 1;
        RecountStats();
    }

    void RecountStats()
    {
        int bonus = (rank - 1) << 1;

        offenceMin = baseOffenceMin + bonus;
        offenceMax = baseOffenceMax + bonus;
        defence = baseDefence + bonus;
        mobility = baseMobility + (rank / 6);

        if (rank < 11 && (rank & 1) == 0)
            UpdateName();
    }

    void GetNextRankExperience()
    {
        nextRankExperience = QualitySum * 66;
    }

    public void AddExp(int exp)
    {
        experience += exp;
    }

    public bool IsDead
    {
        get
        {
            if (Health == 0)
            {
                return true;
            }
            return false;
        }
    }
}
