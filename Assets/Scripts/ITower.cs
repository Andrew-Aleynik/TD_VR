using UnityEngine;

public interface ITower : ILive
{
    void Attack(IEnemy enemy);
    void Mount();
    void Unmount();
    int damageAmount { get; }
    bool mount { get; }
}
