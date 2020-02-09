using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGameGrid : HexGrid
{
    public HexGameUI gui;
    HexCell currentPathFrom, currentPathTo;
    bool currentPathExists;
    int searchFrontierPhase;

    public bool HasPath
    {
        get
        {
            return currentPathExists;
        }
    }

    void ShowPath(int speed)
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = (current.Distance - 1) / speed;
                current.SetLabel(turn.ToString());
                current.EnableHighlight(Color.white);
                current = current.PathFrom;
            }
        }
        currentPathFrom.EnableHighlight(Color.blue);
        currentPathTo.EnableHighlight(Color.red);
    }

    public void ClearPath()
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.DisableHighlight();
                current.Distance = 0;
                current = current.PathFrom;
            }
            current.DisableHighlight();
            currentPathExists = false;
        }
        currentPathFrom = currentPathTo = null;
    }
}
