using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero : ITileOccupation, IBattleOccupation, IItemEquipper, IMovementAgent
{

    public Hero() 
    {
    }

    public bool IsMine => !PhotonNetwork.IsConnected || PhotonNetwork.LocalPlayer.ActorNumber == ownerRoomId;

    [NonSerialized]
    public int ownerRoomId;

    public int heroIndex;

    public EquipableInventory inventory;

    public CreatureStats heroStats;

    public string heroName;

    public bool CanEnterWater => false;

    public HeroCustomization Customization
    {
        get
        {
            if (customization == null) 
            { 
                customization = new HeroCustomization();
                Debug.LogWarning("Created new customization. Should only happen during developement when starting in game scene");
            }
            return customization;
        }
    }

    public HeroCustomization customization;

    public MapTile MapTile
    {
        get => mapTile;
        set => mapTile = value;
    }

    public void DisplayInBattle(Transform leftPartyParent)
    {

    }

    public MapTile mapTile;

    protected bool isHerosTurn;

    public bool IsHerosTurn => isHerosTurn; 

    [NonSerialized]
    protected GameObject heroObject;

    public Transform runtimeHeroObject => heroObject.transform;

    [NonSerialized]
    public bool interuptMovement;

    public int maxFocus;

    public int MaxFocus => maxFocus;

    public int currentFocus;

    public int CurrentFocus => currentFocus;

    public void UseFocusOnSkillCheck(SkillCheck check)
    {
        currentFocus--;
        check.numberFocusUsed++;
    }

    public void RegainSkillCheckFocus(SkillCheck check)
    {
        currentFocus += check.numberFocusUsed;
        check.numberFocusUsed = 0;
    }

    public bool CanSpendFocus => currentFocus > 0;

    public bool CanBeCrossed => true;

    public bool CanBeEntered => true;

    public int restMovementInTurn;

    protected bool IsInCity => !heroObject.activeSelf;

    public bool OnPlayersSide => true;

    public bool HasSupportRange => false;

    public bool HelpsInFight => true;

    public Transform HelmetParent => heroObject.transform;

    public Transform WeaponParent => heroObject.transform;

    public int MovementRemaining { get => restMovementInTurn; set => restMovementInTurn = value; }

    public Vector2Int CurrentTile { get => mapTile.Coordinates; set => mapTile = HexagonWorld.instance.DataFromIndex(value); }

    public Transform RuntimeHeroObject => runtimeHeroObject;

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
        SkillCheckUI ui = InterfaceController.GetInterfaceMask<SkillCheckUI>();
        ui.AdaptUIAndOpen(SkillCheck.GetMovementSkillCheckForHero(this));
        ui.EvaluateSkillCheck(r => restMovementInTurn = r.numberSuccessfull);
    }

    public void OnHeroEnter(Hero p, MapMovementAnimation mapMovement)
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
        Heroes.SetHero(this, heroIndex);
        heroObject = Customization.SpawnPlayer(parent, this);
        //playerObject = looks.SpawnPlayer();
        heroObject.transform.position = MapTile.CenterPos;

        if(MapTile.ContainsTown)
            OnEnterCity();
    }

    public void DisplayInPreFight(Transform parent)
    {
        Debug.Log("Hero Display not ready");
    }

    public IBattleParticipant GetParticipant()
    {
        GameObject hero = Customization.SpawnPlayer(null, this);
        HeroCombat h = hero.GetComponent<HeroCombat>();
        h.Hero = this;
        return h;
    }

    public void EquipHelmet(GameObject g)
    {
        
    }

    public void EquipWeapon(GameObject g)
    {
        
    }
}
