﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController
{

    private static InterfaceController instance;

    public static InterfaceController Instance => instance;

    static InterfaceController()
    {
        instance = new InterfaceController();
    }

    protected InterfaceController()
    {
        GameManagerPun.SetInterfaceController(this);
    }

    protected HashSet<IInterfaceMask> allInterfaceMasks = new HashSet<IInterfaceMask>();

    public void AddPossibleInterfaceMask(IInterfaceMask mask) => allInterfaceMasks.Add(mask);

    public static T GetInterfaceMask<T>() where T : IInterfaceMask
    {
        Type requestedType = typeof(T);
        return (T)instance.allInterfaceMasks.Where(m => m.GetType().IsEquivalentTo(requestedType)).FirstOrDefault();
    }

    private HashSet<IInterfaceMask> activeMasks = 
        new HashSet<IInterfaceMask>();


    //[SerializeField]
    //private Text text;

    //[SerializeField]
    //private GameObject textContainer;

    //private IEnumerator textDismisser;


    public void AddMask(IInterfaceMask mask, bool closeAllOtherMasks = false)
    {
        if (closeAllOtherMasks)
        {
            Clear();
        }
        activeMasks.Add(mask);
        ApplyMask(mask);
        mask.Open();
    }

    public void Clear()
    {
        foreach(IInterfaceMask m in activeMasks)
        {
            RemoveMask(m);
        }
        activeMasks.Clear();
    }

    public void ForceMask(IInterfaceMask mask)
    {
        activeMasks.Add(mask);
        ApplyMask(mask);
        mask.Open();
    }

    private void ApplyMask(IInterfaceMask mask)
    {
        Cursor.lockState = mask.CursorMode;
        switch (mask.CursorMode)
        {
            case CursorLockMode.Confined:
            case CursorLockMode.None:
                Cursor.visible = true;
                break;

            case CursorLockMode.Locked:
                Cursor.visible = false;
                break;
        }
    }

    public void RemoveMask(IInterfaceMask mask)
    {
        activeMasks.Remove(mask);
        mask.Close();
        DetermineCursorMode();
    }

    protected void DetermineCursorMode()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    //public static void DisplayText(string text, float duration = 2)
    //{
    //    Interface.DisplayText_(text, duration);
    //}

    //private void DisplayText_(string text, float duration = 2)
    //{
    //    if(textDismisser != null)
    //    {
    //        StopCoroutine(textDismisser);
    //    }
    //    textContainer.SetActive(true);
    //    this.text.text = text;
    //    textDismisser = this.DoDelayed(duration, delegate
    //    {
    //        DismissText();
    //    });
    //}

    //private void DismissText()
    //{
    //    textContainer.SetActive(false);
    //}
    
}