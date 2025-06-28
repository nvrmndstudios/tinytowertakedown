using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed = 40f;
    private bool lostTarget = false;
    private float timeoutAfterLostTarget = 2f;
    private float lostTargetTimer = 0f;

    private Vector3 lastKnownDirection;

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            if (!lostTarget)
            {
                lostTarget = true;
                lostTargetTimer = timeoutAfterLostTarget;
                lastKnownDirection = transform.forward; // Cache forward direction
            }

            lostTargetTimer -= Time.deltaTime;
            transform.position += lastKnownDirection * (speed * Time.deltaTime);

            if (lostTargetTimer <= 0f)
            {
                Destroy(gameObject);
            }

            return;
        }

        // Move toward the target
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * (speed * Time.deltaTime);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            var t = target.GetComponent<Targetable>();
            if (t != null)
            {
                t.MarkAsDead();
            }

            Destroy(gameObject);
        }
    }
}