using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableWeapon : EquipableItem
{
    
    public enum Handling { OneHanded, TwoHanded };
    
    public Handling handling;

    public List<CombatAction> actions;

    public int Damage;

    public SkillCheck.Skill handlingType;

    public bool canFocus = true;

    public bool brakesOnCritFail;

    [Min(1)]
    public int standartWeaponRange = 1;

}
