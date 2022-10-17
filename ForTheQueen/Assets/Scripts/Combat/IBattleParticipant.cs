using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleParticipant
{

    GameObject gameObject { get; }

    void StartTurn();

    int MaxHealth { get; }

    int CurrentHealth { get; set; }

    int Speed { get; }

    CombatState CombatState { set; }

}
