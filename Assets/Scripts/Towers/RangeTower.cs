using UnityEngine;
using System.Collections.Generic;

public class RangeTower : MonoBehaviour, ITower
{
    public int maxHealth = 10;
    public int damageAmount = 4;
    public Collider attackRange;
    public float attackInterval = 1f;
    public GameObject projectilePrefab;
    public Transform projectileStartPosition;
    private float attackCooldown = 0f;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    private int health;
    private bool mount = false;

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
        return GetAvailableEnemies().Count > 0;
    }

    private IEnemy GetEnemyToAttack()
    {
        List<Collider> enemies = GetAvailableEnemies();

        Collider nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider enemy in enemies)
        {
            if (enemy == null) continue;
            
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy.GetComponent<IEnemy>();
    }

    private List<Collider> GetAvailableEnemies()
    {
        List<Collider> availableEnemies = new List<Collider>();

        if (attackRange is BoxCollider boxCollider)
        {
            Vector3 center = attackRange.transform.TransformPoint(boxCollider.center);
            Vector3 halfExtents = boxCollider.size * 0.5f;
            Quaternion orientation = attackRange.transform.rotation;

            Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);

        
            foreach (Collider col in colliders)
            {
                if (col.GetComponent<IEnemy>() != null)
                {
                    availableEnemies.Add(col);
                }
            }
        }

        return availableEnemies;
    }

    private void Die()
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
        
        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        
        if (projectileScript != null)
        {
            projectileScript.SetTarget(enemy);
            projectileScript.SetDamageAmount(GetDamageAmount());
            projectileScript.Fly();
        }
        else
        {
            Debug.LogError("Projectile prefab doesn't have Projectile script!");
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
