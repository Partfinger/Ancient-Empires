using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit Market", menuName = "Ranged Market", order = 57)]

public class UnitMarket : Ability
{
    public UnitMarketLogic UnitInMarketPrefab;
    public UnitMarketLogic[] units;

    public void _Awake()
    {
        int l = grid.unitsArray.Length;
        UnitMarketLogic.market = this;
        units = new UnitMarketLogic[l];
        if (l > 4)
            gui.MarketPanel.rectTransform.sizeDelta = new Vector2(385, 170);
        else
            gui.MarketPanel.rectTransform.sizeDelta = new Vector2(5 + 95 * l, 170);
        gui.MarketContent.sizeDelta = new Vector2(5 + 95 * l, 140);
        for (int i = 0; i < units.Length; i++)
        {
            units[i] = Instantiate(UnitInMarketPrefab, gui.MarketContent);
            units[i].SetUnit(grid.unitsArray.GetUnit(i));
            units[i].gameObject.transform.localPosition = new Vector3(5 + 95 * i, -5);
        }
    }

    public void BuyUnit(ref int id)
    {
        Canceled();
        //grid.AddBoughtUnit(id, gui.currentCell, 360f);
    }

    public override void Selected()
    {
        gui.MarketPanel.gameObject.SetActive(true);
    }

    public override bool IsUsable()
    {
        return true;
    }

    public override void Canceled()
    {
        gui.MarketPanel.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
