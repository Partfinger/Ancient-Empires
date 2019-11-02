using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEditor;

public class SaveLoadMenu : MonoBehaviour, ILevelSelector
{


    public Text menuLabel, actionButtonLabel;

    public HexMapEditor editor;

	public InputField nameInput;

	public RectTransform listContent;

	public SaveLoadItem itemPrefab;

	public HexGrid hexGrid;

    public HexMiniMapGenerator miniMapGenerator;

    bool saveMode;

    public delegate void MethodAction();

    public MethodAction method;

    [SerializeField]
    DialogMenu dialogMenu;

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
            if (File.Exists(editor.ActiveMap.UpdateToHead()))
            {
                dialogMenu.Show(0);
                method = Save;
            }
            else
            {
                Save();
            }
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

    public void Delete()
    {
        dialogMenu.Show(1);
        method = DeleteMap;
    }


    public void DeleteMap () {
        string mapName = nameInput.text;
        string pathData = Path.Combine(Application.persistentDataPath, $"Levels/LevelsData/{mapName}.bin");
        string pathJSON = Path.Combine(Application.persistentDataPath, $"Levels/{mapName}.mapd");
        string pathImage = Path.Combine(Application.persistentDataPath, $"Levels/LevelsMinimap/{mapName}.png");
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
			Directory.GetFiles(MapManager.path, "*.mapd");
		Array.Sort(paths);
		for (int i = 0; i < paths.Length; i++) {
			SaveLoadItem item = Instantiate(itemPrefab);
			item.menu = this;
			item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
			item.transform.SetParent(listContent, false);
		}
	}

    void Save () {

        editor.ActiveMap.UpdateLinks();
        miniMapGenerator.Generate(editor.ActiveMap.toMini);

        using (
			BinaryWriter writer =
			new BinaryWriter(File.Open(editor.ActiveMap.toData, FileMode.Create))
		) {
			hexGrid.Save(writer);
            editor.stuffEditor.Save(writer);

        }
        editor.ActiveMap.Save();
	}
    
    void Load(string mapName)
    {
        MapManager NewMapManager = new MapManager(mapName);
        Debug.LogWarning("Map var " + NewMapManager.EditorVer);
        if (NewMapManager.EditorVer == HexMetrics.EditorVers)
        {
            hexGrid.Initialize(NewMapManager);
            editor.ActiveMap = NewMapManager;
        }
        else if (NewMapManager.EditorVer == 4)
        {
            Debug.LogWarning("Old map format");

        }
        else
        {
            Debug.LogWarning("This map format is not supported.");
        }
        /*
        using (BinaryReader reader = new BinaryReader(File.OpenRead(NewMapManager.toData)))
        {
            Debug.LogWarning("Map var " + NewMapManager.EditorVer);
            if (NewMapManager.EditorVer == HexMetrics.EditorVers)
            {
                editor.ActiveMap = NewMapManager;
                hexGrid.Load(reader);
            }
            else
            {
                if (NewMapManager.EditorVer == 4)
                {
                    editor.ActiveMap = NewMapManager;
                    hexGrid.OldLoad(reader);
                    Debug.LogWarning("Old map format");
                }
            }
            HexMapCamera.ValidatePosition();
        }*/
    }
}