using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegeneration : ClampedValue
{
    public bool ReduceIfRemaining(float cost)
    {
        bool result = CurrentValue >= cost;
        if (result)
        {
            CurrentValue -= cost;
        }
        return result;
    }
}
