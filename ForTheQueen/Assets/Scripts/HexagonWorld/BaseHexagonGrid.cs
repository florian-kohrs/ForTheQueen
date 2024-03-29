using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseHexagonGrid : MonoBehaviourPun
{

    public virtual float SpaceBetweenHexes => 0f;

    public virtual float HexDiameter => 10f;

    public float WorldTotalWidth => Size.x * HexDiameter;

    public float WorldTotalHeight => Size.y * HexDiameter;

    public float HexYSpacing => Mathf.Sin(Mathf.Deg2Rad * 60) * HexDiameter;

    public float HexXSpacing => Mathf.Cos(Mathf.Deg2Rad * 60) * HexDiameter * 1.5f;

    public float HexRadius => HexDiameter / 2;

    public float HexSmallSqrRadius => Mathf.Pow(HexYSpacing / 2, 2);

    public float TotalHexSpace => SpaceBetweenHexes + HexDiameter;

    public abstract Vector2Int Size { get; }

    public abstract BaseMapTile[,] BaseMapData { get; }

    public bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < Size.x && point.y < Size.y;
    }


    protected Vector2Int GetNaiveArrayIndexFromPos(Vector3 pos)
    {
        //pos.x -= HEX_RADIUS;
        //pos.z -= HEX_RADIUS;
        int coordX = (int)(pos.x / (HexXSpacing + SpaceBetweenHexes));

        if (coordX % 2 != 0)
            pos.z -= HexYSpacing / 2;

        int coordY = (int)(pos.z / (HexYSpacing + SpaceBetweenHexes));

        return new Vector2Int(coordX, coordY);
    }


    public Maybe<Vector2Int> GetIndexFromPosition(Vector3 pos)
    {
        Vector2Int guessedIndex = GetNaiveArrayIndexFromPos(pos);
        List<Vector2Int> neighbours = GetInBoundsNeighbours(guessedIndex);
        neighbours.Add(guessedIndex);
        Maybe<Vector2Int> foundIndex = new Maybe<Vector2Int>();
        foreach (var index in neighbours)
        {
            if (IsInBounds(index) && IsPointInsideHexagonSimple(index, pos))
            {
                if (foundIndex.HasValue)
                {
                    Debug.LogWarning("Mouse hovered detected over multiple fields. This should not happen.");
                }
                foundIndex.Value = index;
            }
        }
        return foundIndex;
    }



    /// <summary>
    /// checks if distance is within circle lying inside hexagon
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool IsPointInsideHexagonSimple(Vector2Int coord, Vector3 point)
    {
        Vector3 relativePos = point - MapTile.GetCenterPosForCoord(coord, SpaceBetweenHexes, HexXSpacing, HexYSpacing);
        float sqrDist = Vector3.SqrMagnitude(relativePos);
        return sqrDist < HexSmallSqrRadius;
    }


    public List<Vector2Int> GetInBoundsNeighbours(Vector2Int field)
    {
        return HexagonPathfinder.GetCircumjacent(field, IsInBounds).ToList();
    }


    public Maybe<BaseMapTile> GetBaseWorldTileFromPosition(Vector3 pos) => GetIndexFromPosition(pos).ApplyValueToFunction(DataFromIndex);

    public BaseMapTile DataFromIndex(Vector2Int index) => BaseMapData[index.x, index.y];


    public IEnumerable<BaseMapTile> BaseMapTilesFromIndices(IEnumerable<Vector2Int> indices)
    {
        return indices.Select(DataFromIndex);
    }



}
