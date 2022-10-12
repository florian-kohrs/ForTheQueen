using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleOccupation
{

    bool OnPlayersSide { get; }

    bool HasSupportRange { get; }

    bool HelpsInFight { get; }

    MapTile MapTile { get; }

    void DisplayInPreFight(Transform parent);

}
