using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class HexMiniMapGenerator : MonoBehaviour
{
    public Camera camera;
    static int scale = 512;
    public HexGrid grid;

    public void Generate(string path)
    {
        camera.gameObject.SetActive(true);
        Vector3 delta = HexMetrics.Perturb(grid.GetCell(grid.CellCount - 1).transform.position) - HexMetrics.Perturb(grid.GetCell(0).transform.position);
        float max = Mathf.Max(delta.x, delta.z) / 2;
        camera.orthographicSize = max;

        camera.transform.position = delta / 2 + new Vector3(2, 50);
        RenderTexture tempRT = new RenderTexture(scale, scale, 24);
        camera.targetTexture = tempRT;
        camera.Render();

        RenderTexture.active = tempRT;

        Texture2D virtualPhoto = new Texture2D(scale, scale, TextureFormat.RGB24, false);
        virtualPhoto.ReadPixels(new Rect(0, 0, scale, scale), 0, 0);

        RenderTexture.active = null;
        camera.targetTexture = null;

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        camera.gameObject.SetActive(false);
    }

}
