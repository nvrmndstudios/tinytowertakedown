using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [Header("Castle Stage (3 → 2 → 1 → 0 = Game Over)")]
    public int currentStage = 2;

    [Tooltip("Index 3 = Stage 3, Index 2 = Stage 2, Index 1 = Stage 1, Index 0 = Game Over")]
    [SerializeField] private GameObject[] stageVisuals;

    [SerializeField] private GameObject _explosion;

    public Action<int> OnStageChanged;
    public Action OnGameOver;

    public bool CastleDestroyed = false;

    public void StartGame()
    {
        CastleDestroyed = false;
        currentStage = 2;
        UpdateStageVisuals();
        Debug.Log("Castle reset to stage 3");
    }

    public void EndGame()
    {
        
    }

    public void TrySetStage(int newStage)
    {
        if (CastleDestroyed)
        {
            return;
        }

        if (newStage == -1)
        {
            currentStage = newStage;
            CastleDestroyed = true;
            GameManager.Instance.UpdateLifeCount(0);
            GameManager.Instance.ChangeState(GameManager.GameState.Result);
            DoExplosion();
            return;
        }

        if (newStage < currentStage)
        {
            currentStage = newStage;
            GameManager.Instance.UpdateLifeCount(currentStage);
            Debug.Log($"Castle stage set to {currentStage}");
            OnStageChanged?.Invoke(currentStage);
            DoExplosion();
            UpdateStageVisuals();
        }
    }

    private void DoExplosion()
    {
        var post = transform.position;
        post.y += 1;
        var exp = Instantiate(_explosion, post, Quaternion.identity);
    }

    private void UpdateStageVisuals()
    {
        if (stageVisuals == null || stageVisuals.Length == 0)
        {
            Debug.LogWarning("Stage visuals not configured.");
            return;
        }

        for (int i = 0; i < stageVisuals.Length; i++)
        {
            if (stageVisuals[i] != null)
                stageVisuals[i].SetActive(i == currentStage);
        }
    }
}