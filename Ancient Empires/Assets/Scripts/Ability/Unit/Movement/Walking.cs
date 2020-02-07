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
		
		fromCell.EnableHighlight(Color.blue);
        Frontier.Add(fromCell);
        Space.Add(fromCell.Index);
        int moveCost, distantion, currentDistantion;
        bool wasChecked, isUnit;

        while (Frontier.Count > 0)
        {
            current = Frontier[0];
            current.WasChecked = true;
            currentDistantion = current.Distance;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                neighbour = current.GetNeighbor(d);
                if (neighbour)
                {
                    edgeType = current.GetEdgeType(neighbour);
                    wasChecked = neighbour.WasChecked;
                    distantion = neighbour.Distance;
                    isUnit = neighbour.Unit;
                    if (
                        neighbour.WaterLevel > 1 ||
                        edgeType == HexEdgeType.Cliff ||
                        wasChecked && distantion < currentDistantion ||
                        isUnit && !neighbour.Unit.Owner.IsAlly(unit.Owner)
                    )
                        continue;
                    moveCost = EmptyCost + 1;
                    /*if (edgeType == HexEdgeType.Slope)
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

                    if (wasChecked && distantion < moveCost || moveCost > unit.Mobility)
                        continue;
                    else
                    {
                        neighbour.Distance = moveCost;
                        neighbour.PathFrom = current;
                        if (!wasChecked) // не був прочеканий
                        {
                            Space.Add(neighbour.Index);
                            Frontier.Add(neighbour);
                            if (!isUnit)
                            {
                                neighbour.EnableHighlight(Color.green);
                            }
                        }
                    }*/
                    current.UpdateDistanceLabel();
                    Frontier.RemoveAt(0);
                    Frontier.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                }
            }
        }
        return false;
		/*
        fromCell = unit.Location;
        HexCell current, neighbour;
        HexEdgeType edgeType;
        fromCell.Distance = 0;

        fromCell.EnableHighlight(Color.blue);
        Frontier.Enqueue(fromCell);
        Space.Add(fromCell.Index);
        int moveCost, distantion, currentDistantion;
        bool wasChecked, isUnit;
        while (Frontier.Count > 0)
        {
            current = Frontier.Dequeue();
            current.WasChecked = true;
            currentDistantion = current.Distance;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                neighbour = current.GetNeighbor(d);
                if (neighbour)
                {
                    edgeType = current.GetEdgeType(neighbour);
                    wasChecked = neighbour.WasChecked;
                    distantion = neighbour.Distance;
                    isUnit = neighbour.Unit;
                    if (
                        neighbour.WaterLevel > 1 ||
                        edgeType == HexEdgeType.Cliff ||
                        wasChecked && distantion < currentDistantion ||
                        isUnit && !neighbour.Unit.Owner.IsAlly(unit.Owner)
                    )
                        continue;
                    moveCost = EmptyCost + currentDistantion;
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
                    
                    if (wasChecked && distantion < moveCost || moveCost > unit.Mobility)
                        continue;
                    else
                    {
                        neighbour.Distance = moveCost;
                        neighbour.PathFrom = current;
                        if (!wasChecked) // не був прочеканий
                        {
                            Space.Add(neighbour.Index);
                            Frontier.Enqueue(neighbour);
                            if (!isUnit)
                            {
                                neighbour.EnableHighlight(Color.green);
                            }
                        }
                        else
                        {
                            Frontier.Enqueue(neighbour);
                        }
                    }
                }
            }
        }
        return false;
		*/
    }

    public override bool TriggerAbility(Unit unit, HexCell destination)
    {
        if (destination.WasChecked)
        {
            unit.Travel(GetPath(unit.Location, destination));
            Finish();
            return true;
        }
        else
        {
            Canceled();
            return false;
        }
    }
}
