using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class HexMapEditor : MonoBehaviour {

	public HexGrid hexGrid;

    sbyte activeElevation;
	sbyte activeWaterLevel;
    byte activeTerrainTypeIndex = 1;
    byte activeFeatureLevel = 1, activeFeature, activeBuildingLevel = 0, activeBuilding;

	int brushSize;

	bool applyElevation = true;
	bool applyWaterLevel = true;
    bool applyFeature = false;
    bool applyBuilding = false;

    enum OptionalToggle {
		Ignore, Yes, No
	}

	OptionalToggle riverMode, roadMode;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = (byte)index;
    }

    public void SetActiveFeature(int num)
    {
        activeFeature = (byte)num;
    }

    public void SetActiveBuilding(int num)
    {
        activeBuilding = (byte)num;
    }

    public void SetApplyFeature(bool toggle)
    {
        applyFeature = toggle;
    }

    public void SetApplyBuilding(bool toggle)
    {
        applyBuilding = toggle;
    }

    public void SetActiveFeatureLevel(float level)
    {
        activeFeatureLevel = (byte)level;
    }

    public void SetActiveBuildingLevel(float level)
    {
        activeBuildingLevel = (byte)level;
    }

    public void SetApplyElevation (bool toggle) {
		applyElevation = toggle;
	}

	public void SetElevation (float elevation) {
		activeElevation = (sbyte)elevation;
	}

	public void SetApplyWaterLevel (bool toggle) {
		applyWaterLevel = toggle;
	}
	public void SetWaterLevel (float level) {
		activeWaterLevel = (sbyte)level;
	}

	public void SetBrushSize (float size) {
		brushSize = (int)size;
	}

	public void SetRiverMode (int mode) {
		riverMode = (OptionalToggle)mode;
	}

	public void SetRoadMode (int mode) {
		roadMode = (OptionalToggle)mode;
	}

	public void ShowUI (bool visible) {
		hexGrid.ShowUI(visible);
	}

	void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
		else {
			previousCell = null;
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			HexCell currentCell = hexGrid.GetCell(hit.point);
			if (previousCell && previousCell != currentCell) {
				ValidateDrag(currentCell);
			}
			else {
				isDrag = false;
			}
			EditCells(currentCell);
			previousCell = currentCell;
		}
		else {
			previousCell = null;
		}
	}

	void ValidateDrag (HexCell currentCell) {
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		) {
			if (previousCell.GetNeighbor(dragDirection) == currentCell) {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + brushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++) {
			for (int x = centerX - brushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

	void EditCell (HexCell cell) {
		if (cell) {
            if (activeTerrainTypeIndex > 0)
            {
                cell.TerrainTypeIndex = activeTerrainTypeIndex;
            }
            if (applyElevation) {
				cell.Elevation = activeElevation;
			}
			if (applyWaterLevel) {
				cell.WaterLevel = activeWaterLevel;
			}
            if (applyBuilding ^ applyFeature)
            {
                if (applyBuilding)
                {
                    cell.BuildingLevel = activeBuildingLevel;
                    cell.ActiveBuilding = activeBuilding;
                    if (cell.IsFeature)
                    {
                        cell.FeatureLevel = 0;
                        cell.ActiveFeature = 0;
                    }
                }
                else if (!cell.IsBuilding)
                {
                    cell.FeatureLevel = activeFeatureLevel;
                    cell.ActiveFeature = activeFeature;
                }
            }
            if (riverMode == OptionalToggle.No) {
				cell.RemoveRiver();
			}
			if (roadMode == OptionalToggle.No) {
				cell.RemoveRoads();
			}
			if (isDrag) {
				HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
				if (otherCell) {
					if (riverMode == OptionalToggle.Yes) {
						otherCell.SetOutgoingRiver(dragDirection);
					}
					if (roadMode == OptionalToggle.Yes) {
						otherCell.AddRoad(dragDirection);
					}
				}
			}
		}
	}

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (
            BinaryWriter writer =
                new BinaryWriter(File.Open(path, FileMode.Create))
        )
        {
            writer.Write(1);
            hexGrid.Save(writer);
        }
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == 1)
            {
                hexGrid.Load(reader);
                HexMapCamera.ValidatePosition();
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
                return;
            }
            hexGrid.Load(reader);
        }
    }

}