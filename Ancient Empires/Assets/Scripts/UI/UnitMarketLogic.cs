using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMarketLogic : MonoBehaviour
{
    public static UnitMarket market;

    int id, cost;

    public void SetUnit(Unit unit)
    {
        id = unit.ID;
        cost = unit.unitData.Cost;
        unitName.text = unit.name;
        unitCost.text = cost.ToString();
        image.sprite = unit.unitData.Icon;
    }

    public void CheckBuyable()
    {
        if (cost < int.MaxValue)
        {
            unitCost.color = Color.green;
        }
    }

    [SerializeField]
    Image image;
    [SerializeField]
    Text unitName, unitCost;

    public void OnClick()
    {
        market.BuyUnit(ref id);
    }
}
