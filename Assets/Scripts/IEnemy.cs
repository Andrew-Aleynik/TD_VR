using UnityEngine;

public interface IEnemy : ILive, IValuable
{
    void Attack(ITower tower);
    int GetDamageAmount();
    void MakeStep();
    float GetSpeed();
    Transform GetTransform();
}
