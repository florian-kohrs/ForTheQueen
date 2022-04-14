using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaddle
{

    float Effectiveness { get; }

    Vector3 Offset { get; }

    int Slots { get; }

    Vector3 RiderPosition { get; }

    EquipableItemAsset SaddleItem { get; }


}
