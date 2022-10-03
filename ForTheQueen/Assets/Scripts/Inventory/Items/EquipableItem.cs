using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipableItem : InventoryItem
{

    [Tooltip("Gameobject to instantiate when the item is equiped")]
    [SerializeField]
    protected GameObject prefab;

    [SerializeField]
    protected Vector3 equipPosition;
    public Vector3 EquipPosition => equipPosition;

    [SerializeField]
    protected Vector3 equipEulerAngle;
    public Vector3 EquipEulerAngle => equipEulerAngle;

    [SerializeField]
    protected Vector3 equipScale = Vector3.one;
    public Vector3 EquipScale => equipScale;

    public GameObject CreateInstance(Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        Transform t = instance.transform;
        t.localPosition = equipPosition;
        t.localEulerAngles = equipEulerAngle;
        t.localScale = equipScale;
        OnInstantiate(instance);
        return instance;
    }

    protected virtual void OnInstantiate(GameObject instance) { }

}
