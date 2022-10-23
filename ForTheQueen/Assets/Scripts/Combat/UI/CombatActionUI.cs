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

    protected HeroCombat currentHero;

    protected Vector2Int targetField;

    protected CombatAction choosenAction;

    protected override void AdaptUITo(HeroCombat heroCombat, Vector3 pos)
    {
        DeleteOldMarkers();
        currentCombatActions = new List<CombatAction>();
        currentCombatActionsUIs = new List<GameObject>();
        currentHero = heroCombat;
        EquipableWeapon weapon = heroCombat.EquippedWeapon;
        CreateActionUI(CombatAction.fleeAction, SkillCheck.Skill.Speed);
        if (weapon == null)
        {
            CreateActionUI(CombatAction.unarmedStrikeAction, SkillCheck.Skill.Strength, true);
        }
        else
        {
            foreach (var action in currentHero.inventory.availableCombatActions)
            {
                CreateActionUI(action, weapon.handlingType, weapon.canFocus);
            }
        }
    }

    protected void DeleteOldMarkers()
    {
        if (currentCombatActionsUIs != null)
        {
            currentCombatActionsUIs.ForEach(a => { if (a != null) Destroy(a); });
        } 
    }

    protected GameObject CreateActionUI(CombatAction a, SkillCheck.Skill skill, bool canFocus = true)
    {
        SkillCheck skillCheck = new SkillCheck(currentHero.Hero) { canFocus = canFocus, skill = skill, numberSkillChecks = a.numberSkillChecks };
        GameObject actionUI = Instantiate(actionPrefab, uiPrefabParent);
        SkillCheckButton b = actionUI.GetComponent<SkillCheckButton>();
        b.skillCheck = skillCheck;
        b.Hero = HeroCombat.CurrentActiveHeroInCombat;
        int i = currentCombatActions.Count;
        b.leftClickAction = delegate { PunBroadcastCommunication.StartCombatAction(i); };
        ApplyActionToUI(actionUI, a);
        currentCombatActionsUIs.Add(actionUI);
        currentCombatActions.Add(a);
        return actionUI;
    }
    

    public void StartAction(int index)
    {
        choosenAction = currentCombatActions[index];
        HeroCombat.currentHeroTurnInCombat.OnSelectedAction(choosenAction);
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
            Debug.Log($"{r.numberSuccessfull} successes with action {choosenAction.name}");
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

