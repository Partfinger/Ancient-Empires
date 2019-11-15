using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    medium, light, heavy, flying, amphibian, aquatic, special
}

[CreateAssetMenu(fileName = "New UnitData", menuName = "Unit Data", order = 51)]
public class UnitData : ScriptableObject
{
    [SerializeField]
    protected string[] names;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected Sprite icon;
    [SerializeField]
    protected int cost, baseOffenceMin, baseOffenceMax, baseDefence, baseMobility;
    [SerializeField]
    MovementType movementType;

    public string Name(int id)
    {
        return names[id];
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

    public int BaseOffenceMin
    {
        get
        {
            return baseOffenceMin;
        }
    }

    public int BaseOffenceMax
    {
        get
        {
            return baseOffenceMax;
        }
    }

    public int BaseDefence
    {
        get
        {
            return baseDefence;
        }
    }

    public int BaseMobility
    {
        get
        {
            return baseMobility;
        }
    }

    public MovementType MovementType
    {
        get
        {
            return movementType;
        }
    }
}
