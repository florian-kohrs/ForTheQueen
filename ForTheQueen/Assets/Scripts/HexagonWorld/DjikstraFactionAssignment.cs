using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DjikstraFactionAssignment
{

    protected static Queue<Vector2Int> candidates = new Queue<Vector2Int>();

    protected static Vector2Int currentContinentSize;

    protected static int[,] currentContinent;

    public static void AssignFactionForContinent(int[,] continent)
    {
        currentContinent = continent;
        InitialFactionAssignment();
        while(candidates.Count > 0)
        {
            Vector2Int next = candidates.Dequeue();

            int currentFaction = continent[next.x, next.y];
            foreach(Vector2Int neighbour in GetNeighboursOf(next))
            {
                candidates.Enqueue(neighbour);
                continent[neighbour.x, neighbour.y] = currentFaction;
            }
        }
    }

    protected static void InitialFactionAssignment()
    {
        int widthIndex = currentContinent.GetLength(0) - 1;
        int heightIndex = currentContinent.GetLength(1) - 1;
        currentContinentSize = new Vector2Int(widthIndex + 1, heightIndex + 1);
        currentContinent[0, 0] = 1;
        currentContinent[widthIndex, 0] = 2;
        currentContinent[0, heightIndex] = 3;
        currentContinent[widthIndex, heightIndex] = 4;

        candidates.Enqueue(new Vector2Int(0, 0));
        candidates.Enqueue(new Vector2Int(widthIndex, 0));
        candidates.Enqueue(new Vector2Int(0, heightIndex));
        candidates.Enqueue(new Vector2Int(widthIndex, heightIndex));
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
        return IsInBound(pos) && !VisitedPoint(pos);
    }

    protected static bool IsInBound(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < currentContinentSize.x &&
            pos.y >= 0 && pos.y < currentContinentSize.y;
    }

    protected static bool VisitedPoint(Vector2Int pos)
    {
        return currentContinent[pos.x, pos.y] > 0;
    }

}
