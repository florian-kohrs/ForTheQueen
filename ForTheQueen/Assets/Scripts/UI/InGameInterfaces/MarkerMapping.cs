using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMapping
{

    public Dictionary<MapTile, GameObject> markerMapping = new Dictionary<MapTile, GameObject>();

    public List<MapTile> newTiles = new List<MapTile>();

    public void Add(MapTile tile, GameObject g)
    {
        markerMapping[tile] = g;
    }

    public void ClearMarkers()
    {
        foreach (var kv in markerMapping)
        {
            kv.Key.ClearMarker(kv.Value);   
        }
    }

}
