using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AII : IPlayerInterface
{

    AIPlayer bot;

    Unit IPlayerInterface.SelectedUnit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    Ability IPlayerInterface.SelectedAbility { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void MovementInt(ref Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
