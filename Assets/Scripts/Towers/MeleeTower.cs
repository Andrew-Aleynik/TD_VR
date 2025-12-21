using System.Collections.Generic;
using UnityEngine;

public class MeleeTower : MonoBehaviour, ITower
{
    public int MaxHealth = 10;
    public int maxHealth => MaxHealth;
    public int DamageAmount = 2;
    public int damageAmount => DamageAmount;
    public float attackInterval = 0.5f;
    private float attackCooldown = 0f;
    private int health;
    public bool mount { get; set; } = false;

    public AttackZone Attack_zone;
    public HPBar hpBar;

    void Start()
    {
        health = maxHealth;
        hpBar.SetValue(maxHealth, health);
        hpBar.Hide();
    }

    void Update()
    {
        if (!HasTargetsInAttackRange() || !mount)
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
        Attack_zone.Enemies.RemoveAll(enemy => 
        enemy == null || (enemy as UnityEngine.Object) == null);
        return Attack_zone.Enemies;
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
