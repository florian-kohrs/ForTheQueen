using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileDispenserType { Crossbow, Bow }

public class ProjectileItem : InstantiatableItem
{

    public float weight;

    public float damage = 5;

    public int stackAmount = 20;

    public ProjectileDispenserType[] canBeUsedWith = new ProjectileDispenserType[1];

    [Tooltip("Ignores the parent scale")]
    public Vector3 demiLossyScale = Vector3.one;

    public override GameObject GetItemInstance(Transform parent)
    {
        GameObject result = base.GetItemInstance(parent);
        Vector3 pLocalScale = result.transform.parent.localScale;
        result.transform.localScale = new Vector3(
            demiLossyScale.x / pLocalScale.x,
            demiLossyScale.y / pLocalScale.y,
            demiLossyScale.z / pLocalScale.z
            );
        result.GetComponent<IProjectile>().ProjectileDamage = damage;
        return result;
    }

}
