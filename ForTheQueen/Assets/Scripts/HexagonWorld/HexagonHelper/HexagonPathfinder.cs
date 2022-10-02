using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPathfinder : MonoBehaviour, INavigatable<Vector2Int, Vector2Int>
{

    public HexagonWorld world;

    protected WorldTile[,] World => world.World;

    public IEnumerable<Vector2Int> GetCircumjacent(Vector2Int field)
    {
        ///if a field cant be crossed (but may still be entered, e.g. enemies on map)
        ///no neighbours will be available for this field
        if (!World[field.x, field.y].CanBeCrossed)
            return Array.Empty<Vector2Int>();

        return GetCircumjacent(field, IsFieldAvailableForPathfinder);
    }

    public IEnumerable<Vector2Int> GetCircumjacent(Vector2Int field, Predicate<Vector2Int> p)
    {
        int x = field.x;
        int y = field.y;
        int sign;
        if (x % 2 == 0)
            sign = -1;
        else
            sign = 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x, y + 1), p);
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x, y - 1), p);
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x + 1, y + sign), p);
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x - 1, y + sign), p);
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x + 1, y), p);
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x - 1, y), p);

        return neighbours.ToArray();
    }

    protected void AddIfFieldIsAvailable(List<Vector2Int> list, Vector2Int field, Predicate<Vector2Int> p)
    {
        if (p(field))
            list.Add(field);
    }

    public float DistanceToTarget(Vector2Int from, Vector2Int to)
    {
        int xDiff = Mathf.Abs(from.x - to.x);
        int yDiff = Mathf.Max(0, Mathf.Abs(from.y - to.y) - xDiff / 2) - (xDiff % 2);
        return xDiff + yDiff;
    }


    protected bool IsFieldAvailableForPathfinder(Vector2Int point)
    {
        return world.IsInBounds(point) && world.FieldCanBeEntered(point);
    }


    public float DistanceToField(Vector2Int from, Vector2Int to)
    {
        return World[to.x, to.y].biom.moveSpeedOnTile;
    }

    public bool ReachedTarget(Vector2Int current, Vector2Int destination)
    {
        return current == destination;
    }

    public bool IsEqual(Vector2Int t1, Vector2Int t2)
    {
        return t1.Equals(t2);
    }


}
