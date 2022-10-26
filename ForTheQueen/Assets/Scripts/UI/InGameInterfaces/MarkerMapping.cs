using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMapping
{

    public Dictionary<BaseMapTile, GameObject> markerMapping = new Dictionary<BaseMapTile, GameObject>();

    public List<BaseMapTile> newTiles = new List<BaseMapTile>();

    public void Add(BaseMapTile tile, GameObject g)
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
