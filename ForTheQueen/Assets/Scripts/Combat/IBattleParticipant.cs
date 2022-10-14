using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleParticipant
{

    void StartTurn();

    int MaxHealth { get; }

    int CurrentHealth { get; set; }

    int Speed { get; }

    GameObject SpawnCombatObject();
     
    CombatState CombatState { set; }

}
