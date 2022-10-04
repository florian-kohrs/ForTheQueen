using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory
{

    /// <summary>
    /// item is key, itemcount is value
    /// </summary>
    public Dictionary<InventoryItemRef, int> items = new Dictionary<InventoryItemRef, int>();

    [System.NonSerialized]
    public System.Action OnInventoryChanged;

    public void AddItems(IEnumerable<ItemContainer> items)
    {
        foreach (ItemContainer i in items)
        {
            AddItem(i);
        }
    }

    public void AddItem(InventoryItem item, int count = 1)
    {
        InventoryItemRef itemRef = new InventoryItemRef();
        itemRef.RuntimeRef = item;
        AddItem(itemRef, count);
    }

    public void AddItem(ItemContainer item)
    {
        AddItem(item.item, item.itemCount);
    }

    public void AddItem(InventoryItemRef item, int count = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] += count;
        }
        else
        {
            items.Add(item,count);
        }
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItems(IEnumerable<ItemContainer> cs)
    {
        foreach(ItemContainer c in cs)
        {
            RemoveItem(c.item, c.itemCount, false);
        }
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(InventoryItem i, int count = 1, bool updateUI = true)
    {
        return RemoveItem(new InventoryItemRef() { RuntimeRef = i }, count, updateUI);
    }

    public bool RemoveItem(InventoryItemRef item, int count = 1, bool callInventoryChanged = true)
    {
        int itemCount;
        bool result = items.TryGetValue(item, out itemCount) && itemCount >= count;
        
        if (result)
        {
            itemCount -= count;
            if(itemCount == 0)
            {
                items.Remove(item);
            }
            else
            {
                items[item] = itemCount;
            }
            if (callInventoryChanged)
            {
                OnInventoryChanged?.Invoke();
            }
        }

        return result;
    }

    public bool GotItems(IEnumerable<ItemContainer> items)
    {
        bool result = true;
        foreach (ItemContainer i in items)
        {
            result = result && HasItem(i);
        }
        return result;
    }

    public bool HasItem(ItemContainer item)
    {
        return HasItem(item.item, item.itemCount);
    }

    public bool HasItem(InventoryItem i, int count)
    {
        return HasItem(new InventoryItemRef() { RuntimeRef = i }, count);
    }

    public bool HasItem(InventoryItemRef i, int count)
    {
        int itemCount;

        items.TryGetValue(i, out itemCount);

        return itemCount >= count;
    }

    public void AddItems(IEnumerable<ItemContainer> items, int count = 1)
    {
        foreach (ItemContainer i in items)
        {
            AddItem(i);
        }
    }


}
