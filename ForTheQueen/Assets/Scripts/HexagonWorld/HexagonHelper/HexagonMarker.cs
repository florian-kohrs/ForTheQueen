using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMarker : MonoBehaviour
{

    public GameObject defaultMarkerPrefab;

    protected GameObject currentMarkerPrefab;

    public List<GameObject> MarkHexagons(IEnumerable<MapTile> tiles)
    {
        return MarkHexagons(tiles, defaultMarkerPrefab);
    }

    public List<GameObject> MarkHexagons(IEnumerable<MapTile> tiles, GameObject markerPrefab)
    {
        List<GameObject> markers = new List<GameObject>();

        currentMarkerPrefab = markerPrefab;
        if (currentMarkerPrefab == null)
            currentMarkerPrefab = defaultMarkerPrefab;

        foreach (var h in tiles)
        {
            markers.Add(SpawnMarkerAt(h.CenterPos));
        }
        return markers;
    }

    protected GameObject SpawnMarkerAt(Vector3 pos)
    {
        GameObject marker = Instantiate(currentMarkerPrefab, transform);
        marker.transform.position = pos + Vector3.up * 0.05f;
        return marker;
    }

}
