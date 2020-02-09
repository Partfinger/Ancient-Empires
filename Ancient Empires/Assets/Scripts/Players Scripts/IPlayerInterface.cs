﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInterface
{
    Player Player { get; set; }

    Unit SelectedUnit { get; set; }

    Ability SelectedAbility { get; set; }

    void SelectUnitAfterMove();

    void OpenUnitMarket();

    void HideUnitMarket();

    void Wait();
}
