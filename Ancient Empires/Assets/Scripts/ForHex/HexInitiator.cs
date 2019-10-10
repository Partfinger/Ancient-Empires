using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInitiator : MonoBehaviour
{
    public HexGrid grid;
    public HexGameUI ui;
    private void Start()
    {
        if (BatchData.manager == null)
            grid.Initialize();
        else
        {
            grid.Initialize(BatchData.manager);
            ui.Player = BatchData.players[0];
        }
        Destroy(gameObject);
    }
}
