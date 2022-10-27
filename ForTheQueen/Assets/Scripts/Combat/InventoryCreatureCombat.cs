using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryCreatureCombat : BaseCreatureCombat
{

    public EquipableInventory inventory;

    protected override int ExtraAttackRange
    {
        get
        {
            if (inventory != null && inventory.EquippedWeapon != null)
                return inventory.EquippedWeapon.standartWeaponRange;
            else
                return 0;
        }
    }

}
