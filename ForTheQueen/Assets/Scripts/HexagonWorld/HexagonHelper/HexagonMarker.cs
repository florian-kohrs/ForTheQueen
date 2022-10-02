using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMarker : MonoBehaviour
{

    public GameObject defaultMarkerPrefab;

    protected GameObject currentMarkerPrefab;

    protected List<GameObject> activeMarkers = new List<GameObject>();

    public List<GameObject> MarkHexagons(IEnumerable<MapTile> tiles)
    {
        return MarkHexagons(tiles, defaultMarkerPrefab);
    }

    public List<GameObject> MarkHexagons(IEnumerable<MapTile> tiles, GameObject markerPrefab)
    {
        currentMarkerPrefab = markerPrefab;
        if (currentMarkerPrefab == null)
            currentMarkerPrefab = defaultMarkerPrefab;

        DeleteOldMarks();

        foreach (var h in tiles)
        {
            SpawnMarkerAt(h.CenterPos);
        }
        return activeMarkers;
    }

    protected void SpawnMarkerAt(Vector3 pos)
    {
        GameObject marker = Instantiate(currentMarkerPrefab, transform);
        marker.transform.position = pos + Vector3.up * 0.05f;
        activeMarkers.Add(marker);
    }

    public void DeleteOldMarks()
    {
        foreach (var m in activeMarkers)
        {
            Destroy(m);
        }
        activeMarkers.Clear();
    }

}
