using UnityEngine;

public class RotateTowardsClick : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool rotateOnlyYAxis = true;
    [SerializeField] private float rotationSpeed = 5f; // Degrees per second

    private Quaternion targetRotation;
    private bool shouldRotate = false;

    public bool CanShoot => !shouldRotate;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            {
                Vector3 targetPoint = hit.point;
                Vector3 direction = targetPoint - transform.position;

                if (rotateOnlyYAxis)
                {
                    direction.y = 0f;
                }

                if (direction != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(direction);
                    shouldRotate = true;
                }
            }
        }

        if (shouldRotate)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation; // Snap to target when very close
                shouldRotate = false;
            }
        }
    }
}