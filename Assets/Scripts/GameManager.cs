using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        BuildingPath,
        BuildingTurrets,
        Level
    }
    
    [SerializeField] private Level[] _levels;
    [SerializeField] private UnityEvent<GameState> _onStateChanged;
    [SerializeField] private UnityEvent<OnLevelChangedEventArgs> _onLevelChanged;

    public event UnityAction<GameState> OnStateChanged
    {
        add => _onStateChanged.AddListener(value);
        remove => _onStateChanged.RemoveListener(value);
    }

    public event UnityAction<OnLevelChangedEventArgs> OnLevelChanged
    {
        add => _onLevelChanged.AddListener(value);
        remove => _onLevelChanged.RemoveListener(value);
    }

    private int _currentLvlIndex = -1;

    private GameState _currentState = GameState.BuildingPath;

    public GameState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            _onStateChanged?.Invoke(value);
        }
    }
    
    public int CurrentLevel => _currentLvlIndex + 1;
    public int TotalLevels => _levels.Length;

    public void NextLevel()
    {
        _currentLvlIndex = (_currentLvlIndex + 1) % _levels.Length;
        CurrentState = GameState.Level;
        _onLevelChanged?.Invoke(new OnLevelChangedEventArgs
        {
            Level = _levels[_currentLvlIndex],
            CurrentLevel = CurrentLevel,
            TotalLevels = TotalLevels
        });
    }

    public class OnLevelChangedEventArgs
    {
        public Level Level { get; set; }
        public int CurrentLevel { get; set; }
        public int TotalLevels { get; set; }
        public bool IsLastLevel => CurrentLevel == TotalLevels;
    }
}
