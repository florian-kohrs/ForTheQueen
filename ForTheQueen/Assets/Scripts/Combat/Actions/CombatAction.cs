using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAction/* : ScriptableObject*/
{

    public string name;

    public bool targetEnemies = true;

    public int numberSkillChecks;

    public ActionTarget target;

    public List<ActionEffect> effects;

    [Range(20,100)]
    public int accuracy = 100;

    [Tooltip("If target enemies is false, damage will heal instead")]
    public int damage;

    public static CombatAction fleeAction => new CombatAction() { name = "Flee", accuracy = 80, damage = 0, targetEnemies = false, target = ActionTarget.Self, numberSkillChecks = 2, effects = new List<ActionEffect>() };
    
    public static CombatAction unarmedStrikeAction = new CombatAction() { name = "Unarmed Strike", accuracy = 100, damage = 5, numberSkillChecks = 2, target = ActionTarget.Single, targetEnemies = true, effects = new List<ActionEffect>() };


}
