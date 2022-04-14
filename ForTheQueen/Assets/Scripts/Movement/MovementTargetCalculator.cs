using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementTargetCalculator
{

    public Vector3 GetFrameMoveDirection(Vector3 from, float maxSpeed)
    {
        Vector3 result;
        switch (strategy)
        {
            case (Strategy.Pass):
                result = PassTarget(from);
                if (maxSpeed * maxSpeed >= result.sqrMagnitude || result.sqrMagnitude <= targetDistance * targetDistance)
                {
                    if (onReachTargetDistance == null)
                    {
                        GoToPosition(from + result.normalized * 10);
                    }
                    else
                    {
                        onReachTargetDistance.Invoke();
                        onReachTargetDistance = null;
                    }
                }
                break;

            case Strategy.GoTo:
                result = GoToTarget(from);
                break;
            default:
                throw new System.NotImplementedException("The case: " + strategy + " was not implemented");
        }
        return result;
    }

    public Vector3 FrameTarget
    {
        get
        {
            if (target != null)
            {
                return target.position;
            }
            else
            {
                return stationaryTarget;
            }
        }
    }

    public Vector3 GetFrameSmartTarget(Vector3 from)
    {
        if (strategy == Strategy.Pass)
        {
            return from + PassTarget(from);
        }
        else
        {
            return from + GoToTarget(from);
        }

    }

    public Transform target;

    public Vector3 stationaryTarget;

    protected float distanceToTarget;

    protected float targetDistance;

    System.Action onReachTargetDistance;

    public enum Strategy { Pass, GoTo }

    public Strategy strategy = Strategy.GoTo;


    private Vector3 PassTarget(Vector3 from)
    {
        Vector3 target = FrameTarget;
        Vector3 dir = target - from;
        Vector3 normal = new Vector3(dir.z, 0, -dir.x);
        Vector3 dirTo = (target + normal.normalized * distanceToTarget) - from;
        return dirTo;
    }

    private Vector3 GoToTarget(Vector3 from)
    {
        Vector3 target = FrameTarget;
        Vector3 directon = (target - from);
        return (target - directon.normalized * distanceToTarget) - from;
    }

    public void GoToPosition(Vector3 pos)
    {
        strategy = Strategy.GoTo;
        target = null;
        stationaryTarget = pos;
    }

    public void GoToTarget(Transform target)
    {
        StayAtTargetRange(target, 0);
    }


    public void StayAtTargetRange(Transform target, float distance)
    {
        distanceToTarget = distance;
        strategy = Strategy.GoTo;
        this.target = target;
    }


    public void PassTargetAtDistance(Transform target, float normalDistance, float targetDistance, System.Action onPassed = null)
    {
        distanceToTarget = normalDistance;
        strategy = Strategy.Pass;
        this.target = target;
        this.targetDistance = targetDistance;
        this.onReachTargetDistance = onPassed;
    }

}
