using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreatureCombat : MonoBehaviour, IBattleParticipant
{

    public CreatureStats stats;

    public CombatState CombatState { protected get; set; }

    public int MaxHealth => stats.MaxHealth;

    public int CurrentHealth
    {
        get
        {
            return stats.currentHealth;
        }
        set
        {
            stats.currentHealth = value;
        }
    }

    public int Speed  => stats.speed;

    public abstract GameObject SpawnCombatObject();

    public abstract void StartTurn();
}
