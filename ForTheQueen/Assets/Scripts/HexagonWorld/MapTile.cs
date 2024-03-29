using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MapTile : BaseMapTile
{

    public MapTile() { }

    public MapTile(Vector2Int coordinates, BaseHexagonGrid grid) : base(coordinates, grid)
    {
    }

    public void OnPlayerUncovered(Hero p)
    {
        occupations.ForEach(occupation => occupation.OnPlayerUncovered(p));
    }

    public void OnPlayerReachedFieldAsTarget(Hero p)
    {
        occupations.ForEach(occupation => occupation.OnPlayerReachedFieldAsTarget(p));
    }

    public void OnPlayerLeftAfterStationary(Hero p)
    {
        occupations.ForEach(occupation => occupation.OnPlayerLeftFieldAfterStationary(p));
    }

    public void OnPlayerEntered(Hero p, MapMovementAnimation mapMovement)
    {
        occupations.ForEach(occupation => occupation.OnHeroEnter(p, mapMovement));
    }

    public void OnMouseStay(Hero p)
    {
        occupations.ForEach(occupation => occupation.OnPlayerMouseHover(p));
    }

    public void OnMouseExit(Hero p)
    {
        occupations.ForEach(occupation => occupation.OnPlayerMouseExit(p));
    }

    public bool IsWater => kingdomOfMapTile == null;

    protected List<ITileOccupation> occupations = new List<ITileOccupation>();


    public IEnumerable<ITileOccupation> Occupations => occupations;

    protected bool canBeCrossed = true;

    public bool CanBeCrossed(bool allowWaterTiles) => canBeCrossed && (!IsWater || allowWaterTiles);


    public bool canBeEntered  = true;

    public override bool CanBeEntered(bool allowWaterTiles) => canBeEntered && (!IsWater || allowWaterTiles);


    public bool discovered = false;

    public bool ContainsTown => occupations.Where(o => o.GetType() == typeof(Town)).Any();

    public bool CanBePermanentlyOccupied => !HasOccupations && canBePermanantlyOccupied;

    public bool HasOccupations => occupations.Count > 0;

    public void RemoveEnemiesFromTile()
    {
        foreach (var item in occupations)
        {
            if (item is IBaseEnemyOccupation enemy)
                enemy.Despawn();
        }
    }


    public Kingdom kingdomOfMapTile;

    public bool canBePermanantlyOccupied = true;

    public void SpawnAllTileOccupations(Transform parent)
    {
        foreach (var item in occupations)
        {
            item.SpawnOccupation(parent);
        }
    }

    public void AddAndSpawnTileOccupation(ITileOccupation occupation)
    {
        AddTileOccupation(occupation);
        occupation.SpawnOccupation(tileMarker.transform.parent);
    }

    public void AddTileOccupation(ITileOccupation occupation)
    {
        occupations.Add(occupation);
        occupation.MapTile = this;
        canBeCrossed &= occupation.CanBeCrossed;
        canBeEntered &= occupation.CanBeEntered;
    }

    public void RemoveTileOccupation(ITileOccupation occupation)
    {
        occupations.Remove(occupation);
        //CanBeCrossed &= occupation.CanBeCrossed;
        //CanBeEntered &= occupation.CanBeEntered;
    }


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
        Vector2 smallStep = VectorExtension.RotateVector(new Vector2(-HexagonWorld.instance.HexRadius, 0), Mathf.Deg2Rad * 60);
        Vector2 largeStep = VectorExtension.RotateVector(new Vector2(-HexagonWorld.instance.HexRadius, 0), Mathf.Deg2Rad * 120);

        Vector3 left = new Vector3(centerX - HexagonWorld.instance.HexRadius, centerY, centerZ);
        ///swap lefts and rights to be correct. wrong rotate method?
        Vector3 topLeft = new Vector3(centerX - smallStep.x, centerY, centerZ - smallStep.y);
        Vector3 topRight = new Vector3(centerX - largeStep.x, centerY, centerZ - largeStep.y);

        Vector3 right = new Vector3(centerX + HexagonWorld.instance.HexRadius, centerY, centerZ);
        Vector3 botRight = new Vector3(centerX + smallStep.x, centerY, centerZ + smallStep.y);
        Vector3 botLeft = new Vector3(centerX + largeStep.x, centerY, centerZ + largeStep.y);

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
        Color c = kingdomOfMapTile.KingdomBiom.color;
        //float colorRandomRange = 0.045f;
        //c.r += Random.Range(-colorRandomRange, colorRandomRange) * c.r;
        //c.g += Random.Range(-colorRandomRange, colorRandomRange) * c.g;
        //c.b += Random.Range(-colorRandomRange, colorRandomRange) * c.b;
        for (int i = 0; i < 6; i++)
        {
            colors.Add(c);
        }
    }

    public override bool IsValidMovementTarget(bool allowWater)
    {
        return CanBeEntered(allowWater);
    }
}
