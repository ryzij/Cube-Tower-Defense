using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SandPathBuildService _sandPathBuildService;
    [SerializeField] private SpawnDeathEnemyController _spawnDeathControl;
    [SerializeField] private Tower _tower;

    private void OnEnable()
    {
        _gameManager.OnStateChanged += OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete += OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed += OnLastEnemyDestroyed;
        _tower.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete -= OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed -= OnLastEnemyDestroyed;
        _tower.OnGameOver -= OnGameOver;
    }

    private static void OnGameStateChanged(GameManager.GameState newState)
    {
        print("State changed, current state: " + newState);
    }

    private static void OnBuildComplete()
    {
        print("Build complete");
    }

    private void OnLastEnemyDestroyed()
    {
        print("Last enemy destroyed");
    }

    private void OnGameOver()
    {
        print("Game over");
    }
}