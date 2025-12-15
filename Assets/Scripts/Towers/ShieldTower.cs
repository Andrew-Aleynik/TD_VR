using System.Collections.Generic;
using UnityEngine;

public class ShieldTower : MonoBehaviour, ITower
{
    public int maxHealth = 20;
    private int health;
    private bool mount = false;

    void Start()
    {
        health = GetMaxHealth();
    }


    private void Die()
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
    }

    public void Unmount()
    {
        mount = false;
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
        if (health <= 0)
        {
            Die();
        }
    }
}
