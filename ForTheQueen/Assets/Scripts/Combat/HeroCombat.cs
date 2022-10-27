using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : InventoryCreatureCombat/*, IMouseTileSelectionCallback<BattleMapTile>*/, IHealthDisplayer, IMovementAgent
{

    public static int MAX_MOVEMENT_PER_TURN = 5;

    protected int currentMovementInTurn;

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

    protected override IHealthDisplayer HealthDisplayer => this;


    protected List<CombatAction> allCombatActions;

    public override List<CombatAction> AllCombatActions
    {
        get
        {
            if (allCombatActions == null)
            {
                allCombatActions = new List<CombatAction>();
                allCombatActions.AddRange(currentHeroTurnInCombat.inventory.AvailableCombatActions);
                allCombatActions.Add(CombatAction.fleeAction);
            }
            return allCombatActions;
        }
    }

    public override int MovementInTurn => MAX_MOVEMENT_PER_TURN;

    public int MovementRemaining { get => restMovement; set => restMovement = value; }

    public bool CanEnterWater => false;

    public Transform RuntimeHeroObject => transform;

    public override bool IsMine => hero.IsMine;

    public override void OnStartTurn()
    {
        currentHeroTurnInCombat = this;
        Debug.Log("Player started turn");
        currentMovementInTurn = MAX_MOVEMENT_PER_TURN;
        InterfaceController.GetInterfaceMask<CombatActionUI>().AdaptUIAndOpen(this);
    }

    public void OnSelectedAction(CombatAction a)
    {
        //this.selectedCombatAction = a;
        //if (a.target == ActionTarget.Self)
        //{
        //    currentSelectedCoord = new Maybe<Vector2Int>(CurrentTile);
        //    ExecuteSelectedAction(CurrentTile);
        //}
        //else 
        //{
        //    CombatState.mouseMapTileEvent.subscribers.AddSubscriber(this);
        //    this.DoDelayed(0.1f, delegate { enabled = true; });
        //    InterfaceController.GetInterfaceMask<CombatActionUI>().Close();
        //    InterfaceController.GetInterfaceMask<CombatInfoText>().AdaptUIAndOpen("Select target field");
        //}
    }

    //protected override void AfterActionLockedIn()
    //{
    //    CombatState.mouseMapTileEvent.subscribers.RemoveSubscriber(this);
    //    enabled = false;
    //}

    //protected override void AfterActionExecuted()
    //{
    //    //selectedCombatAction = null;
    //    enabled = true;
    //}

    //public void BeginTileHover(BattleMapTile tile)
    //{
    //    currentSelectedCoord = new Maybe<Vector2Int>(tile.Coordinates);
    //    CombatState.BeginHoverMapTile(tile.Coordinates);
    //}

    //public void OnMouseStay(BattleMapTile tile)
    //{
    //}

    //public void ExitTileHovered(BattleMapTile tile)
    //{
    //    currentSelectedCoord.RemoveValue();
    //    CombatState.StopHoveredTile();
    //}

    //protected bool IsCurrentSelectedCoordValid()
    //{
    //    return currentSelectedCoord != null && currentSelectedCoord.HasValue;
    //}

    protected override void OnDeath()
    {
        Debug.Log($"Player {hero.heroName} died");
    }

    public void UpdateHealthDisplay(int value)
    {
        Debug.Log($"Player {hero.heroName} is on {value} health");
    }
}
