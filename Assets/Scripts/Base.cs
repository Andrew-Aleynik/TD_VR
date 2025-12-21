using UnityEngine;

public class Base : MonoBehaviour, ILive
{
    public int MaxHealth;
    public int maxHealth => MaxHealth;

    private int health;
    [SerializeField] private HPBar hPBar;

    private bool _isDie = false;
    public bool IsDie { get { return _isDie; } }

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
            Destroy(gameObject);
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

    public void Die() 
    {
        _isDie = true;
    }
}
