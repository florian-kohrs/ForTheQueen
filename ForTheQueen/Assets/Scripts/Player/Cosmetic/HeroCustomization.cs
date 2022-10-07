using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroCustomization
{

    public int raceId;

    public int classId;

    public int beardId;

    public int otherCosmeticId;

    public int skinColorId;

    public bool designIsSet;

    public GameObject SpawnPlayer()
    {
        return GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

}
