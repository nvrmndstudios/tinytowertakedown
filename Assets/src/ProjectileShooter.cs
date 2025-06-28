using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform directionSource; // Usually the rotating parent
    [SerializeField] private float detectRange = 1000f;
    [SerializeField] private float shootCooldown = 1f;

    [SerializeField] private RotateTowardsClick rotateTowardsClick;

    private float shootTimer = 0f;

    void Update()
    {
        if(rotateTowardsClick == null) return;

        if (rotateTowardsClick.CanShoot)
        {
            shootTimer -= Time.deltaTime;

            Vector3 origin = directionSource.position;
            Vector3 direction = directionSource.forward;

            // Draw the detection ray in the Scene view
            Debug.DrawRay(origin, direction * detectRange, Color.red);

            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, detectRange, targetLayer))
            {
                if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
                {
                    if (shootTimer <= 0f)
                    {
                        FireAtTarget(hit.collider.transform);
                        shootTimer = shootCooldown;
                    }
                }
            }
        }
    }

    private void FireAtTarget(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, directionSource.position, directionSource.rotation);
        SoundManager.Instance.PlayShoot();
        projectile.GetComponent<Projectile>().Initialize(target);
    }
}