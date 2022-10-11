using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathAccuracy { Perfect, VeryGood, Good, Decent, NotSoGoodAnymore, ITakeAnyThing }

public class Pathfinder<T, J>
{

    private float AccuracyFactor(PathAccuracy acc)
    {
        float result;
        switch (acc)
        {
            case PathAccuracy.Perfect:
                {
                    result = 1f;
                    break;
                }
            case PathAccuracy.VeryGood:
                {
                    result = 0.95f;
                    break;
                }
            case PathAccuracy.Good:
                {
                    result = 0.8f;
                    break;
                }
            case PathAccuracy.Decent:
                {
                    result = 0.5f;
                    break;
                }
            case PathAccuracy.NotSoGoodAnymore:
                {
                    result = 0.2f;
                    break;
                }
            case PathAccuracy.ITakeAnyThing:
                {
                    result = 0f;
                    break;
                }
            default:
                {
                    throw new System.ArgumentException("Unexpected Accuracy: " + acc);
                }
        }
        return result;
    }

    public static List<T> FindPath(INavigatable<T, J> map, T start, J target, PathAccuracy accuracy = PathAccuracy.Perfect)
    {
        return new Pathfinder<T, J>(map, start, target, accuracy).GetPath();
    }

    private Pathfinder(INavigatable<T, J> map, T start, J target, PathAccuracy accuracy, float estimatedStepProgress = 0.5f)
    {
        this.start = start;
        this.target = target;
        pathAccuracy = AccuracyFactor(accuracy);
        nav = map;
        float estimatedLength = nav.DistanceToTarget(start, target);
        int estimatedQueueSize = (int)Mathf.Clamp(estimatedStepProgress * estimatedLength * (1 - (pathAccuracy / 2)), 10, 10000);
        pathTails = new BinaryHeap<float, Path<T, J>>(float.MinValue, float.MaxValue, estimatedQueueSize);
    }

    INavigatable<T, J> nav;

    float pathAccuracy;

    protected J target;

    protected T start;

    protected List<T> GetPath()
    {
        AddTailUnchecked(new Path<T, J>(nav, start, target));
        return BuildPath();
    }
    int count = 0;
    protected List<T> BuildPath()
    {
        count = 0;
        while (HasTail && !ReachedTarget)
        {
            count++;
            AdvanceClosest();
        }
        List<T> result = new List<T>();
        pathTails.Peek().BuildPath(result);
        if (ReachedTarget)
        {
            //Debug.Log("found path after: " + count + " iterations of length " + result.Count);
        }
        else
        {
            Debug.Log("no valid path found");
        }
        return result;
    }

    public void AdvanceClosest()
    {
        try
        {
            Path<T, J> closest = GetClosest();
            usedFields.Add(closest.current);
            IEnumerable<T> adjacent = nav.GetCircumjacent(closest.current);
            foreach (var t in adjacent)
            {
                if (!usedFields.Contains(t))
                    AddTailUnchecked(closest.Advance(t)); 
            }
        }
        catch (Exception x)
        {
            Debug.Log($"Found no path after {count} iterations");
            throw x;
        }
    }

    public Path<T, J> GetClosest()
    {
        Path<T, J> closest;

        do
        {
            closest = pathTails.Dequeue();
        }
        while (usedFields.Contains(closest.current));

        return closest;
    }

    public Path<T, J> PeekUnusedClosest()
    {
        DismissSeenFields();

        return pathTails.Peek();
    }


    public bool ReachedTarget => nav.ReachedTarget(PeekUnusedClosest().current, target);


    protected void DismissSeenFields()
    {
        while (pathTails.Count() > 0 && usedFields.Contains(pathTails.Peek().current))
        {
            pathTails.Dequeue();
        }
    }

    public BinaryHeap<float, Path<T, J>> pathTails;

    public HashSet<T> usedFields = new HashSet<T>();


    public void AddTailUnchecked(Path<T, J> p)
    {
        pathTails.Enqueue(p.TotalMinimumDistance, p);
    }

    protected bool TryGetClosestField(out Path<T, J> path)
    {
        if (pathTails.size > 0)
        {
            path = pathTails.Dequeue();
        }
        else
        {
            path = null;
        }
        return path != null;
    }

    protected bool HasTail
    {
        get 
        {
            DismissSeenFields();
            return pathTails.size > 0; 
        }
    }



}
