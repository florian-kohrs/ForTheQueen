using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyableAssets : ScriptableObject
{

    public abstract GameObject Apply(Transform target, Hero h);

    public abstract void Remove(GameObject runTimeInstance, Hero h);

    public string description;

}
