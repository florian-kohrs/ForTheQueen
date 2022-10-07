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

    public bool customizationDone;

    public GameObject SpawnPlayer(Transform parent, Hero h)
    {
        ApplyableAssets race = ListLookUp.instance.races.list[raceId];
        GameObject heroGameObject = race.Apply(parent, h);

        return heroGameObject;
    }

}
