using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class HexagonGrid<T> : BaseHexagonGrid where T : BaseMapTile
{

    public abstract T[,] GridData { get; }

    public Maybe<T> GetWorldTileFromPosition(Vector3 pos)
    {
        return GetIndexFromPosition(pos).ApplyValueToFunction(DataFromIndex);
    }

    public override BaseMapTile[,] BaseMapData => GridData;

    public new T DataFromIndex(Vector2Int index) => GridData[index.x, index.y];

    public T DataFromIndex(int x, int y) => GridData[x, y];


    public IEnumerable<T> GetAdjencentTiles(Vector2Int coord, bool includeCenter = false)
    {
        if (includeCenter)
            yield return DataFromIndex(coord);

        foreach (T tile in MapTilesFromIndices(GetInBoundsNeighbours(coord)))
            yield return tile;
    }

    public IEnumerable<T> MapTilesFromIndices(IEnumerable<Vector2Int> indices)
    {
        return indices.Select(DataFromIndex);
    }


}
