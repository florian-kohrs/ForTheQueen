using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckButton : MultiClickButton
{

    protected SkillCheck skillCheck;

    public SkillCheck SkillCheck => skillCheck;

    public void SetSkillCheck(SkillCheck skillCheck)
    {
        this.skillCheck = skillCheck;
        skillCheckImage.sprite = GenerellLookup.instance.spriteLookup.SpriteFromSkillCheck(skillCheck.skill);
    }

    public Image skillCheckImage;

    public Action onMouseHover;

    protected Hero hero;

    public Hero Hero
    {
        set
        {
            hero = value;
            skillCheck.hero = value;
            interactable = hero.IsMine;
        }
    }

    public Action leftClickAction;

    //protected bool activated = false;

    public static Action currentSkillCheckAction;

    private void Start()
    {
        if (!hero.IsMine)
            return;
        //activated = false;
        leftClick.AddListener(()=> LeftClick());
        rightClick.AddListener(() => UseFocus());
        onBeginHover.AddListener(() => OnBeginnHover());
        onEndHover.AddListener(() => OnEndHover());
    }

    public void LeftClick()
    {
        //if (activated)
        //    return;

        leftClickAction();
        //activated = true;
    }

    public void UseFocus()
    {
        //if (activated)
        //    return;

        if (SkillCheck.CanAddFocus) //add networking to button interactions
            PunBroadcastCommunication.UseFocusRPC();
    }

    public void OnBeginnHover()
    {
        //if (activated)
        //    return;

        onMouseHover();
    }

    public void OnEndHover()
    {
        //if (activated)
        //    return;

        //InterfaceController.Instance.RemoveMask<SkillCheckUI>();
    }

}
