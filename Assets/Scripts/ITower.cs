using UnityEngine;

public interface ITower : ILive
{
    void Attack(IEnemy enemy);

    int GetDamageAmount();

    void Mount();

    void Unmount();

    bool IsMount();
}
