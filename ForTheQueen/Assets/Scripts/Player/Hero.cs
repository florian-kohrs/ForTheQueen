using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero : ITileOccupation
{

    public Hero() { }

    public Hero(SpawnableCreature heroPrefab)
    {
        this.heroPrefab = heroPrefab;
    }

    public SpawnableCreature heroPrefab;

    [System.NonSerialized]
    public int ownerRoomId;

    public int heroId;

    public Inventory inventory;

    public CreatureStats playerStats;

    public PlayerLooks looks;

    public MapTile MapTile { get; set; }

    public bool isPlayersTurn;

    [System.NonSerialized]
    protected GameObject playerObject;

    public bool CanBeCrossed => true;

    public bool CanBeEntered => true;

    protected bool IsInCity => !playerObject.activeSelf;

    public void OnPlayerEntered(Hero p)
    {
    }

    public void OnPlayerMouseExit(Hero p)
    {
    }

    public void OnPlayerMouseHover(Hero p)
    {
    }

    public void OnPlayerReachedFieldAsTarget(Hero p)
    {
        if (IsInCity)
            return;
        ///step aside
    }

    public void OnPlayerLeftFieldAfterStationary(Hero p)
    {
        if(IsInCity)
            return;
        ///move back
    }

    public void OnPlayerUncovered(Hero p)
    {
    }

    public void OnEnterCity()
    {
        playerObject.SetActive(false);
    }

    public void OnExitCity()
    {
        playerObject.SetActive(true);
    }

    public void SpawnOccupation(Transform parent)
    {
        playerObject = GameObject.Instantiate(heroPrefab.prefab, parent);
        //playerObject = looks.SpawnPlayer();
        playerObject.transform.position = MapTile.CenterPos;
    }

}
