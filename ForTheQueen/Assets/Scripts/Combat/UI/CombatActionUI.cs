using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionUI : AdaptableInterfaceMask<EquipableWeapon>
{

    public GameObject actionPrefab;

    [SerializeField]
    protected Transform uiPrefabParent;

    protected List<GameObject> currentCombatActions = new List<GameObject>();

    protected EquipableWeapon weapon;

    protected override void AdaptUITo(EquipableWeapon weapon, Vector3 pos)
    {
        this.weapon = weapon;
        CreateActionUI(CombatAction.fleeAction, SkillCheck.Skill.Speed);
        foreach (var action in weapon.actions)
        {
            CreateActionUI(action, weapon.handlingType, weapon.canFocus);
        }
    }

    protected GameObject CreateActionUI(CombatAction a, SkillCheck.Skill skill, bool canFocus = true)
    {
        SkillCheck skillCheck = new SkillCheck() { canFocus = canFocus, skill = skill, }
        GameObject actionUI = Instantiate(actionPrefab, uiPrefabParent);
        SkillCheckButton b = actionUI.GetComponent<SkillCheckButton>();
        int i = currentCombatActions.Count;
        b.leftClickAction = delegate { PunBroadcastCommunication.StartCombatAction(i); };
        ApplyActionToUI(actionUI, a);
        currentCombatActions.Add(actionUI);
        return actionUI;
    }
    

    public void StartAction(int index)
    {
        InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUI(weapon[index].  SkillCheckUI.
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

