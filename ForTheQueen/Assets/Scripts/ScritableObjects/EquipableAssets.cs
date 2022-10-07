using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableAssets : ApplyableAssets
{

    public Vector3 equipPos;

    public Vector3 equipScale = Vector3.one;

    public GameObject prefab;

    public override GameObject Apply(Transform target, Hero h)
    {
        return CreateInstance(target);
    }

    public GameObject CreateInstance(Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        Transform t = instance.transform;
        t.localPosition = equipPos;
        t.localScale = equipScale;
        OnInstantiate(instance);
        return instance;
    }

    public override void Remove(GameObject runTimeInstance, Hero h)
    {
        Destroy(runTimeInstance);
    }

    protected virtual void OnInstantiate(GameObject instance) { }

}
