using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [Header("Castle Stage (3 → 2 → 1 → 0 = Game Over)")]
    public int currentStage = 3;

    [Tooltip("Index 3 = Stage 3, Index 2 = Stage 2, Index 1 = Stage 1, Index 0 = Game Over")]
    [SerializeField] private GameObject[] stageVisuals;

    public Action<int> OnStageChanged;
    public Action OnGameOver;

    private void Start()
    {
        UpdateStageVisuals();
    }

    public void TrySetStage(int newStage)
    {
        if (newStage == -1)
        {
            foreach (var t in stageVisuals)
            {
                t.SetActive(false);
            }
            OnGameOver?.Invoke();
            Debug.Log("Castle destroyed!");
            return;
        }

        if (newStage < currentStage)
        {
            currentStage = newStage;
            Debug.Log($"Castle stage set to {currentStage}");
            OnStageChanged?.Invoke(currentStage);
            UpdateStageVisuals();
        }
    }

    private void UpdateStageVisuals()
    {
        if (stageVisuals == null)
        {
            Debug.Log("Stage visuals array is not properly configured.");
            return;
        }

        for (int i = 0; i < stageVisuals.Length; i++)
        {
            stageVisuals[i].SetActive(i == currentStage);
        }
    }
}