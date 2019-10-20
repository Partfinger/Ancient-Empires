using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour {

    public HexGrid hexGrid;
    public Material terrainMaterial;
    public MapSettingMenu mapSettingMenu;
    public HexMapStuffEditor stuffEditor;

    MapManager activeMap;

    public MapManager ActiveMap
    {
        get
        {
            return activeMap;
        }
        set
        {
            activeMap = value;
            stuffEditor.UpdateOwnerDropdown();
        }
    }

    public UnitsArray unitsArray;

    sbyte activeElevation;
    sbyte activeWaterLevel;
    int activeTerrainTypeIndex = 0;
    byte activeFeatureLevel = 1, activeFeature, activeBuildingLevel = 0, activeBuilding;

    int brushSize;

    bool applyElevation = true;
    bool applyWaterLevel = true;
    bool applyFeature = false;
    bool applyBuilding = false;

    HexCell previousCell, oldCell;

    delegate void _Update();
    _Update currentUpdate;

    enum OptionalToggle {
        Ignore, Yes, No
    }

    OptionalToggle riverMode, roadMode;

    bool isDrag;
    HexDirection dragDirection;

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index - 1;
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

    public void SetApplyElevation(bool toggle) {
        applyElevation = toggle;
    }

    public void SetElevation(float elevation) {
        activeElevation = (sbyte)elevation;
    }

    public void SetApplyWaterLevel(bool toggle) {
        applyWaterLevel = toggle;
    }
    public void SetWaterLevel(float level) {
        activeWaterLevel = (sbyte)level;
    }

    public void SetBrushSize(float size) {
        brushSize = (int)size;
    }

    public void SetRiverMode(int mode) {
        riverMode = (OptionalToggle)mode;
    }

    public void SetRoadMode(int mode) {
        roadMode = (OptionalToggle)mode;
    }

    public void SetEditMode(bool toggle)
    {
        if (toggle)
            currentUpdate = defUpdate;
        else
            currentUpdate = NotEditUpdate;
        //enabled = toggle;
    }

    void Awake()
    {
        terrainMaterial.DisableKeyword("GRID_ON");
        SetEditMode(false);
        MapManager.UpdatePath();
        ActiveMap = new MapManager
        {
            X = hexGrid.cellCountX,
            Z = hexGrid.cellCountZ,
            EditorVer = HexMetrics.EditorVers
        };
        hexGrid.Initialize();
        currentUpdate = NotEditUpdate;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            currentUpdate();
        }
    }

    void defUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
            return;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                DestroyUnit();
            }
            else
            {
                CreateUnit();
            }
            return;
        }
        previousCell = null;
    }

    void NotEditUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            HexCell currentCell = GetCellUnderCursor();
            if (oldCell)
                oldCell.DisableHighlight();
            if (currentCell)
            {
                stuffEditor.SelectedCell = currentCell;
                oldCell = currentCell;
                oldCell.EnableHighlight(Color.red);
            }
        }
    }

    HexCell GetCellUnderCursor()
    {
        return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void HandleInput()
    {
        HexCell currentCell = GetCellUnderCursor();
        if (oldCell)
            oldCell.DisableHighlight();
        if (currentCell)
        {
            if (previousCell && previousCell != currentCell)
            {
                previousCell.DisableHighlight();
                ValidateDrag(currentCell);
            }
            else
            {
                isDrag = false;
            }
            EditCells(currentCell);
            stuffEditor.SelectedCell = currentCell;
            previousCell = currentCell;
            oldCell = currentCell;
            oldCell.EnableHighlight(Color.red);
        }
        else
        {
            previousCell = null;
        }
            
	}

    void CreateUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.Unit)
        {
            /*hexGrid.AddUnit(
                Instantiate(unitsArray.GetUnit(1)), cell, Random.Range(0f, 360f)
            );*/
        }
    }

    void DestroyUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && cell.Unit)
        {
            //hexGrid.RemoveUnit(cell.Unit);
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
            if (activeTerrainTypeIndex > -1)
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
                    cell.ActiveBuilding = (BuildingType)activeBuilding;
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

    public void ShowGrid(bool visible)
    {
        if (visible)
        {
            terrainMaterial.EnableKeyword("GRID_ON");
        }
        else
        {
            terrainMaterial.DisableKeyword("GRID_ON");
        }
    }

}