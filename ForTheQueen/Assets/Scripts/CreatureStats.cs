using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureStats
{

    public int level;

    public int agility;

    public int strength;

    public int intelligence;

    public int talent;

    public int speed;

    public int dodge;

    public int vitality;

    public int currentHealth;

    public int MaxHealth => Mathf.RoundToInt(vitality / 10f * level);

    public int GetStatsOfSkill(SkillCheck.Skill skill)
    {
        switch (skill)
        {
            case SkillCheck.Skill.Speed:
                return speed;
            case SkillCheck.Skill.Strength:
                return strength;
            case SkillCheck.Skill.Talent:
                return talent;
            case SkillCheck.Skill.Agility:
                return agility;
            case SkillCheck.Skill.Vitality:
                return vitality;
            case SkillCheck.Skill.Intelligence:
                return intelligence;
            default:
                throw new System.Exception($"No sprite available for skill{skill}");
        }
    }

}
