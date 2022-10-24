using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : InventoryCreatureCombat, IMouseTileSelectionCallback<IBattleParticipant>, IHealthDisplayer
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

    protected override IHealthDisplayer HealthDisplayer => this;

    protected override List<CombatAction> AllCombatActions => throw new System.NotImplementedException();

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
            currentSelectedCoord = new Maybe<Vector2Int>(CurrentTile);
            ExecuteSelectedAction(CurrentTile);
        }
        else 
        {
            CombatState.mouseMapTileEvent.subscribers.AddSubscriber(this);
            this.DoDelayed(0.1f, delegate { enabled = true; });
            InterfaceController.GetInterfaceMask<CombatActionUI>().Close();
            InterfaceController.GetInterfaceMask<CombatInfoText>().AdaptUIAndOpen("Select target field");
        }
    }

    protected override void AfterActionLockedIn()
    {
        CombatState.mouseMapTileEvent.subscribers.RemoveSubscriber(this);
        enabled = false;
    }

    protected override void AfterActionExecuted()
    {
        selectedCombatAction = null;
        enabled = true;
    }

    public override void OnTurnEnded()
    {
        enabled = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && SelectedCombatAction != null && IsCurrentSelectedCoordValid())
        {
            CombatState.SelectHoveredMapTile(currentSelectedCoord.Value);
        }
    }


    public void BeginTileHover(Vector2Int coord, IBattleParticipant tile)
    {
        currentSelectedCoord = new Maybe<Vector2Int>(coord);
        CombatState.BeginHoverMapTile(currentSelectedCoord.Value);
    }

    public void OnMouseStay(Vector2Int coord, IBattleParticipant tile)
    {
    }

    public void ExitTileHovered(Vector2Int coord, IBattleParticipant tile)
    {
        currentSelectedCoord.RemoveValue();
        CombatState.StopHoveredTile();
    }

    protected bool IsCurrentSelectedCoordValid()
    {
        return currentSelectedCoord != null && currentSelectedCoord.HasValue;
    }

    protected override void OnDeath()
    {
        Debug.Log($"Player {hero.heroName} died");
    }

    public void UpdateHealthDisplay(int value)
    {
        Debug.Log($"Player {hero.heroName} is on {value} health");
    }
}
