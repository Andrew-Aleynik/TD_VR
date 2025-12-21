using UnityEngine;

public class Projectile : MonoBehaviour
{   
    public float rotationSpeed = 1f;
    public float speed = 7f;
    private IEnemy target;
    public int damageAmount { get; set; }
    private bool canMove = false;
    private float timeToLive = 3f;

    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        if (target == null || (target as UnityEngine.Object) == null)
        {
            Destroy(gameObject);
            return;
        }
        if (canMove)
        {
            Transform enemyTransform = target.transform;

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

    public void Fly()
    {
        canMove = true;
    }
}
