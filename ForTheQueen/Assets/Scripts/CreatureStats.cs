using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureStats
{

    [Range(0,95)]
    public int level;

    [Range(0, 95)]
    public int agility;

    [Range(0, 95)]
    public int strength;

    [Range(0, 95)]
    public int intelligence;

    [Range(0, 95)]
    public int talent;

    [Range(0, 95)]
    public int speed;

    [Range(0, 95)]
    public int vitality;

    [Range(0, 95)]
    public int dodge;


    public int currentHealth;

    public int armor;

    public int magicResist;

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
