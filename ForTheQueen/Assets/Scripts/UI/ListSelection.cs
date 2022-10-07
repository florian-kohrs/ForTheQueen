using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListSelection : MonoBehaviour
{

    [SerializeField]
    protected DesignerList availableItems;

    protected List<ApplyableAssets> AvailableItems => availableItems.list;

    [SerializeField]
    protected TextMeshProUGUI selectionText;

    protected int currentSelectedIndex = 0;

    [SerializeField]
    protected Transform applyTarget;

    protected int MaxItems => AvailableItems.Count;

    protected ApplyableAssets SelectedAsset => AvailableItems[currentSelectedIndex];

    protected GameObject equippedItem;

    protected CallbackSet<Action> callbackSet = new CallbackSet<Action>();

    protected Hero h;

    public void SetHero(Hero h) 
    {
        this.h = h;
        UpdateSelection(0);
    }

    public int CurrentSelectedIndex
    {
        get { return currentSelectedIndex; }
    }

    protected void ChangeCurrentSelectedIndex(int delta)
    {
        currentSelectedIndex += delta;
        if (currentSelectedIndex < 0)
            currentSelectedIndex += MaxItems;
        else
            currentSelectedIndex = currentSelectedIndex % MaxItems; 
    }

    public void AddDesignChangeListener(Action onDesignChanged)
    {
        callbackSet.AddSubscriber(onDesignChanged);
    }

    public void OpenSelection()
    {
        gameObject.SetActive(true);
    }

    public void UpdateSelection(int index)
    {
        ChangeItem(index - currentSelectedIndex);
    }

    public void ChangeItem(int sign)
    {
        ClearItem();
        ChangeCurrentSelectedIndex(sign);
        ApplyItem();
        callbackSet.CallForEachSubscriber(s => s.Invoke());
    }

    protected void ApplyItem()
    {
        ClearItem();
        equippedItem = AvailableItems[CurrentSelectedIndex].Apply(applyTarget, h);
        selectionText.text = SelectedAsset.description;
    }

    protected void ClearItem()
    {
        if (equippedItem != null) 
        {
            AvailableItems[CurrentSelectedIndex].Remove(equippedItem, h);
            equippedItem = null;
        }
    }

}
