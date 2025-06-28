using System;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    private Castle castle;
    public bool IsDead { get; private set; } = false;
    public Action OnDeath;

    [Header("Check Settings")]
    [SerializeField] private float checkInterval = 0.5f;

    private float stage2Radius = 6f;
    private float stage1Radius = 3f;
    private float deathRadius = 1f;
    private float checkTimer;
    
    
    public void MarkAsDead()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"{gameObject.name} is dead");
        // Optional: Add animation, effects, or disable collider
        OnDeath?.Invoke();
        Destroy(gameObject); // Or customize
    }
    
  

    private void Update()
    {
        if (castle == null) return;

        Vector3 direction = (castle.transform.position - transform.position).normalized;
        transform.position += direction * (moveSpeed * Time.deltaTime);

        // Optional: rotate to face center
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
        
        checkTimer -= Time.deltaTime;

        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            CheckDistanceAndUpdateCastle();
        }
    }

    public void SetCenterPoint(Castle cstl, float stage2Rad, float stage1Rad, float deathRad)
    {
        stage2Radius = stage2Rad;
        stage1Radius = stage1Rad;
        deathRadius = deathRad;
        castle = cstl;
    }
    
    void CheckDistanceAndUpdateCastle()
    {
        if (castle == null) return;

        float dist = Vector3.Distance(transform.position, castle.transform.position);

        if (dist <= deathRadius)
        {
            castle.TrySetStage(0); // Game Over
        }
        else if (dist <= stage1Radius)
        {
            castle.TrySetStage(0);
        }
        else if (dist <= stage2Radius)
        {
            castle.TrySetStage(1);
        }
    }
}