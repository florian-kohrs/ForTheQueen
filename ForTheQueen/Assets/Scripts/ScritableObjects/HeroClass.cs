using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroClass : ApplyableAssets
{

    public List<ItemContainer> startItems;

    public string classDescription;

    public CreatureStats classStartStats;

    public override GameObject Apply(Transform target, Hero h)
    {
        h.inventory.InitializeInventoryWithItems(startItems);
        h.heroStats = classStartStats;
        return null;
    }

    public override void Remove(GameObject runTimeInstance, Hero h)
    {
        h.inventory.Clear();
    }

}
