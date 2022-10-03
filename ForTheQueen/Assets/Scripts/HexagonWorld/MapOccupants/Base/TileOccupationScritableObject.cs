using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOccupationScritableObject : SaveableScriptableObject
{

    public GameObject prefab;

    public Vector3 spawnOffset;

    public Vector3 spawnScale = Vector3.one;

    public GameObject Spawn(Transform parent)
    {
        GameObject gameObject = Instantiate(prefab, parent);
        gameObject.transform.localPosition = spawnOffset;
        gameObject.transform.localScale = spawnScale;
        return gameObject;
    }

}
