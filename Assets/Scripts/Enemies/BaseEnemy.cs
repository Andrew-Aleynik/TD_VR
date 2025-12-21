using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    public int DamageAmount = 2;
    public int damageAmount => DamageAmount;
    public float Speed = 3f;
    public float speed => Speed;
    public int MaxHealth = 10;
    public int maxHealth => MaxHealth;
    public int Value = 5;
    public int value => Value;
    public Vector3 movementDirection;
    private int health;
    private List<ITower> towersInRange = new List<ITower>();
    private float attackCooldown = 0f;
    public float attackInterval = 1f;
    public HPBar hpBar;

    void Start()
    {
        health = maxHealth;
        transform.rotation = Quaternion.LookRotation(movementDirection);
        hpBar.SetValue(maxHealth, health);
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
        GameManager.Instance.AddScore(value);
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
        tower.TakeDamage(damageAmount);
    }

    public void MakeStep() 
    {
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        hpBar.SetValue(maxHealth, health);
        if (health <= 0)
        {
            Die();
        }
    }
}
