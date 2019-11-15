using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCostManager
{

    public delegate int Calculate(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbour);

    public static Calculate GetMovement(MovementType movement)
    {
        switch(movement)
        {
            case MovementType.light:
                return Light;
            case MovementType.medium:
                return Medium;
            case MovementType.heavy:
                return Heavy;
            case MovementType.flying:
                return Fly;
            case MovementType.amphibian:
                return Amphibian;
            case MovementType.special:
                return Heavy;
        }
        return null;
    }

    static int Medium(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        if (neighbor)
        {
            HexEdgeType edgeType = current.GetEdgeType(neighbor);
            if (
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
                moveCost += edgeType == HexEdgeType.Flat ? 10 : 14;
                //moveCost += neighbor.IsFeature ? neighbor.FeatureLevel : 0;
            }
            return moveCost;
        }
        else
        {
            return -1;
        }
    }

    static int Heavy(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        if (neighbor)
        {
            HexEdgeType edgeType = current.GetEdgeType(neighbor);
            if (
                neighbor.SearchPhase > searchFrontierPhase ||
                neighbor.IsUnderwater ||
                neighbor.HasRiver ||
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
                //moveCost += neighbor.IsFeature ? neighbor.FeatureLevel : 0;
            }
            return moveCost;
        }
        else
        {
            return -1;
        }
    }

    static int Light(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        if (neighbor)
        {
            HexEdgeType edgeType = current.GetEdgeType(neighbor);
            if (
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
                moveCost += 6;
            }
            else
            {
                moveCost += edgeType == HexEdgeType.Flat ? 10 : 12;
            }
            return moveCost;
        }
        else
        {
            return -1;
        }
    }

    static int Fly(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        if (neighbor)
        {
            HexEdgeType edgeType = current.GetEdgeType(neighbor);
            if (
                neighbor.SearchPhase > searchFrontierPhase
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
                int delta = 0;
                if ((delta = neighbor.Elevation - current.Elevation) != 0)
                {
                    if (delta > 0)
                    {
                        moveCost += 10 + delta * 2;
                    }
                    else
                    {
                        moveCost += 10 + delta * -1;
                    }
                }
                else
                {
                    moveCost += 10;
                }
                moveCost += neighbor.IsUnderwater ? 1 : 0;
            }
            return moveCost;
        }
        else
        {
            return -1;
        }
    }

    static int Amphibian(ref HexCell current, ref HexDirection d, ref int searchFrontierPhase, out HexCell neighbor)
    {
        neighbor = current.GetNeighbor(d);
        if (neighbor)
        {
            HexEdgeType edgeType = current.GetEdgeType(neighbor);
            if (
                neighbor.SearchPhase > searchFrontierPhase ||
                (edgeType == HexEdgeType.Cliff && !neighbor.IsUnderwater)
            )
            {
                return -1;
            }
            int moveCost = 0;
            if (current.HasRoadThroughEdge(d))
            {
                moveCost += 9;
            }
            else
            {
                moveCost += edgeType == HexEdgeType.Flat ? 10 : 12;
            }
            return moveCost;
        }
        else
        {
            return -1;
        }
    }
}
