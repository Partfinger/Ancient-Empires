using UnityEngine;

public class HexFeatureManager : MonoBehaviour {

	public HexFeatureCollection[] hexFeatureCollection, hexBuildingCollections;

    public AbilityType[] cellAbilities;

    public Transform BrigdePrefab;

	Transform container;

    static int[] defence =
    {
        2, 1, 3
    };

	public void Clear () {
		if (container) {
			Destroy(container.gameObject);
		}
		container = new GameObject("Features Container").transform;
		container.SetParent(transform, false);
	}

	public void Apply () {}

	public void AddFeature (ref HexCell cell, Vector3 position) {
		HexHash hash = HexMetrics.SampleHashGrid(position);
		if (hash.a >= cell.FeatureLevel / 6f) {
			return;
		}
        int activeFeature = cell.ActiveFeature - 1;
        Transform instance = Instantiate(hexFeatureCollection[activeFeature].Pick(hash.с));
        cell.buff.AddDef(defence[activeFeature]);
		instance.localPosition = HexMetrics.Perturb(position);
		instance.localRotation = Quaternion.Euler(-90f, 360f * hash.b, 0f);
		instance.SetParent(container, false);
	}

    public void AddAloneBuilding(ref HexCell cell)
    {
        Transform instance = Instantiate(hexBuildingCollections[(int)cell.ActiveBuilding - 1].Pick(cell.BuildingLevel));
        cell.Building = instance.gameObject.GetComponent<HexBuilding>();
        cell.ability = AbilityType.unitMarket;
        cell.Building.AppointCell(ref cell);
        instance.localPosition = HexMetrics.Perturb(cell.Position);
        instance.SetParent(container, false);
    }

    public void AddMoreBuildingLogic(ref HexCell cell, ref int num)
    {
        Transform instance = Instantiate(hexBuildingCollections[(int)cell.ActiveBuilding - 1].Pick(0));
        cell.Building = instance.gameObject.GetComponent<HexBuilding>();
        cell.Building.AppointCell(ref cell);
        cell.ability = AbilityType.unitMarket;

        /*for (int i = container.childCount - num; i < container.childCount; i++)
        {
            container.GetChild(i).GetChild(2).gameObject.re// = ;
            mat = cell.Building.material;
        }*/
        instance.localPosition = HexMetrics.Perturb(cell.Position);
        instance.SetParent(container, false);
    }

    public void AddMoreBuilding(ref HexCell cell, Vector3 position)
    {
        int active = (int)cell.ActiveBuilding - 1;
        Transform instance = Instantiate(hexBuildingCollections[active].Pick(cell.BuildingLevel + 1));
        //cell.buff.AddDef(defence[activeFeature]);
        instance.localPosition = HexMetrics.Perturb(position);
        instance.localRotation = Quaternion.FromToRotation(Vector3.right,cell.Position - instance.localPosition);
        instance.SetParent(container, false);
    }

    public void AddBridge(Vector3 roadCenter1, Vector3 roadCenter2)
    {
        roadCenter1 = HexMetrics.Perturb(roadCenter1);
        roadCenter2 = HexMetrics.Perturb(roadCenter2);
        Transform instance = Instantiate(BrigdePrefab);
        instance.localPosition = (roadCenter1 + roadCenter2) * 0.5f;
        instance.forward = roadCenter2 - roadCenter1;
        float length = Vector3.Distance(roadCenter1, roadCenter2);
        instance.localScale = new Vector3(
            1f, 1f, length * (1f / HexMetrics.bridgeDesignLength)
        );
        instance.SetParent(container, false);
    }
}