using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SandPathBuildService _sandPathBuildService;
    [SerializeField] private SpawnDeathEnemyController _spawnDeathControl;

    private void OnEnable()
    {
        _gameManager.OnStateChanged += OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete += OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed += OnLastEnemyDestroyed;
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete -= OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed -= OnLastEnemyDestroyed;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        print("State changed, current state: " + newState);
    }

    private static void OnBuildComplete()
    {
        print("Build complete");
    }

    private static void OnLastEnemyDestroyed()
    {
        print("Last enemy destroyed");
    }
}