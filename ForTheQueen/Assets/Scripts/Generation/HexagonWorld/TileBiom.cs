using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileBiom : SaveableScriptableObject
{

    public float moveSpeedOnTile;

    public Color color;

    public bool CanMoveOnTile => moveSpeedOnTile > 0;

}
