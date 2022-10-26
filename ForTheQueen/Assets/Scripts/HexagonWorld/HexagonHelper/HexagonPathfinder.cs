using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPathfinder : INavigatable<Vector2Int, Vector2Int>
{

    protected bool allowWater = false;

    protected BaseHexagonGrid grid;

    protected Vector2Int start;
    protected Vector2Int end;

    protected Func<HexagonPathfinder, Vector2Int, Vector2Int, bool> hasReachedTargetOverride;

    private HexagonPathfinder(BaseHexagonGrid grid, bool allowWater)
    {
        this.grid = grid;
        this.allowWater = allowWater;
    }

    private HexagonPathfinder(BaseHexagonGrid grid, Vector2Int start, Vector2Int end, bool allowWater) : this(grid, allowWater)
    {
        this.start = start;
        this.end = end; 
    }

    public static List<Vector2Int> GetPath(BaseHexagonGrid grid, Vector2Int start, Vector2Int end, bool allowWater, PathAccuracy pathAccuracy = PathAccuracy.Perfect, Func<HexagonPathfinder, Vector2Int, Vector2Int, bool> hasReachedTargetOverride = null)
    {
        HexagonPathfinder pathfinder = new HexagonPathfinder(grid, start, end, allowWater);
        pathfinder.hasReachedTargetOverride = hasReachedTargetOverride;
        return Pathfinder<Vector2Int, Vector2Int>.FindPath(pathfinder, start, end, pathAccuracy);
    }

    public IEnumerable<Vector2Int> GetCircumjacent(Vector2Int field)
    {
        ///if a field cant be crossed (but may still be entered, e.g. enemies on map)
        ///no neighbours will be available for this field
        //if (!grid.BaseMapData[field.x, field.y].CanBeEntered(allowWater))
        //    return Array.Empty<Vector2Int>();

        return GetCircumjacent(field, IsFieldAvailableForPathfinder);
    }

    public static IEnumerable<Vector2Int> GetCircumjacent(Vector2Int field, Predicate<Vector2Int> p)
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

    protected static void AddIfFieldIsAvailable(List<Vector2Int> list, Vector2Int field, Predicate<Vector2Int> p)
    {
        if (p(field))
            list.Add(field);
    }

    public float DistanceToTarget(Vector2Int from, Vector2Int to)
    {
        int x = from.x;
        int bonus = 0;
   

        int xDiff = Mathf.Abs(from.x - to.x);

        if (xDiff % 2 != 0 && (x % 2 == 0 && to.y < from.y || x % 2 == 1 && to.y > from.y))
            bonus = 1;

        int yDiff = Mathf.Max(0, Mathf.Abs(from.y - to.y) - Mathf.FloorToInt(xDiff / 2f) - bonus);
        return xDiff + yDiff;
    }

    public static IEnumerable<Vector2Int> GetNeighboursInDistance(Vector2Int start, int distance)
    {
        return GetNeighboursInDistance(start, distance, _=>true);
    }

    public static IEnumerable<Vector2Int> GetAccessableNeighboursInDistance(BaseHexagonGrid grid, Vector2Int start, int distance, bool allowWater, bool includeStart = true)
    {
        HexagonPathfinder pathfinder = new HexagonPathfinder(grid, allowWater);
        return GetNeighboursInDistance(start, distance, 
            (p)=> grid.IsInBounds(p) && pathfinder.FieldCanBeEntered(p, allowWater), includeStart);
    }

    public static IEnumerable<Vector2Int> GetNeighboursInDistance(Vector2Int start, int distance, Predicate<Vector2Int> p, bool includeStart = true)
    {
        if(includeStart)
            yield return start;
        Stack<Vector2Int> activeIteration = new Stack<Vector2Int>();
        Stack<Vector2Int> nextIteration = new Stack<Vector2Int>();
        nextIteration.Push(start);
        HashSet<Vector2Int> seen = new HashSet<Vector2Int>() { start};
        for (int i = 0; i < distance; i++)
        {
            activeIteration = nextIteration;
            nextIteration = new Stack<Vector2Int>();
            while (activeIteration.Count > 0)
            {
                Vector2Int current = activeIteration.Pop();
                foreach (var item in GetCircumjacent(current, p))
                {
                    if (seen.Contains(item))
                        continue;
                    yield return item;
                    seen.Add(item);
                    nextIteration.Push(item);
                }
            } 
        }
    }

    protected bool IsFieldAvailableForPathfinder(Vector2Int point)
    {
        return grid.IsInBounds(point) && FieldCanBeEntered(point);
    }

    public bool FieldCanBeEntered(Vector2Int point, bool allowWater)
    {
        return grid.DataFromIndex(point).CanBeEntered(allowWater);
    }

    public bool FieldCanBeEntered(Vector2Int point)
    {
        return FieldCanBeEntered(point, allowWater);
    }


    public float DistanceToField(Vector2Int from, Vector2Int to)
    {
        return 1;// World[to.x, to.y].biom.moveSpeedOnTile;
    }

    public bool ReachedTarget(Vector2Int current, Vector2Int destination)
    {
        if(hasReachedTargetOverride == null)
            return current == destination;
        else
            return hasReachedTargetOverride(this, current, destination);
    }

    public bool IsEqual(Vector2Int t1, Vector2Int t2)
    {
        return t1.Equals(t2);
    }


}
