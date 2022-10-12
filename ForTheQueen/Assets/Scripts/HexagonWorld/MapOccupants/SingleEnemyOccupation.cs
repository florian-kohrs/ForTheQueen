using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SingleEnemyOccupation : BaseEnemyOccupation<SingleEnemyOccupationScripableObject>
{

    public SingleEnemyOccupation() { }

    public SingleEnemyOccupation(SingleEnemyOccupationScripableObject e) : base(e) { }

    public override bool HasSupportRange => OccupationObject.hasSupportRange;

    public override bool HelpsInFight => true;

    public override void DisplayInPreFight(Transform parent)
    {
    }
}
