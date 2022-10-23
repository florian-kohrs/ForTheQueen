using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleParticipant
{

    GameObject gameObject { get; }

    void StartTurn();

    void OnTurnEnded();

    int MaxHealth { get; }

    int CurrentHealth { get; set; }

    int Armor { get; }

    int MagicResist { get; }

    int Speed { get; }

    CombatState CombatState { set; }

    string Name { get; }

    bool OnPlayersSide { get; }

    Vector2Int CurrentTile { get; set; }

    void ApplyEffect(ActionEffect effect);

}
