using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaddleItem : EquipableItemAsset, ISaddle
{

    public float Effectiveness => effectiveness;

    public Vector3 Offset => equipLocalPosition;

    public Vector3 RiderPosition => riderPosition;

    public Vector3 riderPosition;

    public int Slots => slots;

    public float effectiveness = 5;

    public int slots = 1;

    EquipableItemAsset ISaddle.SaddleItem => this;
}
