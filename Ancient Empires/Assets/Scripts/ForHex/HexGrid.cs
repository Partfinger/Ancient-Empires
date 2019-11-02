﻿using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    public int cellCountX, cellCountZ;

    public HexGameUI gui;

    public HexCell cellPrefab;
	public Text cellLabelPrefab;
	public HexGridChunk chunkPrefab;

	public Texture2D noiseSource;

    HexCell currentPathFrom, currentPathTo;
    bool currentPathExists;
    HexCellShaderData cellShaderData;

    public int seed;
    int searchFrontierPhase;

    HexGridChunk[] chunks;
    int chunkCountX, chunkCountZ;
	HexCell[] cells;

    HexCellPriorityQueue searchFrontier;

    public bool HasPath
    {
        get
        {
            return currentPathExists;
        }
    }

    public void Initialize(MapManager manager)
    {
        CreateMap(manager.X, manager.Z);
        if (!cellShaderData)
        {
            cellShaderData = gameObject.AddComponent<HexCellShaderData>();
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(manager.toData)))
        {
            Load(reader);
        }
        HexMapCamera.ValidatePosition();
    }

    public void Initialize()
    {
        Application.targetFrameRate = 60;
        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);
        cellShaderData = gameObject.AddComponent<HexCellShaderData>();
        CreateMap(cellCountX, cellCountZ);
    }

    public void CreateMap(int x, int z)
    {

        if (
            x <= 0 || x % HexMetrics.chunkSizeX != 0 ||
            z <= 0 || z % HexMetrics.chunkSizeZ != 0
        )
        {
            Debug.LogError("Unsupported map size.");
            return;
        }

        ClearPath();
        ClearUnits();
        if (chunks != null)
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                Destroy(chunks[i].gameObject);
            }
        }

        cellCountX = x;
        cellCountZ = z;

        chunkCountX = cellCountX / HexMetrics.chunkSizeX;
        chunkCountZ = cellCountZ / HexMetrics.chunkSizeZ;

        cellShaderData.Initialize(cellCountX, cellCountZ);

        CreateChunks();
        CreateCells();
    }

	void CreateChunks () {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) {
			for (int x = 0; x < chunkCountX; x++) {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCells () {
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void OnEnable () {
		if (!HexMetrics.noiseSource) {
			HexMetrics.noiseSource = noiseSource;
			HexMetrics.InitializeHashGrid(seed);
        }
	}

	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index =
			coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}

	public HexCell GetCell (HexCoordinates coordinates) {
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ) {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX) {
			return null;
		}
		return cells[x + z * cellCountX];
	}

    public HexCell GetCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return GetCell(hit.point);
        }
        return null;
    }

    public HexCell GetCell(int index)
    {
        return cells[index];
    }

    public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.ShaderData = cellShaderData;
        cell.Index = i;

        if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	void AddCellToChunk (int x, int z, HexCell cell) {
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}

    public void FindAnyPath(HexCell fromCell, HexCell toCell, ref PathCostManager.Calculate calculate, int speed)
    {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = SearchAny(ref fromCell, ref toCell, ref calculate, ref speed);
        if (currentPathExists)
        {
            ShowPath(speed);
        }
    }

    public List<HexCell> GetPath()
    {
        if (!currentPathExists)
        {
            return null;
        }
        List<HexCell> path = ListPool<HexCell>.Get();
        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            path.Add(c);
        }
        path.Add(currentPathFrom);
        path.Reverse();
        return path;
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

    void ClearUnits()
    {

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

    public int CellCount
    {
        get
        {
            return cells.Length;
        }
    }

    bool SearchAny(ref HexCell fromCell, ref HexCell toCell, ref PathCostManager.Calculate calculate, ref int speed)
    {
        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);
        while (searchFrontier.Count > 0)
        {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
            }

            int currentTurn = current.Distance - 1 / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                int moveCost = calculate(ref current, ref d, ref searchFrontierPhase, out HexCell neighbor);
                if (moveCost == -1) continue;

                int distance = current.Distance + moveCost;
                int turn = distance / speed + 1;
                if (turn > currentTurn)
                {
                    distance = turn * speed + moveCost;
                }

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic =
                        neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }
    }

    public void OldLoad(BinaryReader reader)
    {
        ClearPath();
        ClearUnits();
        CreateMap(reader.ReadInt32(), reader.ReadInt32());
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
        }
        int unitsCount = reader.ReadInt32();
        /*
        for (int i = 0; i < unitsCount; i++)
        {
            Unit unit = Instantiate(unitsArray.GetUnit(reader.ReadInt32()));
            unit.Load(reader, this);
            unit.transform.SetParent(transform, false);
            units.Add(unit);
        }*/
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Refresh();
        }
    }

    public void Load(BinaryReader reader)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
        }
        
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Refresh();
        }
    }
}