using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetMenue : MonoBehaviour
{

    private const string BASE_FOLDER_NAME = "ScriptableObjects/";

    //[MenuItem("Assets/Create/Custom/Weapons/MeleeWeapons")]
    //public static void NewMeleeWeapon()
    //{
    //    AssetCreator.CreateAsset<InventoryMeleeWeapon>(BASE_FOLDER_NAME + "MeleeWeapons");
    //}

    [MenuItem("Assets/Create/Custom/HexagonWorld/Biom")]
    public static void NewHexagonBiom()
    {
        AssetCreator.CreateAsset<TileBiom>(BASE_FOLDER_NAME + "HexagonBioms");
    }

    [MenuItem("Assets/Create/Custom/Map/Town")]
    public static void NewTown()
    {
        AssetCreator.CreateAsset<TownScriptableObject>(BASE_FOLDER_NAME + "Town");
    }

    [MenuItem("Assets/Create/Custom/Spawnables/Creauture")]
    public static void NewCreatureReference()
    {
        AssetCreator.CreateAsset<SpawnableCreature>(BASE_FOLDER_NAME + "Creauture");
    }

    [MenuItem("Assets/Create/Custom/Character/DesignerList")]
    public static void NewDesignerList()
    {
        AssetCreator.CreateAsset<DesignerList>(BASE_FOLDER_NAME + "DesignerList");
    }

    [MenuItem("Assets/Create/Custom/Character/EquipableAsset")]
    public static void NewEquipableAsset()
    {
        AssetCreator.CreateAsset<EquipableAssets>(BASE_FOLDER_NAME + "EquipableAsset");
    }

    [MenuItem("Assets/Create/Custom/Character/Class")]
    public static void HeroClass()
    {
        AssetCreator.CreateAsset<HeroClass>(BASE_FOLDER_NAME + "HeroClass");
    }

    [MenuItem("Assets/Create/Custom/Map/BlockingOccupation")]
    public static void BlockingOccupation()
    {
        AssetCreator.CreateAsset<TileBlockingOccupationObject>(BASE_FOLDER_NAME + "TileBlockingOccupation");
    }

    [MenuItem("Assets/Create/Custom/Creatures/SingleEnemy")]
    public static void SingleEnemyOccupationScripableObject()
    {
        AssetCreator.CreateAsset<SingleEnemyOccupationScripableObject>(BASE_FOLDER_NAME + nameof(SingleEnemyOccupationScripableObject));
    }

    //[MenuItem("Assets/Create/Custom/Combat/Action")]
    //public static void CombatAction()
    //{
    //    AssetCreator.CreateAsset<CombatAction>(BASE_FOLDER_NAME + nameof(CombatAction));
    //}

    [MenuItem("Assets/Create/Custom/Item/Weapon")]
    public static void NewWeapon()
    {
        AssetCreator.CreateAsset<EquipableWeapon>(BASE_FOLDER_NAME + nameof(EquipableWeapon));
    }

}
