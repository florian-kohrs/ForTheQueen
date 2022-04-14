using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthStateCallbackReciever
{

    void OnDeath();

    void RecieveNonLethalDamage(float damage);

}
