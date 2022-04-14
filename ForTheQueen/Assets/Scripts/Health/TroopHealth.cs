using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopHealth : HealthController, ITroopHealth
{

    public List<AudioClip> getHitSounds;

    public List<AudioClip> deathSound;

    public AudioSource source;

    public Rigidbody r;

    public void SetArmor(float armor)
    {
        this.armor = armor;
    }

    public void SetMaxHealth(float maxHealth)
    {
        maxValue = maxHealth;
        CurrentValue = maxValue;
    }

    protected override void OnValueChanged(float delta)
    {
        if (getHitSounds.Count > 0)
        {
            if (delta < 0)
            {
                source.clip = Rand.PickOne(getHitSounds);
                source.Play();
            }
        }
    }

    public override void OnDeath()
    {
        r.isKinematic = true;
        //r.AddForce((transform.forward + transform.up/2) * -25, ForceMode.Impulse);
        r.useGravity = false;
        r.transform.eulerAngles = new Vector3(0, r.transform.eulerAngles.y, 0);
        r.transform.localScale = new Vector3(1, 0.1f, 1);
        r.transform.position += new Vector3(0, -0.5f, 0);

        foreach (MonoBehaviour b in GetComponentsInChildren<MonoBehaviour>())
        {
            b.enabled = false;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        if (deathSound.Count > 0)
        {
            source.clip = Rand.PickOne(deathSound);
            source.Play();
        }
    }

}
