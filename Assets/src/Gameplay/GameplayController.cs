using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
   [SerializeField] private Castle _castle;
   [SerializeField] private WaveEnemySpawner _waveEnemySpawner;
   [SerializeField] private GameObject _playerObject;
   [SerializeField] private CameraController _cameraController;
   public void StartGame()
   {
      _castle.StartGame();
      _waveEnemySpawner.StartGame();
      _cameraController.StartGame();
   }

   public void EndGame()
   {
      _castle.EndGame();
      _waveEnemySpawner.EndGame();
      _cameraController.EndGame();
   }
}
