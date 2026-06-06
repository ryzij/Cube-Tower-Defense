using UnityEngine;
using UnityEngine.Events;

public class WinLoseController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SpawnDeathEnemyController _spawnDeathEnemyController;
    [SerializeField] private Tower _tower;
    [SerializeField] private UnityEvent<OnGameOverEventArgs> _onGameOver;
    [SerializeField] private UnityEvent _onWin;

    public event UnityAction<OnGameOverEventArgs> OnGameOver
    {
        add => _onGameOver.AddListener(value);
        remove => _onGameOver.RemoveListener(value);
    }

    public event UnityAction OnWin
    {
        add => _onWin.AddListener(value);
        remove => _onWin.RemoveListener(value);
    }

    private bool _isLastLevel;
    private int _currentLevel;

    private void OnEnable()
    {
        _gameManager.OnLevelChanged += OnLevelChanged;
        _spawnDeathEnemyController.OnLastEnemyDestroyed += OnLastEnemyDestroyed;
        _tower.OnGameOver += OnDeath;
    }

    private void OnDisable()
    {
        _gameManager.OnLevelChanged -= OnLevelChanged;
        _spawnDeathEnemyController.OnLastEnemyDestroyed -= OnLastEnemyDestroyed;
        _tower.OnGameOver -= OnDeath;
    }

    private void OnLastEnemyDestroyed()
    {
        if (!_isLastLevel)
            return;

        _gameManager.CurrentState = GameManager.GameState.Win;
        _onWin?.Invoke();
    }

    private void OnLevelChanged(GameManager.OnLevelChangedEventArgs args)
    {
        _isLastLevel = args.IsLastLevel;
        _currentLevel = args.CurrentLevel;
    }

    private void OnDeath()
    {
        _gameManager.CurrentState = GameManager.GameState.Lose;
        _onGameOver?.Invoke(new OnGameOverEventArgs(_currentLevel));
    }

    public class OnGameOverEventArgs
    {
        public int CurrentLevel { get; }
        public OnGameOverEventArgs(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
    }
}