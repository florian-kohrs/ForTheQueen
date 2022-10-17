using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : InventoryCreatureCombat
{

    protected Hero hero;

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
        Debug.Log("Player started turn");
    }
}
