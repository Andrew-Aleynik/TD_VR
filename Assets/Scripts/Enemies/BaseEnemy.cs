using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    public int damageAmount = 2;
    public float speed = 3f;
    public int maxHealth = 10;
    public int value = 5;
    public Vector3 movementDirection;
    private int health;
    private List<ITower> towersInRange = new List<ITower>();
    private float attackCooldown = 0f;
    public float attackInterval = 1f;
    public HPBar hpBar;

    void Start()
    {
        health = GetMaxHealth();
        transform.rotation = Quaternion.LookRotation(movementDirection);
        hpBar.SetValue(GetMaxHealth(), health);
        hpBar.Show();
    }

    void Update()
    {
        if (!HasTargetsInAttackRange())
        {
            MakeStep();
        }
        else
        {
            if (attackCooldown <= 0f)
            {
                ITower tower = GetTowerToAttack();
                if (tower != null) {
                    Attack(tower);
                    attackCooldown = attackInterval;
                }
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    private bool HasTargetsInAttackRange()
    {
        return towersInRange.Count > 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ITower tower = other.GetComponent<ITower>();
        if (tower != null)
        {
            if (!towersInRange.Contains(tower))
            {
                towersInRange.Add(tower);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ITower tower = other.GetComponent<ITower>();
        if (towersInRange.Contains(tower))
        {
            towersInRange.Remove(tower);
        }
    }

    private ITower GetTowerToAttack() 
    {
        towersInRange.RemoveAll(tower => 
        tower == null || (tower as UnityEngine.Object) == null);
        if (towersInRange.Count > 0)
        {
            return towersInRange[0];
        }
        return null;
    }

    public void Attack(ITower tower)
    {
        tower.TakeDamage(GetDamageAmount());
    }

    public void MakeStep() 
    {
        transform.position += movementDirection * GetSpeed() * Time.deltaTime;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        hpBar.SetValue(GetMaxHealth(), health);
        if (health <= 0)
        {
            Die();
        }
    }

    public int GetDamageAmount() 
    {
        return damageAmount;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetValue()
    {
        return value;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
