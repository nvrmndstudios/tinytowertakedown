using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("View Points")]
    [SerializeField] private Transform startView;
    [SerializeField] private Transform endView;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        EndGame();
    }

    public void StartGame()
    {
        StartCoroutine(WaitForADelay());
      
    }

    private IEnumerator WaitForADelay()
    {
        yield return new WaitForSeconds(1f);
        if (startView != null)
            StartTransitionTo(startView);
    }

    public void EndGame()
    {
        if (endView != null)
            StartTransitionTo(endView);
    }

    private void StartTransitionTo(Transform targetView)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(SmoothTransition(targetView));
    }

    private IEnumerator SmoothTransition(Transform target)
    {
        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            transform.position = Vector3.Lerp(initialPosition, target.position, t);
            transform.rotation = Quaternion.Slerp(initialRotation, target.rotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}