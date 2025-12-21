using UnityEngine;

public interface ILive
{
    void TakeDamage(int damageAmount);
    void Die(); 
    int maxHealth { get; }
}
