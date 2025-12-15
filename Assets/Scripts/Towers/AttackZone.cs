using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private List<IEnemy> enemies;
    public List<IEnemy> Enemies { get { return enemies; } }

    private void OnTriggerEnter(Collider other)
    {
        var object_type = other.GetComponent<IEnemy>();
        if (object_type != null)
        {
            enemies.Add(object_type);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var object_type = other.GetComponent<IEnemy>();
        if (object_type != null)
        {
            enemies.Remove(object_type);
        }
    }
}
