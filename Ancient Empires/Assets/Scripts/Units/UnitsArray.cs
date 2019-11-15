using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitsArray", menuName = "Unit Array", order = 52)]
public class UnitsArray : ScriptableObject
{
    [SerializeField]
    Unit[] array;

    public Unit GetUnit(int n)
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
