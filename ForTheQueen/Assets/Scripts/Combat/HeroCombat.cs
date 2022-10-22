using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : InventoryCreatureCombat
{

    protected Hero hero;

    public static HeroCombat currentHeroTurnInCombat;
    public static Hero CurrentActiveHeroInCombat => currentHeroTurnInCombat.hero;
    public static EquipableWeapon CurrentHeroEquippedWeapon => currentHeroTurnInCombat.inventory.EquippedWeapon;

    public EquipableWeapon EquippedWeapon => inventory.EquippedWeapon;

    public Hero Hero
    {
        get => hero;
        set
        {
            hero = value;
            stats = hero.heroStats;
            inventory = hero.inventory;
        }
    }

    public override void StartTurn()
    {
        currentHeroTurnInCombat = this;
        Debug.Log("Player started turn");
        InterfaceController.GetInterfaceMask<CombatActionUI>().AdaptUIAndOpen(this);
    }

    public override void OnTurnEnded()
    {
        enabled = false;
    }

    private void Update()
    {
        
    }

}
