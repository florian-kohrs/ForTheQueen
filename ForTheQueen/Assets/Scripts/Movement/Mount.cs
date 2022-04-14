using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mount : HealthController, IMount
{

    public abstract void MoveTowardsTarget(Vector3 dir);

    public abstract void ApplyGravityPull(float fallSpeed);

    public abstract void AnimateDirection(Vector3 dir);

    public abstract bool CanMount(IRider r);

    public abstract Vector3 SaddlePosition { get; }

    protected virtual void OnStart() { }
    protected virtual void AfterUpdate() { }

    protected IRider rider;

    protected virtual void OnStop() { }

    protected bool isStopped;

    public bool HasSaddle => saddle != null;

    protected ISaddle saddle;

    public SaddleItem startWithSaddle;

    protected EquipedSaddle saddleInstance;

    public float agility = 40;

    public virtual float ActiveSpeed(Vector3 targetDir)
    {
        float angle = Vector3.Angle(targetDir.normalized, transform.forward);
        float slowFactor = Mathf.Cos(angle / 360 * Mathf.PI);
        return speed * slowFactor;
    }

    [SerializeField]
    protected MovementTargetCalculator movement;

    public float speed = 5;

    public abstract bool CanReachPoint(Vector3 p);


    public abstract bool IsGrounded { get; }

    public ISaddle Saddle { get => saddle; set => EquipSaddle(value); }


    public IRider MountedBy => rider;

    IMovementController IGetMovementController.Controller => this;

    public bool IsMount => true;

    public Vector3 Position => transform.position;

    protected void Start()
    {
        if(startWithSaddle != null)
        {
            EquipSaddle(startWithSaddle);
        }
        if (movement.FrameTarget == Vector3.zero)
        {
            movement.GoToPosition(transform.position);
        }
        OnStart();
    }

    public void EquipSaddle(ISaddle saddle)
    {
        if(this.saddle != null)
        {
            Destroy(saddleInstance);
        }
        this.saddle = saddle;
        EquipableItemAsset saddleItem = saddle.SaddleItem;
        saddleInstance = saddleItem.GetItemInstance(transform).GetComponent<EquipedSaddle>();
        saddleInstance.transform.localPosition += SaddlePosition;
    }

    public void GoToTarget(Transform target)
    {
        StayAtTargetRange(target, 0);
    }

    protected void Update()
    {
        ApplyGravity();
        MoveAndAnimate();
        AfterUpdate();
    }

    private void ApplyGravity()
    {
        if (!IsGrounded)
        {
            airTime += Time.deltaTime;
            ApplyGravityPull(airTime * airTime * Physics.gravity.y);
        }
        else
        {
            airTime = 0;
        }
    }

    protected float idleDistance = 0.25f;

    private void MoveAndAnimate()
    {
        Vector3 dir = movement.GetFrameMoveDirection(transform.position, speed * Time.deltaTime);

        if (movement.strategy != MovementTargetCalculator.Strategy.GoTo 
            || dir.sqrMagnitude > idleDistance * idleDistance)
        {
            isStopped = false;
            MoveTowardsTarget(dir);
            AnimateDirection(dir);
        }
        else if (!isStopped)
        {
            isStopped = true;
            OnStop();
        }
    }

    [SerializeField]
    protected float airTime = 0;


    public bool MountWith(IRider r)
    {
        bool result = CanMount(r);
        if (result)
        {
            rider = r;
            r.MountAt(this, saddleInstance.transform.position + saddle.RiderPosition, saddleInstance.transform);
        }
        return result;
    }

    public override void OnDeath()
    {
        rider.OnHorseDeath();
    }

    public void GoToPosition(Vector3 pos)
    {
        movement.GoToPosition(pos);
    }

    public void StayAtCurrentPosition()
    {
        GoToPosition(transform.position);
    }

    public void StayAtTargetRange(Transform target, float distance)
    {
        movement.StayAtTargetRange(target, distance);
    }

    public void Demount(IRider r)
    {
        r.Demount();
    }

    public void OnRiderDemounted(IRider r)
    {
        rider = null;
    }
    
    public void PassTargetAtDistance(Transform target, float f, float targetDistance, System.Action onPassed = null)
    {
        movement.PassTargetAtDistance(target, f, targetDistance, 
            delegate 
            { 
                if(onPassed == null)
                {
                    GoToPosition(transform.position + transform.forward * 15);
                }
                else
                {
                    onPassed.Invoke(); 
                }
            });
    }

    public Vector3 GetDirection()
    {
        Vector3 dir = movement.GetFrameMoveDirection(transform.position, speed * Time.deltaTime);
        return dir.normalized * ActiveSpeed(dir);
    }
}
