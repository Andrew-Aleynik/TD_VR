using UnityEngine;

public class Projectile : MonoBehaviour
{   
    public float rotationSpeed = 1f;
    public float speed = 7f;
    private IEnemy target;
    private int damageAmount;
    private bool canMove = false;

    void Update()
    {
        if (target == null || (target as UnityEngine.Object) == null)
        {
            Destroy(gameObject);
            return;
        }
        if (canMove)
        {
            Transform enemyTransform = target.GetTransform();

            Vector3 targetPosition = enemyTransform.position;
            
            Vector3 direction = (targetPosition - transform.position).normalized;
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    rotationSpeed * Time.deltaTime
                );
            }
            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget > 100f || transform.position.y < -1)
            {
                Destroy(gameObject);
            }
            else if (distanceToTarget < 1f) 
            {
                target.TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(IEnemy enemy)
    {
        target = enemy;
    }

    public void SetDamageAmount(int damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    public void Fly()
    {
        canMove = true;
    }
}
