using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLooks
{

    public int raceId;

    public int beardId;

    public int otherCosmeticId;

    public int skinColorId;

    public GameObject SpawnPlayer()
    {
        return GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

}
