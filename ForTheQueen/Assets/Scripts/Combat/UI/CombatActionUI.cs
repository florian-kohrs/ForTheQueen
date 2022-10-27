using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionUI : AdaptableInterfaceMask<HeroCombat>
{

    public GameObject actionPrefab;

    [SerializeField]
    protected Transform uiPrefabParent;

    protected List<GameObject> currentCombatActionsUIs = new List<GameObject>();

    protected List<CombatAction> currentCombatActions;

    protected List<CombatActionButton> currentSkillCheckButtons;

    protected CombatActionButton SelectedSkillCheckButton
    {
        get
        {
            if (ChoosenActionIndex < 0)
                return null;
            else
                return currentSkillCheckButtons[ChoosenActionIndex];
        }
    }

    protected HeroCombat currentHero;

    protected Vector2Int targetField;

    protected CombatAction ChoosenAction => currentCombatActions[ChoosenActionIndex];

    protected int ChoosenActionIndex => HeroCombat.currentHeroTurnInCombat.currentSelectedCombatActionIndex;

    protected override void AdaptUITo(HeroCombat heroCombat, Vector3 pos)
    {
        DeleteOldMarkers();
        currentSkillCheckButtons = new List<CombatActionButton>();
        currentCombatActions = new List<CombatAction>();
        currentCombatActionsUIs = new List<GameObject>();
        currentHero = heroCombat;
        EquipableWeapon weapon = heroCombat.EquippedWeapon;
        
        foreach (var action in currentHero.AllCombatActions)
        {
            CreateActionUI(action, weapon);
        }
        SelectAction(0, true);
    }

    protected void DeleteOldMarkers()
    {
        if (currentCombatActionsUIs != null)
        {
            currentCombatActionsUIs.ForEach(a => { if (a != null) Destroy(a); });
        } 
    }

    protected GameObject CreateActionUI(CombatAction a, EquipableWeapon equipedWeapon)
    {
        SkillCheck skillCheck = a.GetSkillCheck(currentHero.stats, equipedWeapon);// new SkillCheck(currentHero.Hero) { canFocus = canFocus, skill = skill, numberSkillChecks = a.numberSkillChecks };
        GameObject actionUI = Instantiate(actionPrefab, uiPrefabParent);
        CombatActionButton c = actionUI.GetComponent<CombatActionButton>();
        SkillCheckButton b = c.skillCheckBtn;
        b.SetSkillCheck(skillCheck);
        b.Hero = HeroCombat.CurrentActiveHeroInCombat;
        int i = currentCombatActions.Count;
        b.leftClickAction = delegate { PunBroadcastCommunication.SelectCombatAction(i); };
        b.onMouseHover = delegate { PunBroadcastCommunication.BeginHoverSkillCheckBtn(i); };
        ApplyActionToUI(actionUI, a);
        c.combatAction = a;
        c.Display();
        c.Unselect();
        currentSkillCheckButtons.Add(c);
        currentCombatActionsUIs.Add(actionUI);
        currentCombatActions.Add(a);
        return actionUI;
    }

    public void BeginHoverSkillCheckBtn(int index)
    {
        //HeroCombat.currentHeroTurnInCombat.currentSelectedCombatActionIndex = index;
        //InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUIAndOpen(currentSkillCheckButtons[index].skillCheckBtn.skillCheck);
    }

    public void SelectAction(int index, bool forceSelect = false)
    {
        SelectedSkillCheckButton?.Unselect();
        if (index == ChoosenActionIndex && !forceSelect)
        {
            index = -1;
            HeroCombat.currentHeroTurnInCombat.currentSelectedCombatActionIndex = index;
            InterfaceController.GetInterfaceMask<SkillCheckUI>().RemoveMask();

        }
        else
        {
            HeroCombat.currentHeroTurnInCombat.currentSelectedCombatActionIndex = index;
            SelectedSkillCheckButton.Select();
            InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUIAndOpen(currentSkillCheckButtons[index].skillCheckBtn.SkillCheck);
            HeroCombat.currentHeroTurnInCombat.OnSelectedAction(ChoosenAction);
        }
    }

    protected void OnSkillCheckDone(SkillCheckResult r)
    {
        if (r.CritFail)
        {
             if(currentHero.inventory.HasWeaponEquipped && currentHero.EquippedWeapon.brakesOnCritFail)
                Debug.Log("Destroy weapon here");
            Debug.Log($"Crit fail");

        }
        else
        {
            Debug.Log($"{r.numberSuccessfull} successes with action {ChoosenAction.name}");
        }
    }

    protected void ApplyActionToUI(GameObject ui, CombatAction action)
    {
        //ui.GetComponent<Image>().sprite = GenerellLookup.instance.spriteLookup.SpriteFromSkillCheck(action.actionSkillCheck.skill);
    }

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => false;

    public override bool BlockPlayerActiveAction => false;

    public override bool BlockPlayerPassiveAction => false;


}

