using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreatureCombat : MonoBehaviour, IBattleParticipant
{

    public CreatureStats stats;

    public CombatState CombatState { protected get; set; }

    public abstract int MovementInTurn { get; }

    public int MaxHealth => stats.MaxHealth;


    public CombatAction SelectedCombatAction
    {
        get
        {
            if (currentSelectedCombatActionIndex < 0)
                return null;

            return AllCombatActions[currentSelectedCombatActionIndex];
        }
    }

    public abstract List<CombatAction> AllCombatActions { get;}

    protected Maybe<Vector2Int> currentSelectedCoord = new Maybe<Vector2Int>();

    protected Dictionary<ActionEffect, ActionEffectInstance> activeEffects = new Dictionary<ActionEffect, ActionEffectInstance>();
    
    protected abstract IHealthDisplayer HealthDisplayer { get; }

    public HashSet<IParticipantUIReference> UIReferences { get; private set; } = new HashSet<IParticipantUIReference>();

    protected abstract void OnDeath();

    public int CurrentAttackRange
    {
        get
        {
            if (SelectedCombatAction == null)
                return 0;
            else
                return Mathf.Max(1, SelectedCombatAction.actionRangeModifier + ExtraAttackRange);
        }
    }

    public int currentSelectedCombatActionIndex = -1;

    protected virtual int ExtraAttackRange => 0;

    public void ExecuteAction(int index, Vector2Int target)
    {
        currentSelectedCombatActionIndex = index;
        InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUIAndOpen(SelectedCombatAction.GetSkillCheck(stats, null));
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
            SelectedCombatAction.ApplyActionToTarget(b, r);
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
    public int RestMovementInTurn { get => restMovement; set => restMovement = value; }

    public abstract bool IsMine { get; }

    protected int restMovement;

    public void StartTurn()
    {
        RestMovementInTurn = MovementInTurn;
        OnStartTurn();
    }

    public abstract void OnStartTurn();

    public virtual void OnTurnEnded() { }

    public void ApplyEffect(ActionEffect effect)
    {
        ActionEffectInstance e;
        if(!activeEffects.TryGetValue(effect, out e))
        {
            e = new ActionEffectInstance();
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
