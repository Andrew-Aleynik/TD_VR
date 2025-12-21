using UnityEngine;
using System.Collections.Generic;

public class RangeTower : MonoBehaviour, ITower
{
    public int MaxHealth = 10;
    public int maxHealth => MaxHealth;
    public int DamageAmount = 4;
    public int damageAmount => DamageAmount;
    public AttackZone attackZone;
    public float attackInterval = 1f;
    public GameObject projectilePrefab;
    public Transform projectileStartPosition;
    private float attackCooldown = 0f;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    private int health;
    public bool mount { get; set; } = false;
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
            IEnemy enemy = GetEnemyToAttack();
            if (enemy != null) {
                Attack(enemy);
                attackCooldown = attackInterval;
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private bool HasTargetsInAttackRange()
    {
        attackZone.Enemies.RemoveAll(enemy => 
        enemy == null || (enemy as UnityEngine.Object) == null);
        return attackZone.Enemies.Count > 0;
    }

    private IEnemy GetEnemyToAttack()
    {
        return attackZone.GetClosestEnemy();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void ThrowProjectile(IEnemy enemy)
    {
        GameObject projectileInstance = Instantiate(
            projectilePrefab,
            projectileStartPosition.position,
            projectileStartPosition.rotation
        );
        GameManager.Instance.projectilePool.Add(projectileInstance);
        
        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        
        if (projectileScript != null)
        {
            projectileScript.SetTarget(enemy);
            projectileScript.damageAmount = damageAmount;
            projectileScript.Fly();
        }
    }

    public void Attack(IEnemy enemy)
    {
        ThrowProjectile(enemy);
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
