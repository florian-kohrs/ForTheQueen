using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipableInventory : Inventory
{

    [NonSerialized]
    public IItemEquipper equipper;

    protected AssetPolyRef<EquipableWeapon> equippedWeapon;

    [NonSerialized]
    protected GameObject equippedWeaponInstance;

    public bool HasWeaponEquipped => equippedWeapon != null && equippedWeapon.RuntimeRef != null;

    public EquipableWeapon EquippedWeapon => equippedWeapon?.RuntimeRef;

    public List<CombatAction> AvailableCombatActions => HasWeaponEquipped ? equippedWeapon.RuntimeRef.actions : new List<CombatAction>() { CombatAction.unarmedStrikeAction };
    
    public List<CombatAction> AvailableWeaponActions => HasWeaponEquipped ? equippedWeapon.RuntimeRef.actions : new List<CombatAction>() { };

    public void EquipWeapon(EquipableWeapon w)
    {
        if (HasWeaponEquipped)
            RemoveEquippedWeapon();

        equippedWeapon = new AssetPolyRef<EquipableWeapon>(w);
        equippedWeaponInstance = w.CreateInstance(equipper.WeaponParent);
        equipper.EquipWeapon(equippedWeaponInstance);
    }

    public void RemoveEquippedWeapon()
    {
        GameObject.Destroy(equippedWeaponInstance);
        equippedWeaponInstance = null;
        equippedWeapon = null;
    }

    public void InitializeInventoryWithItems(List<ItemContainer> items)
    {
        foreach (var item in items)
        {
            AddItem(item);
            if(!HasWeaponEquipped && item.item.RuntimeRef is EquipableWeapon e)
            {
                EquipWeapon(e);
            }
        }
    }

}
