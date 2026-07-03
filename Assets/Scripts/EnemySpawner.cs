using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SandPathBuildService _pathBuildService;
    [SerializeField] private Transform _enemyHolder;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnityEvent<Enemy> _onEnemySpawned;

    public event UnityAction<Enemy> OnEnemySpawned
    {
        add => _onEnemySpawned.AddListener(value);
        remove => _onEnemySpawned.RemoveListener(value);
    }

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindAnyObjectByType<GameManager>();
        if (_enemyHolder == null)
            _enemyHolder = transform;
        if (_spawnPoint == null)
            _spawnPoint = transform;
    }

    private void OnEnable()
    {
        _gameManager.OnLevelChanged += OnLevelChanged;
    }
    private void OnDisable()
    {
        _gameManager.OnLevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(GameManager.OnLevelChangedEventArgs args) => StartCoroutine(SpawnCoroutine(args.Level));

    private IEnumerator SpawnCoroutine(Level level)
    {
        var wait = new WaitForSeconds(level.LevelTimeSec / level.EnemiesPrefabs.Count);

        foreach (var prefab in level.EnemiesPrefabs)
        {
            var enemy = prefab.Spawn(
                level.EnemiesHealthMultiplier,
                level.EnemiesSpeedMultiplier,
                level.EnemiesDamagingMultiplier,
                _pathBuildService,
                _spawnPoint,
                _enemyHolder);
            _onEnemySpawned?.Invoke(enemy);

            yield return wait;
        }
    }
}
