using UnityEngine;
using UnityEngine.UI;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;
    [SerializeField]
    Text text1, text2;

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
        CreateMap(int.Parse(text1.text), int.Parse(text2.text));
    }

	void CreateMap (int x, int z) {
		hexGrid.CreateMap(x, z);
		HexMapCamera.ValidatePosition();
		Close();
	}
}