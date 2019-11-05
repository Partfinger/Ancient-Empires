using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HexInitiator : MonoBehaviour
{
    public HexGameGrid grid;
    public HexGameUI ui;
    private void Start()
    {
        if (Partie.manager == null)
            grid.Initialize();
        else
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Partie.manager.toData)))
            {
                grid.Initialize(Partie.manager, reader);
            }
            ui.Player = Partie.players[0];
        }
        Destroy(gameObject);
    }
}
