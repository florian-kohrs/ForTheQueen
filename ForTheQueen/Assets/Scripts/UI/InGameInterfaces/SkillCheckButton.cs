using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckButton : MultiClickButton
{

    public SkillCheck skillCheck;

    public Action onMouseHover;

    protected Hero hero;

    public Hero Hero
    {
        set
        {
            hero = value;
            interactable = hero.IsMine;
        }
    }

    public Action leftClickAction;

    protected bool activated = false;

    public static Action currentSkillCheckAction;

    private void Start()
    {
        activated = false;
        leftClick.AddListener(()=> LeftClick());
        rightClick.AddListener(() => UseFocus());
        onBeginHover.AddListener(() => OnBeginnHover());
        onEndHover.AddListener(() => OnEndHover());
    }

    public void LeftClick()
    {
        if (activated)
            return;

        leftClickAction();
        activated = true;
    }

    public void UseFocus()
    {
        if (activated)
            return;

        if (skillCheck.CanAddFocus) //add networking to button interactions
            PunBroadcastCommunication.UseFocusRPC();
    }

    public void OnBeginnHover()
    {
        if (activated)
            return;

        onMouseHover();
    }

    public void OnEndHover()
    {
        if (activated)
            return;

        InterfaceController.Instance.RemoveMask<SkillCheckUI>();
    }

}
