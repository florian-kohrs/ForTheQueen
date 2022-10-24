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

    protected abstract List<CombatAction> AllCombatActions { get;}

    protected Maybe<Vector2Int> currentSelectedCoord = new Maybe<Vector2Int>();

    protected Dictionary<ActionEffect, ActionEffectInstance> activeEffects = new Dictionary<ActionEffect, ActionEffectInstance>();
    
    protected abstract IHealthDisplayer HealthDisplayer { get; }

    public HashSet<IParticipantUIReference> UIReferences { get; private set; } = new HashSet<IParticipantUIReference>();

    protected abstract void OnDeath();


    public void ExecuteAction(int index, Vector2Int target)
    {
        selectedCombatAction = AllCombatActions[index];
        InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUIAndOpen(selectedCombatAction.GetSkillCheck(stats, null));
        ExecuteSelectedAction(target);
    }

    public void ExecuteSelectedAction(Vector2Int v2)
    {
        currentSelectedCoord = new Maybe<Vector2Int>(v2);
        InterfaceController.GetInterfaceMask<CombatInfoText>().Close();
        InterfaceController.GetInterfaceMask<CombatActionUI>().Close();
        InterfaceController.GetInterfaceMask<SkillCheckUI>().EvaluateSkillCheck(OnActionEvaluated);
        AfterActionLockedIn();
    }

    protected virtual void AfterActionLockedIn() { }

    protected virtual void AfterActionExecuted() { }

    protected void OnActionEvaluated(SkillCheckResult r)
    {
        foreach(var b in CombatState.battleMap.GetAfflictedParticipants(currentSelectedCoord.Value, SelectedCombatAction))
        {
            selectedCombatAction.ApplyActionToTarget(b, r);
        }
        AfterActionExecuted();
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
            HealthDisplayer.UpdateHealthDisplay(value);
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

    public void AddUIReference(IParticipantUIReference uiReference)
    {
        UIReferences.Add(uiReference);
        uiReference.RegisteredInSet = UIReferences;
    }

}
