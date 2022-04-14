using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoempelProjectile : Projectile
{

    public Collider suctionCup;

    public Collider woodenHandle;
    
    private void Start()
    {
        suctionCup.enabled = true;
        woodenHandle.enabled = false;
    }

    protected override void OnProjectileCollisionEnter(Collision c, IHealthController h)
    {
        Destroy(r);
        Destroy(this);
        suctionCup.enabled = false;
        //woodenHandle.enabled = true;
        transform.parent = c.transform;
        //transform.LookAt(collision.GetContact(0).point - (transform.position - collision.GetContact(0).point) * 2);
        transform.position = c.GetContact(0).point;

        if (h != null)
        {
            if (h.IsAlive)
            {
                h.Damage((int)ProjectileDamage);
            }
        }
        else
        {
            Destroy(gameObject, 5);
        }
    }
}
