using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private List<IEnemy> enemies = new List<IEnemy>();
    public List<IEnemy> Enemies { get { return enemies; } }
    public Transform towerPosition;

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

    public IEnemy GetClosestEnemy()
    {
        if (enemies.Count == 0 || towerPosition == null) return null;
        SortEnemiesByDistance();
        return enemies[0];
    }

    private void SortEnemiesByDistance()
    {
        if (towerPosition == null || enemies.Count == 0)
            return;

        enemies.Sort(CompareByDistance);
    }

    private int CompareByDistance(IEnemy a, IEnemy b)
    {
        if (a == null && b == null) return 0;
        if (a == null) return -1;
        if (b == null) return 1;

        float distA = (a.transform.position - towerPosition.position).sqrMagnitude;
        float distB = (b.transform.position - towerPosition.position).sqrMagnitude;
        return distA.CompareTo(distB);
    }
}
