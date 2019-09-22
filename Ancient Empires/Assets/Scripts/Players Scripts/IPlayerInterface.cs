using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInterface
{
    Unit SelectedUnit { get; set; }

    Ability SelectedAbility { get; set; }

    void MovementInt(ref Unit unit);
}
