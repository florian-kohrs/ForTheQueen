using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class WorldTile
{

    public WorldTile(Vector2Int coordinate, int biomIndex)
    {
        this.coordinate = coordinate;
        this.biomIndex = biomIndex - 1;
    }

    protected Vector2Int coordinate;

    [System.NonSerialized]
    public TileBiom biom;

    public int biomIndex;

    public List<TileOccupation> occupations;

    public Vector3 CenterPos => new Vector3(GetXPosForCoord(coordinate), 0, GetZPosForCoord(coordinate));

    public void AddTileToMesh(List<Vector3> verts, List<int> tris, List<Color> colors)
    {
        AddHexagonTriangles(tris, verts.Count);
        AddHeaxgonVerticesToVerts(verts);
        AddHexagonColor(colors);
    }


    protected void AddHeaxgonVerticesToVerts(List<Vector3> verts)
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
        float test = HexagonWorld.HEX_X_SPACING;
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
        Color c = HexagonWorld.WORLD_BIOMS[biomIndex].color;
        c.r += Random.Range(-0.1f, 0.1f) * c.r;
        c.g += Random.Range(-0.1f, 0.1f) * c.g;
        c.b += Random.Range(-0.1f, 0.1f) * c.b;
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
