using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : SaveableScriptableObject
{

    public Sprite icon;
    public string itemName;
    public string itemDescription;
    public int value;
    public bool canBeSoled;
    
}
