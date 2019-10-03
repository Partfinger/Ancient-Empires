using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEditor;

public class SaveLoadMenu : MonoBehaviour {


    public Text menuLabel, actionButtonLabel;

    public HexMapEditor editor;

	public InputField nameInput;

	public RectTransform listContent;

	public SaveLoadItem itemPrefab;

	public HexGrid hexGrid;

    public HexMiniMapGenerator miniMapGenerator;

    bool saveMode;

	public void Open (bool saveMode) {
		this.saveMode = saveMode;
		if (saveMode) {
			menuLabel.text = "Save Map";
			actionButtonLabel.text = "Save";
		}
		else {
			menuLabel.text = "Load Map";
			actionButtonLabel.text = "Load";
		}
		FillList();
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void Action () {
		if (saveMode) {
            string mapName = nameInput.text;
            if (mapName.Length == 0)
            {
                Close();
                return;
            }
            editor.ActiveMap.Name = mapName;
            Save(mapName);
		}
		else {
            string mapName = nameInput.text;
            if (mapName.Length == 0)
            {
                Close();
                return;
            }
            Load(mapName);
		}
		Close();
	}

	public void SelectItem (string name) {
		nameInput.text = name;
	}

	public void Delete () {
        string mapName = nameInput.text;
        string pathData = Path.Combine(Application.persistentDataPath, "Levels/LevelsData/" + mapName + ".bin");
        string pathJSON = Path.Combine(Application.persistentDataPath, "Levels/" + mapName + ".mapd");
        string pathImage = Path.Combine(Application.persistentDataPath, "Levels/LevelsMinimap/" + mapName + ".png");
        if (mapName == "") {
			return;
		}
		if (File.Exists(pathData)) {
			File.Delete(pathData);
		}
        if (File.Exists(pathImage))
        {
            File.Delete(pathImage);
        }
        File.Delete(pathJSON);
        nameInput.text = "";
		FillList();
	}

	void FillList () {
		for (int i = 0; i < listContent.childCount; i++) {
			Destroy(listContent.GetChild(i).gameObject);
		}
		string[] paths =
			Directory.GetFiles(MapManager.GetPath(), "*.mapd");
		Array.Sort(paths);
		for (int i = 0; i < paths.Length; i++) {
			SaveLoadItem item = Instantiate(itemPrefab);
			item.menu = this;
			item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
			item.transform.SetParent(listContent, false);
		}
	}

    void Save (string mapName) {

        string pathData = Path.Combine(Application.persistentDataPath, "Levels/LevelsData/" + mapName + ".bin");
        string pathImage = Path.Combine(Application.persistentDataPath, "Levels/LevelsMinimap/" + mapName + ".png");

        editor.ActiveMap.Save();
        miniMapGenerator.Generate(pathImage);

        using (
			BinaryWriter writer =
			new BinaryWriter(File.Open(pathData, FileMode.Create))
		) {
			hexGrid.Save(writer);
		}
	}
    
    void Load(string mapName)
    {
        MapManager NewMapManager = new MapManager(mapName);
        
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(Application.persistentDataPath, "Levels/LevelsData/" + NewMapManager.Name + ".bin"))))
        {
            if (NewMapManager.EditorVer == HexMetrics.EditorVers)
            {
                Debug.LogWarning("Current map format " + NewMapManager.EditorVer);
                editor.ActiveMap = NewMapManager;
                hexGrid.Load(reader);
            }
            else
            {
                if (NewMapManager.EditorVer == 3)
                {
                    Debug.LogWarning("Old map format " + NewMapManager.EditorVer);
                }
            }
            HexMapCamera.ValidatePosition();
        }
    }

	/*void Load (string path) {
		if (!File.Exists(path)) {
			Debug.LogError("File does not exist " + path);
			return;
		}
		using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
			int header = reader.ReadInt32();
            if (header == HexMetrics.EditorVers)
            {
                Debug.LogWarning("Current map format " + header);
                hexGrid.Load(reader);
                MapManager NewMapManager = new MapManager()
                {
                    Name = "",
                    X = hexGrid.cellCountX,
                    Z = hexGrid.cellCountZ,
                    EditorVer = header
                };
                editor.ActiveMap = NewMapManager;
            }
            else
            {
                if (header == 2)
                {
                    Debug.LogWarning("Old map format " + header);
                    hexGrid.OldLoad(reader);
                }
                else
                {
                    Debug.LogWarning("Unknown map format " + header);
                    return;
                }
            }
            HexMapCamera.ValidatePosition();
        }
	}*/
}