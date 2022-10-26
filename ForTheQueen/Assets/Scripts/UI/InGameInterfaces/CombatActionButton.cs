using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatActionButton : MonoBehaviour
{

    public SkillCheckButton skillCheckBtn;

    public TextMeshProUGUI percentage;

    public TextMeshProUGUI damage;

    public CombatAction combatAction;

    public void Display()
    {
        percentage.text = $"{skillCheckBtn.skillCheck.StandartSuccessPercentage}% / {skillCheckBtn.skillCheck.PerfectRate}%";
        damage.text = $"{combatAction.name}";
        if(combatAction.damage > 0)
            damage.text += $"{combatAction.damage} Damage";
    }

    //private void Start()
    //{
    //    skillCheckBtn.focu
    //}

}
