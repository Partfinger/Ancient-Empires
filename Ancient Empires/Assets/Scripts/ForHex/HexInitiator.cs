using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInitiator : MonoBehaviour
{
    public HexGrid grid;
    public HexGameUI ui;
    private void Start()
    {
        if (Partie.manager == null)
            grid.Initialize();
        else
        {
            grid.Initialize(Partie.manager);
            ui.Player = Partie.players[0];
        }
        Destroy(gameObject);
    }
}
