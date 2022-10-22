using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMarker : MonoBehaviour
{

    public static HexagonMarker Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject defaultMarkerPrefab; 

    public GameObject battleParticipantMarker; 

    public GameObject mapMovementMarker; 

    protected GameObject currentMarkerPrefab;

    [SerializeField]
    protected GameObject markerEnvirenment;

    public MarkerMapping MarkHexagons(IEnumerable<Vector2Int> tiles, GameObject markerPrefab, MarkerMapping mapping = null)
    {
        return MarkHexagons(HexagonWorld.instance.MapTilesFromIndices(tiles), markerPrefab, mapping);
    }

    public MarkerMapping MarkHexagons(IEnumerable<MapTile> tiles, GameObject markerPrefab, MarkerMapping mapping = null)
    {
        if(mapping == null)
            mapping = new MarkerMapping();

        mapping.newTiles = new List<MapTile>();

        currentMarkerPrefab = markerPrefab;
        if (currentMarkerPrefab == null)
            currentMarkerPrefab = defaultMarkerPrefab;

        foreach (var t in tiles)
        {
            mapping.Add(t, SpawnMarkerAt(t));
            mapping.newTiles.Add(t);
        }
        return mapping;
    }

    protected GameObject SpawnMarkerAt(MapTile tile)
    {
        GameObject marker = Instantiate(currentMarkerPrefab, markerEnvirenment.transform);
        marker.transform.position = tile.CenterPos + Vector3.up * 0.05f;
        tile.SetCurrentMarkerOnMapTile(marker);
        return marker;
    }

}
