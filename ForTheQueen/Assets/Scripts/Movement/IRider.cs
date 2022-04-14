using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRider
{
    
    float MountHandling { get; }

    void OnHorseDeath();

    void MountAt(IMount mount, Vector3 pos, Transform parent);

    void Demount();


}
