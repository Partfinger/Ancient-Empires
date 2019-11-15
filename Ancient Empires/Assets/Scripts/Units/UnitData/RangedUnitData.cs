using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Unit", menuName = "Ranged Unit", order = 56)]
public class RangedUnitData : UnitData
{
    [SerializeField]
    int attackDistanceMin, attackDistanceMax;

    public int AttackDistanceMin
    {
        get
        {
            return attackDistanceMin;
        }
    }

    public int AttackDistanceMax
    {
        get
        {
            return attackDistanceMax;
        }
    }
}
