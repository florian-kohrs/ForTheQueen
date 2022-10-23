using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleInfoUI : MonoBehaviour
{

    public TextMeshProUGUI enemyName;

    public TextMeshProUGUI armor;

    public TextMeshProUGUI health;

    public TextMeshProUGUI magicResist;

    public Image maxHealthImage;

    public Transform buffParent;

    protected int maxHealth;

    protected int currentHealth;

    public void ApplySingleEnemy(IBattleParticipant e)
    {
        enemyName.text = e.Name;
        armor.text = e.Armor.ToString();
        magicResist.text = e.MagicResist.ToString();
        maxHealth = e.MaxHealth;
        currentHealth = maxHealth;
        UpdateHealth();
    }

    public void ReduceHealth(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);
        UpdateHealth();
    }

    protected void UpdateHealth()
    {
        maxHealthImage.fillAmount = currentHealth / (float)maxHealth;
        health.text = $"{currentHealth}/{maxHealth}";
    }

}
