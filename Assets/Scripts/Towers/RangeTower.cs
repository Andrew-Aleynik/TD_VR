using UnityEngine;
using System.Collections.Generic;

public class RangeTower : MonoBehaviour, ITower
{
    public int maxHealth = 10;
    public int damageAmount = 4;
    public AttackZone attackZone;
    public float attackInterval = 1f;
    public GameObject projectilePrefab;
    public Transform projectileStartPosition;
    private float attackCooldown = 0f;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    private int health;
    private bool mount = false;
    public HPBar hpBar;

    void Start()
    {
        health = GetMaxHealth();
        hpBar.SetValue(GetMaxHealth(), health);
        hpBar.Hide();
    }

    void Update()
    {
        if (!HasTargetsInAttackRange() || !IsMount())
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
        if (attackZone.Enemies.Count > 0) 
        {
            return attackZone.Enemies[0];
        }
        return null;
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
            projectileScript.SetDamageAmount(GetDamageAmount());
            projectileScript.Fly();
        }
    }

    public void Attack(IEnemy enemy)
    {
        ThrowProjectile(enemy);
    }

    public int GetDamageAmount()
    {
        return damageAmount;
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
