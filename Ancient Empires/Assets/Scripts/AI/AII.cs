using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AII : IPlayerInterface
{

    AIPlayer bot;

    public void SelectedAbility(ref Ability ab)
    {
        ab.Selected();
    }

    public void SelectedUnit(ref Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
