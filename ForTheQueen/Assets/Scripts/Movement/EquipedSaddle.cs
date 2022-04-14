using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedSaddle : MonoBehaviour, ISaddle
{

    public SaddleItem saddle;

    public float Effectiveness => saddle.effectiveness;

    public Vector3 Offset => saddle.equipLocalPosition;

    public int Slots => saddle.slots;

    public int usedSlots;

    public Vector3 RiderPosition => saddle.RiderPosition;

    public EquipableItemAsset SaddleItem => saddle;

    public List<IRider> ridersOnSaddle;

}
