using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver
{

    public Quiver(ProjectileItem item)
    {
        this.item = item;
        amount = item.stackAmount;
    }

    public Quiver(ProjectileItem item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ProjectileItem item;

    public int amount;

}
