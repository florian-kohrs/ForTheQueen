using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyNpcMovement : RigidbodyMovement, IMovementController, IGetMovementController
{

    
    public float moveSpeed = 15;


    public virtual IMovementController Controller => this;

    public bool IsMount => false;

    public MovementTargetCalculator movement;

    protected void FixedUpdate()
    {
        Vector3 dir = GetFrameMoveDir();


        if (dir.sqrMagnitude > moveSpeed * Time.deltaTime)
        {
            dir = dir.normalized * moveSpeed * Time.deltaTime;
        }

        if (dir.sqrMagnitude < 0.2f)
        {
            if(body.velocity.sqrMagnitude < 0.2f)
            {
                body.isKinematic = true;
            }
            else
            {
                body.velocity -= 1.5f * Time.deltaTime * body.velocity;
                AddForce(dir);
            }
        }
        else
        {
            AddForce(dir);
        }
    }

    protected Vector3 GetFrameMoveDir()
    {
        Vector3 dir = movement.GetFrameMoveDirection(transform.position, moveSpeed * Time.deltaTime);

        //RaycastHit hit;
        //Vector3 origin = transform.position;
        //origin.y -= 0.5f;
        //Ray r = new Ray(origin, dir.normalized);
        //Debug.DrawRay(r.origin, r.direction * 2, Color.red, 0.25f);
        //if (Physics.Raycast(r, out hit, 3f))
        //{
        //    MeshCollider c = hit.transform.GetComponent<MeshCollider>();
        //    if (c)
        //    {
        //        Vector3 right = Vector3.Cross(dir,(hit.point + new Vector3(0,1,0))-transform.position);
        //        //Debug.DrawRay(r.origin, (hit.point + new Vector3(0, 1, 0)) - transform.position, Color.blue, 0.25f);
        //        //Vector3 straightTriangle = Quaternion.AngleAxis(90, right).eulerAngles;

        //        float angle = Vector3.Angle(dir, hit.point);

        //        float steepness = 1 - hit.normal.y;
        //        Vector3 newDir = Quaternion.AngleAxis(25, right) * dir;
        //        dir = newDir;
        //        Debug.DrawRay(r.origin, dir, Color.yellow, 0.25f);
                
        //    }
        //}
        return dir;
    }

    protected override void Start()
    {
        base.Start();
        movement.GoToPosition(transform.position);
    }


    public void StayAtTargetRange(Transform target, float distance)
    {
        movement.StayAtTargetRange(target, distance);
    }

    public void GoToPosition(Vector3 pos)
    {
        movement.GoToPosition(pos);
    }

    public void GoToTarget(Transform target)
    {
        StayAtTargetRange(target, 0);
    }

    public void StayAtCurrentPosition()
    {
        GoToPosition(transform.position);
    }


    public bool CanReachPoint(Vector3 p)
    {
        return true;
    }

    public void PassTargetAtDistance(Transform target, float normalDistance, float targetDistance, System.Action onPassed = null)
    {
        movement.PassTargetAtDistance(target, normalDistance, targetDistance, onPassed);
    }
}
