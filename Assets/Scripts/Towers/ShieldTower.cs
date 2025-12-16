using System.Collections.Generic;
using UnityEngine;

public class ShieldTower : MonoBehaviour, ITower
{
    public int maxHealth = 20;
    private int health;
    private bool mount = false;
    public HPBar hpBar;

    void Start()
    {
        health = GetMaxHealth();
        hpBar.SetValue(GetMaxHealth(), health);
        hpBar.Hide();
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    public void Attack(IEnemy enemy)
    {
        enemy.TakeDamage(GetDamageAmount());
    }

    public int GetDamageAmount()
    {
        return 0;
    }

    public void Mount()
    {
        mount = true;
        hpBar.Show();
        Debug.Log("Mount");
    }

    public void Unmount()
    {
        mount = false;
        hpBar.Hide();
        Debug.Log("Unmount");
    }

    public bool IsMount()
    {
        return mount;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        hpBar.SetValue(GetMaxHealth(), health);
        if (health <= 0)
        {
            hpBar.Hide();
            Die();
        }
    }
}
