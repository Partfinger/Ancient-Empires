using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Walking", menuName = "Walking Ability", order = 54)]
public class Walking : Movement
{
    public byte RoadCost, RiverCost, SlopeCost, ShoalCost;

    public override bool Selected(Unit unit)
    {
		fromCell = unit.Location;
		HexCell current, neighbour;
		HexEdgeType edgeType;
		fromCell.Distance = 0;
        fromCell.WasChecked = true;

        fromCell.EnableHighlight(Color.blue);
        Frontier.Add(fromCell);
        int moveCost, mobility = unit.Mobility, currentDistantion;
        bool isUnit;

        while (Frontier.Count > 0)
        {
            current = Frontier[0];
            Frontier.RemoveAt(0);
            Space.Add(current.Index);
            currentDistantion = current.Distance;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                neighbour = current.GetNeighbor(d);
                if (neighbour)
                {
                    edgeType = current.GetEdgeType(neighbour);
                    isUnit = neighbour.Unit;
                    if (
                        neighbour.WaterLevel > 1 ||
                        edgeType == HexEdgeType.Cliff ||
                        isUnit && !neighbour.Unit.Owner.IsAlly(unit.Owner)
                    )
                        continue;
                    moveCost = currentDistantion + EmptyCost;

                    if (edgeType == HexEdgeType.Slope)
                        moveCost += SlopeCost;
                    if (current.HasRoadThroughEdge(d))
                        moveCost -= RoadCost;
                    else
                    {
                        if (current.HasRiverThroughEdge(d))
                        {
                            moveCost += RiverCost;
                        }
                        if (neighbour.IsUnderwater)
                        {
                            moveCost += ShoalCost;
                        }
                    }

                    if (moveCost < neighbour.Distance)
                    {

                    }

                    if (mobility < moveCost)
                    {
                        continue;
                    }

                    if (!neighbour.WasChecked)
                    {
                        neighbour.Distance = moveCost;
                        Frontier.Add(neighbour);
                        neighbour.PathFrom = current;
                        neighbour.EnableHighlight(Color.green);
                        neighbour.WasChecked = true;
                    }
                    else if (moveCost < neighbour.Distance)
                    {
                        neighbour.Distance = moveCost;
                        neighbour.PathFrom = current;
                    }
                    Frontier.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                }
            }
        }
        return false;
    }

    public override bool Trigger(ref Unit unit, ref HexCell destination)
    {
        if (destination && destination.WasChecked)
        {
            if (unit.Location != destination)
                unit.Travel(GetPath(unit.Location, destination));
            else
                unit.Owner.ui.SelectUnitAfterMove();
            Finish();
            return true;
        }
        return false;
    }
}
