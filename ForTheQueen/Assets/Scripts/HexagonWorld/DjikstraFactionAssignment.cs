using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DjikstraFactionAssignment<T>
{

    protected static Queue<Vector2Int> candidates;

    protected static Vector2Int currentContinentSize;

    protected static T[,] currentMap;

    protected static HashSet<Vector2Int> visitedPoints;

    protected static Predicate<Vector2Int> isFieldAvailable;

    public static void BuildDjikstraOnMap(T[,] map, Tuple<T, Vector2Int> startPoint, Predicate<Vector2Int> isFieldAvailable = null)
    {
        BuildDjikstraOnMap(map, new List<Tuple<T, Vector2Int>>() { startPoint }, isFieldAvailable);
    }

    public static void BuildDjikstraOnMap(T[,] map, IEnumerable<Tuple<T, Vector2Int>> startPoints, Predicate<Vector2Int> isFieldAvailable = null)
    {
        DjikstraFactionAssignment<T>.isFieldAvailable = isFieldAvailable;
        candidates = new Queue<Vector2Int>();
        visitedPoints = new HashSet<Vector2Int>();
        currentMap = map;
        InitializeDjisktra(startPoints);
        while(candidates.Count > 0)
        {
            Vector2Int next = candidates.Dequeue();
            T currentFaction = map[next.x, next.y];
            foreach(Vector2Int neighbour in GetNeighboursOf(next))
            {
                visitedPoints.Add(neighbour);
                candidates.Enqueue(neighbour);
                map[neighbour.x, neighbour.y] = currentFaction;
            }
        }
    }
     
    protected static void InitializeDjisktra(IEnumerable<Tuple<T, Vector2Int>> startPoints)
    {
        int widthIndex = currentMap.GetLength(0) - 1;
        int heightIndex = currentMap.GetLength(1) - 1;
        currentContinentSize = new Vector2Int(widthIndex + 1, heightIndex + 1);
        foreach (var item in startPoints)
        {
            AddStartPoint(item.Item2.x, item.Item2.y, item.Item1);
        }
    }

    protected static void AddStartPoint(int x, int y, T index)
    {
        currentMap[x, y] = index;
        candidates.Enqueue(new Vector2Int(x, y));
        visitedPoints.Add(new Vector2Int(x, y));
    }

    protected static IEnumerable<Vector2Int> GetNeighboursOf(Vector2Int p)
    {
        int sign = (int)((p.x % 2) - 0.5f) * 2;

        List<Vector2Int> result = new List<Vector2Int>(){
            new Vector2Int(p.x,p.y + 1),
            new Vector2Int(p.x,p.y - 1),
            new Vector2Int(p.x + 1,p.y + sign),
            new Vector2Int(p.x - 1,p.y + sign),
            new Vector2Int(p.x + 1, p.y),
            new Vector2Int(p.x - 1, p.y),
        };

        return result.Where(IsNeighbourFieldValid);
    }

    protected static bool IsNeighbourFieldValid(Vector2Int pos)
    {
        return IsInBound(pos) && !VisitedPoint(pos) && IsFieldAvailable(pos);
    }

    protected static bool IsFieldAvailable(Vector2Int pos)
    {
        return isFieldAvailable == null || isFieldAvailable(pos);
    }

    protected static bool IsInBound(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < currentContinentSize.x &&
            pos.y >= 0 && pos.y < currentContinentSize.y;
    }

    protected static bool VisitedPoint(Vector2Int pos)
    {
        return visitedPoints.Contains(pos);
    }

}
