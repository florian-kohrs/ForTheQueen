using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMount : Mount
{

    protected Vector3 frameMovement;

    public override bool CanReachPoint(Vector3 p)
    {
        return true;
    }

    public CharacterController controller;

    public override bool IsGrounded => controller.isGrounded;

    public Vector3 saddlePosition;

    public override Vector3 SaddlePosition => saddlePosition;

    protected override void OnStart()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    protected override void AfterUpdate()
    {
        controller.Move(frameMovement);

        frameMovement = Vector3.zero;
    }

    public override void MoveTowardsTarget(Vector3 dir)
    {
        dir.y = 0;
        dir.Normalize();
        Vector3 move = dir * Time.deltaTime * ActiveSpeed(dir);
        if (move.sqrMagnitude > dir.sqrMagnitude)
        {
            move = dir;
        }

        frameMovement += move;
        //controller.Move(frameMovement);
       
    }

    public override bool CanMount(IRider r)
    {
        return true;
    }

    public override void ApplyGravityPull(float fallSpeed)
    {
        frameMovement.y = fallSpeed;
       // controller.Move(new Vector3(0, fallSpeed, 0));
    }

    public override void AnimateDirection(Vector3 dir)
    {
        Vector3 frameDestination = new Vector3(
            transform.position.x + dir.x, 
            transform.position.y, 
            transform.position.z + dir.z);
        transform.LookAt(frameDestination);
    }
}
