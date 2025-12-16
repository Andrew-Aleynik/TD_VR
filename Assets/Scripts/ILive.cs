using UnityEngine;

public interface ILive
{
    int GetMaxHealth();
    void TakeDamage(int damageAmount);
    void Die(); 
}
