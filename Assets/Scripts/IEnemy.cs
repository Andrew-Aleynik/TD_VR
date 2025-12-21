using UnityEngine;

public interface IEnemy : ILive, IValuable
{
    void Attack(ITower tower);
    void MakeStep();
    int damageAmount { get; }
    float speed { get; }
    Transform transform { get; }
}
