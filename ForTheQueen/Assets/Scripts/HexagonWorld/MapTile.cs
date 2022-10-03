using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class MapTile
{

    public MapTile() { }

    public MapTile(Vector2Int coordinates)
    {
        this.coordinates = coordinates;
        center = new Vector3(GetXPosForCoord(coordinates), 0, GetZPosForCoord(coordinates));
    }
    
    public void OnPlayerUncovered() { }

    public void OnPlayerEntered() { }

    public void OnPlayerReachedFieldAsTarget() { }

    public void OnMouseStay() { }


    protected Vector2Int coordinates;

    public Vector2Int Coordinates => coordinates;

    [System.NonSerialized]
    public TileBiom biom;

    public int biomIndex;

    public int AdaptedBiomIndex => biomIndex - 1;

    public bool IsWater => biomIndex == 0;

    [SerializeField]
    protected List<ITileOccupation> occupations = new List<ITileOccupation>();

    public void AddAndSpawnTileOccupation(ITileOccupation occupation, Transform parent)
    {
        AddTileOccupation(occupation);
        occupation.SpawnOccupation(parent);
    }

    public void AddTileOccupation(ITileOccupation occupation)
    {
        occupations.Add(occupation);
        occupation.MapTile = this;
        CanBeCrossed &= occupation.CanBeCrossed;
        CanBeEntered &= occupation.CanBeEntered;
    }

    public bool CanBeCrossed { get; private set; } = true;

    public bool CanBeEntered { get; private set; } = true;


    public bool discovered = false;

    public Vector3 CenterPos => center;

    protected Vector3 center;

    protected GameObject tileMarker;

    protected List<Vector3> verts;

    protected int worldTileMeshIndex;

    protected Vector3 Left => verts[worldTileMeshIndex];
    protected Vector3 TopLeft => verts[worldTileMeshIndex + 1];
    protected Vector3 TopRight => verts[worldTileMeshIndex + 2];
    protected Vector3 Right => verts[worldTileMeshIndex + 3];
    protected Vector3 BotLeft => verts[worldTileMeshIndex + 4];
    protected Vector3 BotRight => verts[worldTileMeshIndex + 5];

    public void AddTileToMesh(List<Vector3> verts, List<int> tris, List<Color> colors)
    {
        if (IsWater)
            return;
        AddHexagonTriangles(tris, verts.Count);
        AddHexagonVerticesToVerts(verts);
        AddHexagonColor(colors);
    }

    public void MarkTile(GameObject prefab, Transform parent)
    {
        if (IsWater)
            return;

        tileMarker = GameObject.Instantiate(prefab, parent);
        tileMarker.transform.position = CenterPos;
    }


    protected void AddHexagonVerticesToVerts(List<Vector3> verts)
    {
        Vector3 center = CenterPos;
        float centerX = center.x;
        float centerY = center.y;
        float centerZ = center.z;
        Vector2 smallStep = VectorExtension.RotateVector(new Vector2(-HexagonWorld.HEX_RADIUS, 0), Mathf.Deg2Rad * 60);
        Vector2 largeStep = VectorExtension.RotateVector(new Vector2(-HexagonWorld.HEX_RADIUS, 0), Mathf.Deg2Rad * 120);

        Vector3 left = new Vector3(centerX - HexagonWorld.HEX_RADIUS, centerY, centerZ);
        ///swap lefts and rights to be correct. wrong rotate method?
        Vector3 topLeft = new Vector3(centerX - smallStep.x, centerY, centerZ - smallStep.y);
        Vector3 topRight = new Vector3(centerX - largeStep.x, centerY, centerZ - largeStep.y);

        Vector3 right = new Vector3(centerX + HexagonWorld.HEX_RADIUS, centerY, centerZ);
        Vector3 botRight = new Vector3(centerX + smallStep.x, centerY, centerZ + smallStep.y);
        Vector3 botLeft = new Vector3(centerX + largeStep.x, centerY, centerZ + largeStep.y);

        worldTileMeshIndex = verts.Count;

        verts.Add(left);
        verts.Add(topLeft);
        verts.Add(topRight);
        verts.Add(right);
        verts.Add(botRight);
        verts.Add(botLeft);
    }

    protected void AddHexagonTriangles(List<int> tris, int startIndex)
    {
        tris.Add(startIndex+1);
        tris.Add(startIndex);
        tris.Add(startIndex+2);

        tris.Add(startIndex);
        tris.Add(startIndex + 1);
        tris.Add(startIndex + 5);

        ///center triangle
        tris.Add(startIndex + 4);
        tris.Add(startIndex);
        tris.Add(startIndex + 5);

        ///most right triangle
        tris.Add(startIndex + 1);
        tris.Add(startIndex + 3);
        tris.Add(startIndex + 5);
        //tris.Add(startIndex + 3);
        //tris.Add(startIndex + 2);
        //tris.Add(startIndex + 4);
        //tris.Add(startIndex+5);
        //tris.Add(startIndex+4);
        //tris.Add(startIndex);

        //tris.Add(startIndex);
        //tris.Add(startIndex + 2);
        //tris.Add(startIndex + 4);
    }

    protected void AddHexagonColor(List<Color> colors)
    {
        Color c = HexagonWorld.WORLD_BIOMS[AdaptedBiomIndex].color;
        float colorRandomRange = 0.045f;
        c.r += Random.Range(-colorRandomRange, colorRandomRange) * c.r;
        c.g += Random.Range(-colorRandomRange, colorRandomRange) * c.g;
        c.b += Random.Range(-colorRandomRange, colorRandomRange) * c.b;
        for (int i = 0; i < 6; i++)
        {
            colors.Add(c);
        }
    }

    protected float GetZPosForCoord(Vector2Int coord)
    {
        float anchorY = (HexagonWorld.HEX_Y_SPACING + HexagonWorld.SPACE_BETWEEN_HEXES) * coord.y;

        if (coord.x % 2 != 0)
            anchorY += HexagonWorld.HEX_Y_SPACING / 2;

        return anchorY;
    }

    protected float GetXPosForCoord(Vector2Int coord)
    {
        float anchorX = (HexagonWorld.HEX_X_SPACING + HexagonWorld.SPACE_BETWEEN_HEXES) * coord.x;

        return anchorX;
    }



}
