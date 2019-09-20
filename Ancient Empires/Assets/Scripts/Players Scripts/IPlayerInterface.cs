using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInterface
{
    void SelectedAbility(ref Ability ab);

    void SelectedUnit(ref Unit unit);
}
