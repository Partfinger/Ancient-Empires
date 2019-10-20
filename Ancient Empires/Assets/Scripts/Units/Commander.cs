using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : Unit
{
    public bool IsBuyable { get; private set; } = false;

    public override void Die()
    {
        location.Unit = null;
        location = null;
        Partie.players[Owner].CommanderDie();
        IsBuyable = true;
        gameObject.SetActive(false);
    }
}
