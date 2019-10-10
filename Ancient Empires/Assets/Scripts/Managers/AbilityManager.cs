using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType : byte
{
    endTurn, movement, meleeAttack, rangeAttack, castleCaptureCommander, unitMarket, villageCapture, castleCaptureSoldier
}

public class AbilityManager : Manager
{
    [SerializeField]
    Ability[] array;

    public List<Ability> GetAbilitiesForCell(IPlayerInterface @interface, ref HexCell cell)
    {
        List<Ability> result = new List<Ability>();
        Unit unit = cell.Unit;
        result.AddRange(ConvertToAbility(unit.GetAbilities(), ref unit));
        if (cell.ability != 0)
        {
            Ability a = array[(byte)cell.ability];
            if (a.IsUsable(@interface, ref cell) )
                result.Add(a);
        }
        return result;
    }

    public List<Ability> GetAbilitiesAfterTravel(IPlayerInterface @interface, ref Unit unit)
    {
        List<Ability> result = new List<Ability>();
        result.AddRange(ConvertToAbility(unit.GetAbilities(), ref unit));
        result.RemoveAt(1);
        return result;
    }

    public Ability GetMovement()
    {
        return array[1];
    }

    List<Ability> ConvertToAbility(AbilityType[] abilityTypes, ref Unit unit)
    {
        List<Ability> res = new List<Ability>();
        Ability current;
        for (int i = 0; i < abilityTypes.Length; i++)
        {
            current = array[(byte)abilityTypes[i]];
            if (current.IsUsable(ref unit))
                res.Add(current);
        }
        return res;
    }

    public void SelectAbility(AbilityType ability, ref Unit unit)
    {
        array[(int)ability].Selected(ref unit);
    }

    public void TriggetAbility(AbilityType ability, ref Unit unit, ref HexCell cell)
    {
        array[(int)ability].TriggerAbility(ref unit, ref cell);
    }

    public bool IsUsable(AbilityType ability, ref Unit unit)
    {
        return array[(int)ability].IsUsable(ref unit);
    }

    public int Length
    {
        get
        {
            return array.Length;
        }
    }
}
