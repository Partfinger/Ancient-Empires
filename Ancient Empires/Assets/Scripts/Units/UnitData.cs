using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitData", menuName = "Unit Data", order = 51)]
public class UnitData : ScriptableObject
{
    [SerializeField]
    private string[] names;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int cost, baseOffenceMin, baseOffenceMax, baseDefence, baseMobility;

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
}
