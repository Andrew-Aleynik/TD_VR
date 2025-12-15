using System.Collections.Generic;
using UnityEngine;

public class MeleeTower : MonoBehaviour, ITower
{
    public int maxHealth = 10;
    public int damageAmount = 2;
    public float attackInterval = 0.5f;
    private float attackCooldown = 0f;
    private int health;
    private bool mount = false;

    public AttackZone Attack_zone;

    void Start()
    {
        health = GetMaxHealth();
    }

    void Update()
    {
        if (!HasTargetsInAttackRange() || !IsMount())
        {
            return;
        }

        if (attackCooldown <= 0f)
        {
            var enemies_to_attack = GetAvailableEnemies();
            foreach (IEnemy enemy in enemies_to_attack)
            {
                Attack(enemy);
            }
            attackCooldown = attackInterval;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private bool HasTargetsInAttackRange()
    {
        return GetAvailableEnemies().Count > 0;
    }

    private List<IEnemy> GetAvailableEnemies()
    {
       return Attack_zone.Enemies;
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
        return damageAmount;
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
