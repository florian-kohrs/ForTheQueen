using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRider : RigidbodyNpcMovement, IRider
{

    public IMount mount;

    public bool IsMounted => mount != null;

    protected Transform defaultParent;

    public override IMovementController Controller
    {
        get
        {
            if (mount == null)
            {
                return this;
            }
            else
            {
                return mount;
            }
        }
    }

    public float MountHandling => 1;

    public void OnHorseDeath()
    {
        Demount();
    }

    public void MountAt(IMount mount, Vector3 pos, Transform parent)
    {
        defaultParent = transform.parent;
        transform.parent = parent;
        transform.position = pos;
        SetKinematicTo(true);
        allowMovement = false;
        this.mount = mount;
    }

    public void Demount()
    {
        transform.parent = defaultParent;
        defaultParent = null;
        SetKinematicTo(false);
        allowMovement = true;
        mount.OnRiderDemounted(this);

        mount = null;
    }

    public override Vector3 GetDirection()
    {
        if (IsMounted)
        {
            return mount.GetDirection();
        }
        else
        {
            return base.GetDirection();
        }
    }

}
