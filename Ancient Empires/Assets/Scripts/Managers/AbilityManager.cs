﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : Manager
{
    public List<Ability> GetAbilitiesForCell(HexCell cell, bool u, IPlayerInterface @interface, ref List<Ability> abilities)
    {
        if (u)
            cell.Unit.GetUsableAbilities(ref abilities);
        if (cell.Building)
            cell.Building.GetAbility(@interface.Player, ref abilities);
        return abilities;
    }

    public void GetAbilitiesAfterTravel(Unit unit, ref List<Ability> abilities)
    {
        if (unit)
            unit.GetUsableAbilitiesAfterMove(ref abilities);
    }
}
