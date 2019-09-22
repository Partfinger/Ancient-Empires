using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType : byte
{
    endTurn, movement,
}

public class AbilityManager : Manager
{
    [SerializeField]
    Ability[] array;

    public Ability GetUnit(int n)
    {
        return array[n];
    }

    public int Length
    {
        get
        {
            return array.Length;
        }
    }
}
