using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMount : CharacterControllerMount
{

    [SerializeField]
    protected Transform rightLeg;

    [SerializeField]
    protected Transform upperHeadJoint;

    [SerializeField]
    protected Transform leftLeg;

    protected Vector3 startEuler;

    protected float maxJointRot = 76;

    public float animSpeed;

    protected float animTime;

    public float maxAngle = 45;

    protected override void OnStart()
    {
        startEuler = rightLeg.localEulerAngles;
        base.OnStart();
    }

    protected override void OnStop()
    {
        animTime = 0;
        MoveLegs(Vector3.zero);
    }

    public override void AnimateDirection(Vector3 dir)
    {
        upperHeadJoint.LookAt(movement.GetFrameSmartTarget(transform.position));

        upperHeadJoint.localEulerAngles = 
            upperHeadJoint.localEulerAngles.Map(f =>
            {
                if (f > 180) { f -= 360; }
                return Mathf.Sign(f) * Mathf.Min(maxJointRot, Mathf.Abs(f));
            });
        
        Vector3 newEuler = transform.eulerAngles;
        newEuler.y = VectorExtension.TowardsAngle(newEuler.y, upperHeadJoint.eulerAngles.y, agility * Time.deltaTime);
        transform.eulerAngles = newEuler;


        MoveLegs(dir);
    }

    protected void MoveLegs(Vector3 dir)
    {

        animTime += Time.deltaTime * ActiveSpeed(dir);

        float currentX = Mathf.Sin(animTime * animSpeed * 2) * maxAngle;
        Vector3 current = startEuler;
        current.x += currentX;

        rightLeg.localEulerAngles = current;

        current.x -= currentX * 2;
        leftLeg.localEulerAngles = current;

    }

}
