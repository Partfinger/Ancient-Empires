using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Ability", menuName = "Movement Ability", order = 54)]
public class Movement : Ability
{

    public delegate int CalculatHeuristics(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbour);

    CalculatHeuristics currentMovement;

    List<int> Space;
    public override bool IsUsable(ref HexCell cell)
    {
        return true;
    }

    public override void TriggerAbility(ref Unit unit, ref HexCell cell)
    {
        if (unit.Location != cell)
        {
            currentMovement = defMovement;
            grid.FindAnyPath(unit.Location, cell, ref currentMovement, unit.Speed);
            unit.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }

    public int defMovement(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        HexEdgeType edgeType = current.GetEdgeType(neighbor);
        if (
            neighbor == null ||
            neighbor.SearchPhase > searchFrontierPhase ||
            neighbor.IsUnderwater ||
            edgeType == HexEdgeType.Cliff
        )
        {
            return -1;
        }
        int moveCost = 0;
        if (current.HasRoadThroughEdge(d))
        {
            moveCost += 7;
        }
        else
        {
            moveCost += edgeType == HexEdgeType.Flat ? 10 : 13;
            moveCost += neighbor.IsFeature ? neighbor.FeatureLevel : 0;
        }
        return moveCost;
    }

    public int heavyMovement(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase)
    {
        HexCell neighbor = current.GetNeighbor(d);
        HexEdgeType edgeType = current.GetEdgeType(neighbor);
        if (
            neighbor == null ||
            neighbor.SearchPhase > searchFrontierPhase ||
            neighbor.IsUnderwater ||
            edgeType == HexEdgeType.Cliff
        )
        {
            return -1;
        }
        int moveCost = 0;
        if (current.HasRoadThroughEdge(d))
        {
            moveCost += 8;
        }
        else
        {
            moveCost += edgeType == HexEdgeType.Flat ? 10 : 15;
            moveCost += neighbor.IsFeature ? neighbor.FeatureLevel : 0;
        }
        return moveCost;
    }

    ////public int rep tile

    public override string ToString()
    {
        return "This unit can move. Current status ";
    }

    public override void Canceled()
    {
        HexCell current;
        for (int i = 0; i < Space.Count; i++)
        {
            current = grid.GetCell(Space[i]);
            current.Distance = 0;
            current.DisableHighlight();
        }
        Space.Clear();
    }
}
