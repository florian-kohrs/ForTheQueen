using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionButton : MonoBehaviour
{

    public SkillCheckButton skillCheckBtn;

    public TextMeshProUGUI percentage;

    public TextMeshProUGUI damage;

    public CombatAction combatAction;

    public Image backgroundImage;

    public Color selectColor = (Color.red + Color.yellow) / 2;

    public Color defaultColor = Color.gray;

    public void Display()
    {
        percentage.text = $"{skillCheckBtn.SkillCheck.StandartSuccessPercentage}% / {skillCheckBtn.SkillCheck.PerfectRate}%";
        damage.text = $"{combatAction.name}";
        if(combatAction.damage > 0)
            damage.text += $"{combatAction.damage} Damage";
    }

    public void Select()
    {
        backgroundImage.color = selectColor;
    }

    public void Unselect()
    {
        backgroundImage.color = defaultColor;
    }

    //private void Start()
    //{
    //    skillCheckBtn.focu
    //}

}
