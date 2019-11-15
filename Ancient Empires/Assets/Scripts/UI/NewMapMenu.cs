using UnityEngine;
using UnityEngine.UI;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;
    public HexMapEditor editor;
    [SerializeField]
    Text text1 = null, text2 = null;

	public void Open () {
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

    public void CreateMap()
    {
        editor.ActiveMap = new MapManager();
        editor.ActiveMap.X = int.Parse(text1.text);
        editor.ActiveMap.Z = int.Parse(text2.text);
        CreateMap(editor.ActiveMap.X, editor.ActiveMap.Z);
    }

	void CreateMap (int x, int z) {
		hexGrid.CreateMap(x, z);
		HexMapCamera.ValidatePosition();
		Close();
	}
}