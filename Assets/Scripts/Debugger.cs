using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SandPathBuildService _sandPathBuildService;
    [SerializeField] private SpawnDeathEnemyController _spawnDeathControl;
    [SerializeField] private Tower _tower;
    [SerializeField] private BlockDetector _blockDetector;

    private int _blockCount;

    private void OnEnable()
    {
        _gameManager.OnStateChanged += OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete += OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed += OnLastEnemyDestroyed;
        _tower.OnGameOver += OnGameOver;
        _blockDetector.OnBlockClicked += OnBlockClicked;
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= OnGameStateChanged;
        _sandPathBuildService.OnBuildComplete -= OnBuildComplete;
        _spawnDeathControl.OnLastEnemyDestroyed -= OnLastEnemyDestroyed;
        _tower.OnGameOver -= OnGameOver;
        _blockDetector.OnBlockClicked -= OnBlockClicked;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        print("State changed, current state: " + newState);

        if (newState == GameManager.GameState.BuildingTurrets)
            print("Total blocks in path: " + _blockCount);
    }

    private static void OnBuildComplete()
    {
        print("Build complete");
    }

    private static void OnLastEnemyDestroyed()
    {
        print("Last enemy destroyed");
    }

    private static void OnGameOver()
    {
        print("Game over");
    }

    private void OnBlockClicked(BlockScript block)
    {
        if (_gameManager.CurrentState == GameManager.GameState.BuildingPath)
            print("Block: " + ++_blockCount + " - " + block);
    }
}