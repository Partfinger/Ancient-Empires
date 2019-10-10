using UnityEngine;
using UnityEngine.UI;

public class SaveLoadItem : MonoBehaviour {

	public ILevelSelector menu;

	public string MapName {
		get {
			return mapName;
		}
		set {
			mapName = value;
			transform.GetChild(0).GetComponent<Text>().text = value;
		}
	}

	string mapName;

	public void Select () {
		menu.SelectItem(mapName);
	}
}