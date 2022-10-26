using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseMapTile
{

    public BaseMapTile() { }

    public BaseMapTile(Vector2Int coordinates, BaseHexagonGrid grid)
    {
        this.coordinates = coordinates;
        center = GetCenterPosForCoord(coordinates, grid);
    }

    protected Serializable3DVector center;

    [NonSerialized]
    protected GameObject tileMarker;

    protected SerializableVector2Int coordinates;

    public SerializableVector2Int Coordinates => coordinates;

    [NonSerialized]
    protected GameObject currentMarkerOnMapTile;

    public GameObject CurrentMarkerOnMapTile => currentMarkerOnMapTile;

    public void ClearMarker(GameObject marker)
    {
        if (currentMarkerOnMapTile == marker && marker != null)
            GameObject.Destroy(currentMarkerOnMapTile);
    }

    public void SetCurrentMarkerOnMapTile(GameObject tile)
    {
        if (currentMarkerOnMapTile != null)
            GameObject.Destroy(currentMarkerOnMapTile);
        currentMarkerOnMapTile = tile;
    }


    public Vector3 CenterPos
    {
        get
        {
            if (center == default)
                center = GetCenterPosForCoord(Coordinates);
            return center;
        }
    }


    public static Vector3 GetCenterPosForCoord(Vector2Int coord, BaseHexagonGrid grid)
    {
        return GetCenterPosForCoord(coord, grid.SpaceBetweenHexes, grid.HexXSpacing, grid.HexYSpacing);
    }

    public static Vector3 GetCenterPosForCoord(Vector2Int coord, float spaceBetweenHexes, float xSpacing, float ySpacing)
    {
        return new Vector3(GetXPosForCoord(coord, spaceBetweenHexes, xSpacing, ySpacing), 0, GetZPosForCoord(coord, spaceBetweenHexes, xSpacing, ySpacing));
    }

    public static Vector3 GetCenterPosForCoord(Vector2Int coord)
    {
        return new Vector3(
            GetXPosForCoord(coord, HexagonWorld.instance.SpaceBetweenHexes, HexagonWorld.instance.HexXSpacing, HexagonWorld.instance.HexYSpacing),
            0,
            GetZPosForCoord(coord, HexagonWorld.instance.SpaceBetweenHexes, HexagonWorld.instance.HexXSpacing, HexagonWorld.instance.HexYSpacing));
    }

    protected static float GetZPosForCoord(Vector2Int coord, float spaceBetweenHexes, float xSpacing, float ySpacing)
    {
        float anchorY = (ySpacing + spaceBetweenHexes) * coord.y;

        if (coord.x % 2 != 0)
            anchorY += ySpacing / 2;

        return anchorY;
    }

    protected static float GetXPosForCoord(Vector2Int coord, float spaceBetweenHexes, float xSpacing, float ySpacing)
    {
        float anchorX = (xSpacing + spaceBetweenHexes) * coord.x;

        return anchorX;
    }



}
