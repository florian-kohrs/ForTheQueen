using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementController : IMovementPredicter
{

    void GoToPosition(Vector3 pos);

    void GoToTarget(Transform target);

    void StayAtCurrentPosition();

    void StayAtTargetRange (Transform target, float distance);

    bool CanReachPoint(Vector3 p);

    void PassTargetAtDistance(Transform target, float normalDistance, float targetDistance, System.Action onPassed = null);

    bool IsMount { get; }

}
