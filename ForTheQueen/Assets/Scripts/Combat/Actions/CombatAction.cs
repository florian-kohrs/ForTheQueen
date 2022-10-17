using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAction : ScriptableObject
{

    public bool targetEnemies = true;

    public int requiredSlots;

    public ActionTarget target;

    public List<ActionEffect> effects;

    [Range(20,100)]
    public int accuracy = 100;

    [Tooltip("If target enemies is false, damage will heal instead")]
    public int damage;

}
