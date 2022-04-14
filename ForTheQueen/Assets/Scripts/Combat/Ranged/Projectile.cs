using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IProjectile
{

    public Rigidbody r;

    public float ProjectileDamage { set; protected get; }

    void Update()
    {
        transform.forward =
        Vector3.Slerp(transform.forward, r.velocity.normalized, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHealthController h = collision.rigidbody?.gameObject.GetComponent<IHealthController>();
        OnProjectileCollisionEnter(collision, h);
    }

    protected abstract void OnProjectileCollisionEnter(Collision c, IHealthController health);

}
