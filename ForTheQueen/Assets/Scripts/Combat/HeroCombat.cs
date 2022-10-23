using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : InventoryCreatureCombat, IMouseTileSelectionCallback<IBattleParticipant>
{

    protected Hero hero;

    public static HeroCombat currentHeroTurnInCombat;
    public static Hero CurrentActiveHeroInCombat => currentHeroTurnInCombat.hero;
    public static EquipableWeapon CurrentHeroEquippedWeapon => currentHeroTurnInCombat.inventory.EquippedWeapon;

    public EquipableWeapon EquippedWeapon => inventory.EquippedWeapon;

    public Hero Hero
    {
        get => hero;
        set
        {
            hero = value;
            stats = hero.heroStats;
            inventory = hero.inventory;
        }
    }

    public override string Name => hero.heroName;

    public override bool OnPlayersSide => true;

    private void Start()
    {
        enabled = false;
    }

    public override void StartTurn()
    {
        currentHeroTurnInCombat = this;
        Debug.Log("Player started turn");
        InterfaceController.GetInterfaceMask<CombatActionUI>().AdaptUIAndOpen(this);
    }

    public void OnSelectedAction(CombatAction a)
    {
        this.selectedCombatAction = a;
        if (a.target == ActionTarget.Self)
        {
            currentSelectedCoord = CurrentTile;
            ExecuteSelectedAction(currentSelectedCoord);
        }
        else 
        {
            CombatState.mouseMapTileEvent.subscribers.AddSubscriber(this);
            this.DoDelayed(0.1f, delegate { enabled = true; });
            InterfaceController.GetInterfaceMask<CombatInfoText>().AdaptUIAndOpen("Select target field");
        }
    }

    public override void OnTurnEnded()
    {
        enabled = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && IsCurrentSelectedCoordValid())
        {
            CombatState.SelectHoveredMapTile(currentSelectedCoord);
        }
    }


    public void BeginTileHover(Vector2Int coord, IBattleParticipant tile)
    {
        currentSelectedCoord = coord;
        CombatState.BeginHoverMapTile(currentSelectedCoord);
    }

    public void OnMouseStay(Vector2Int coord, IBattleParticipant tile)
    {
    }

    public void ExitTileHovered(Vector2Int coord, IBattleParticipant tile)
    {
        currentSelectedCoord = new Vector2Int(-1,-1);
        CombatState.StopHoveredTile(currentSelectedCoord);
    }

    protected bool IsCurrentSelectedCoordValid()
    {
        return currentSelectedCoord != default;
    }

}
