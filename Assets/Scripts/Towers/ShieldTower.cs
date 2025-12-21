using System.Collections.Generic;
using UnityEngine;

public class ShieldTower : MonoBehaviour, ITower
{
    public int MaxHealth = 20;
    public int maxHealth => MaxHealth;
    private int health;
    public bool mount { get; set; } = false;
    public HPBar hpBar;
    public int damageAmount { get; } = 0;

    void Start()
    {
        health = maxHealth;
        hpBar.SetValue(maxHealth, health);
        hpBar.Hide();
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    public void Attack(IEnemy enemy)
    {
        enemy.TakeDamage(damageAmount);
    }

    public void Mount()
    {
        mount = true;
        hpBar.Show();
    }

    public void Unmount()
    {
        mount = false;
        hpBar.Hide();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        hpBar.SetValue(maxHealth, health);
        if (health <= 0)
        {
            hpBar.Hide();
            Die();
        }
    }
}
