using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleEnemyOccupation : BaseEnemyOccupation<SingleEnemyOccupationScripableObject>
{

    public SingleEnemyOccupation() { }

    public SingleEnemyOccupation(SingleEnemyOccupationScripableObject e) : base(e) { }

}
