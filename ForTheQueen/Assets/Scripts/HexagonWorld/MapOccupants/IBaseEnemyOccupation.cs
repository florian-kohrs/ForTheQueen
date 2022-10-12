using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseEnemyOccupation : IBattleOccupation
{

    void Despawn();

    void Delete();

}
