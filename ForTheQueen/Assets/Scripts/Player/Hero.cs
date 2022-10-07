using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero : ITileOccupation
{

    public Hero() 
    {
    }

    public Hero(SpawnableCreature heroPrefab, int heroIndex) : this()
    {
        this.heroIndex = heroIndex;
        this.heroPrefab = new AssetPolyRef<SpawnableCreature>() { RuntimeRef = heroPrefab};
    }

    public bool IsMine => !PhotonNetwork.IsConnected || PhotonNetwork.LocalPlayer.ActorNumber == ownerRoomId;

    public AssetPolyRef<SpawnableCreature> heroPrefab;

    [NonSerialized]
    public int ownerRoomId;

    public int heroIndex;

    public Inventory inventory;

    public CreatureStats heroStats;

    public HeroCustomization customization;

    public MapTile MapTile
    {
        get => mapTile;
        set => mapTile = value;
    }

    public MapTile mapTile;

    protected bool isHerosTurn;

    public bool IsHerosTurn => isHerosTurn; 

    [NonSerialized]
    protected GameObject heroObject;

    public Transform runtimeHeroObject => heroObject.transform;

    [NonSerialized]
    public bool interuptMovement;

    public bool CanBeCrossed => true;

    public bool CanBeEntered => true;

    public int restMovementInTurn;

    protected bool IsInCity => !heroObject.activeSelf;

    public void StartHerosTurn()
    {
        interuptMovement = false;
        isHerosTurn = true;
        GenerateMovement();
    }

    public void EndHerosTurn()
    {
        isHerosTurn = false;
    }

    public void GenerateMovement()
    {
        restMovementInTurn = 5;
    }

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
        heroObject.SetActive(false);
    }

    public void OnExitCity()
    {
        heroObject.SetActive(true);
    }

    public void SpawnOccupation(Transform parent)
    {
        Heroes.heroes[heroIndex] = this;
        heroObject = GameObject.Instantiate(heroPrefab.RuntimeRef.prefab, parent);
        //playerObject = looks.SpawnPlayer();
        heroObject.transform.position = MapTile.CenterPos;

        if(MapTile.ContainsTown)
            OnEnterCity();
    }

}
