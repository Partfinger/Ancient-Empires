using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AII : IPlayerInterface
{

    AIPlayer bot;

    public Player Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    Unit IPlayerInterface.SelectedUnit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    Ability IPlayerInterface.SelectedAbility { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void HideUnitMarket()
    {
        throw new System.NotImplementedException();
    }

    public void MovementInt(ref Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public void OpenUnitMarket()
    {
        throw new System.NotImplementedException();
    }
}
