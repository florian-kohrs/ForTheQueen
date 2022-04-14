using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthController : ClampedValue, IHealthController
{
    public bool IsAlive => CurrentHealth > 0;

    public int CurrentHealth => (int)CurrentValue;

    protected float armor = 0;


    protected IHealthStateCallbackReciever reciever;

    public float CalculateDamageWithArmor(float damage)
    {
        return Mathf.Max(damage - armor, damage / (Mathf.Max(1, armor)));
    }

    public abstract void OnDeath();

    private void Die()
    {
        OnDeath();
        reciever?.OnDeath();
    }

    public bool IsFullLife => CurrentValue == maxValue;

    public int MaxHealth => (int)maxValue;

    public void Heal(int health)
    {
        if (IsAlive)
        {
            Increase(health);
        }
    }

    public bool Damage(int damage)
    {
        if (enabled && IsAlive)
        {
            float reducedDamage = CalculateDamageWithArmor(damage);
            Reduce(damage);
            bool result = !IsAlive;
            if (result)
            {
                Die();
            }
            else
            {
                reciever?.RecieveNonLethalDamage(reducedDamage);
            }
            return result;
        }
        else
        {
            return false;
        }
    }

    public void HealthStateCallbackReciever(IHealthStateCallbackReciever reciever)
    {
        this.reciever = reciever;
    }
}
