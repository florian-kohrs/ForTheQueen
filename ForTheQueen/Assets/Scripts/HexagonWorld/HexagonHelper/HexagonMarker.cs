using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMarker : MonoBehaviour
{

    public GameObject hexagonPrefab;

    protected List<GameObject> activeMarkers = new List<GameObject>();

    public void MarkHexagons(IEnumerable<WorldTile> tiles, bool deleteOldMarks = true)
    {
        if (deleteOldMarks)
            DeleteOldMarks();

        foreach (var h in tiles)
        {
            SpawnMarkerAt(h.CenterPos);
        }
    }

    protected void SpawnMarkerAt(Vector3 pos)
    {
        GameObject marker = Instantiate(hexagonPrefab,transform);
        marker.transform.position = pos + Vector3.up * 0.05f;
        activeMarkers.Add(marker);
    }

    public void DeleteOldMarks()
    {
        foreach (var m in activeMarkers)
        {
            Destroy(m);
        }
    }

}
