using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleOccupation : ITileOccupation
{

    bool OnPlayersSide { get; }

    bool HasSupportRange { get; }

    bool HelpsInFight { get; }

    void DisplayInPreFight(Transform parent);

    IBattleParticipant GetParticipant();

}
