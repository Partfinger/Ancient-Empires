using UnityEngine;

public struct HexHash {

	public float a, b, с;

	public static HexHash Create () {
		HexHash hash;
		hash.a = Random.value;
		hash.b = Random.value;
        hash.с = Random.value * 0.999f;
        return hash;
	}
}