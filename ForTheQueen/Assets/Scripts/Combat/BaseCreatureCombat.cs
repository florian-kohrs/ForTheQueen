using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreatureCombat : MonoBehaviour, IBattleParticipant
{

    public CreatureStats stats;

    public CombatState CombatState { protected get; set; }

    public int MaxHealth => stats.MaxHealth;


    protected CombatAction selectedCombatAction;

    public CombatAction SelectedCombatAction => selectedCombatAction;

    protected Vector2Int currentSelectedCoord;

    protected Dictionary<ActionEffect, ActionEffectInstance> activeEffects = new Dictionary<ActionEffect, ActionEffectInstance>();
    
    protected abstract IHealthDisplayer HealthDisplayer { get; }

    protected abstract void OnDeath();

    public void ExecuteSelectedAction(Vector2Int v2)
    {
        currentSelectedCoord = v2;
        InterfaceController.GetInterfaceMask<CombatInfoText>().Close();
        InterfaceController.GetInterfaceMask<CombatActionUI>().Close();
        InterfaceController.GetInterfaceMask<SkillCheckUI>().EvaluateSkillCheck(OnActionEvaluated);
    }

    protected void OnActionEvaluated(SkillCheckResult r)
    {
        foreach(var b in CombatState.battleMap.GetAfflictedParticipants(currentSelectedCoord, SelectedCombatAction))
        {
            selectedCombatAction.ApplyActionToTarget(b, r);
        }
    }


    public int CurrentHealth
    {
        get
        {
            return stats.currentHealth;
        }
        set
        {
            stats.currentHealth = value;
            HealthDisplayer.ChangeHealth(value);
            if (value <= 0)
                OnDeath();
        }
    }

    public int Speed  => stats.speed;

    public int Armor => stats.armor;

    public int MagicResist =>stats.magicResist;

    public abstract string Name { get; }

    public abstract bool OnPlayersSide { get; }

    public Vector2Int CurrentTile { get ; set; }

    public abstract void StartTurn();

    public virtual void OnTurnEnded() { }

    public void ApplyEffect(ActionEffect effect)
    {
        ActionEffectInstance e;
        if(!activeEffects.TryGetValue(effect, out e))
        {
            activeEffects.Add(effect, e);
        }
        e.stackAmount++;
    }
}
