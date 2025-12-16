using UnityEngine;

public class Base : MonoBehaviour, ILive
{
    public int maxHealth;
    private int health;
    public HPBar hPBar;

    public void Start()
    {
        health = GetMaxHealth();
        hPBar.SetValue(GetMaxHealth(), health);
        hPBar.Show();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var object_type = other.GetComponent<IEnemy>();
        if (object_type != null)
        {
            object_type.Die();
            TakeDamage(1);
        }
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damageAmount) 
    {
        health -= damageAmount;
        hPBar.SetValue(GetMaxHealth(), health);
        if (health <= 0)
        {
            Die();
        }
    }

    //TODO notify GameManager
    public void Die() 
    {
        return;
    }
}
