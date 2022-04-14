using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour, IMovementPredicter
{

    public Rigidbody body;

    public float surfaceMultiplier = 1;

    public bool allowMovement = true;

    public Vector3 Position => body.transform.position;

    protected virtual void Start()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody>();
        }
    }

    public void SetKinematicTo(bool b)
    {
        body.isKinematic = b;
    }

    public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Acceleration)
    {
        if (allowMovement)
        {
            body.isKinematic = false;
            body.AddForce(force * surfaceMultiplier, forceMode);
        }
    }

    public virtual Vector3 GetDirection()
    {
        return body.velocity;
    }


    //public void AddImpulse(Vector3 force, bool useSurfaceMultiplier)
    //{
    //    float multiplier = 1;
    //    if (useSurfaceMultiplier)
    //    {
    //        multiplier = surfaceMultiplier;
    //    }
    //    body.AddForce(force * multiplier, ForceMode.Impulse);
    //}


}
