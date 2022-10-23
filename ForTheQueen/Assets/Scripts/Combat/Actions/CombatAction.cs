using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void ApplyActionToTarget(IBattleParticipant p, SkillCheckResult r)
    {
        p.CurrentHealth -= Mathf.RoundToInt(damage * r.SucessRate);
        if(r.WasPerfect)
        {
            foreach (var e in effects)
                p.ApplyEffect(e);
        }
    }

    public IEnumerable<Vector2Int> GetTargetFieldsFromAction(Vector2Int caster, Vector2Int targetField, ActionTarget t, Predicate<Vector2Int> p)
    {
        return GetTargetFieldsFromAction_(caster, targetField, t).Where(v2=>p(v2));
    }

    protected IEnumerable<Vector2Int> GetTargetFieldsFromAction_(Vector2Int caster, Vector2Int targetField, ActionTarget t)
    {
        switch(t)
        {
            case ActionTarget.Self:
                return new List<Vector2Int>() { };
            case ActionTarget.Single:
                return new List<Vector2Int>() { targetField };
            case ActionTarget.SpreadOne:
                return HexagonPathfinder.GetNeighboursInDistance(targetField, 1);
            case ActionTarget.SpreadTwo:
                return HexagonPathfinder.GetNeighboursInDistance(targetField, 2);
            case ActionTarget.Line:
                return new List<Vector2Int>() { targetField, targetField + Vector2Int.down, targetField + Vector2Int.up };
            case ActionTarget.Row:
                return new List<Vector2Int>() { targetField, targetField + Vector2Int.down, targetField + Vector2Int.up, targetField + Vector2Int.down * 2, targetField + Vector2Int.up * 2 };
            default:
                Debug.LogWarning("Unhandled action target. return single");
                return GetTargetFieldsFromAction_(caster, targetField, ActionTarget.Single);
        }
    }
}
