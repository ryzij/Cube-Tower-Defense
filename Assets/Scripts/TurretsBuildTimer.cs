using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TurretsBuildTimer : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _buildTime = 10f;
    [SerializeField] private UnityEvent<OnTickEventArgs> _onTick;

    public event UnityAction<OnTickEventArgs> OnTick
    {
        add => _onTick.AddListener(value);
        remove => _onTick.RemoveListener(value);
    }

    private static readonly WaitForSeconds _wait = new(1f);

    private void OnEnable()
    {
        _gameManager.OnStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.BuildingPath)
            return;

        StopCoroutine(nameof(Timer));
        if (newState == GameManager.GameState.BuildingTurrets)
            StartCoroutine(nameof(Timer));
    }

    private IEnumerator Timer()
    {
        for (float i = 0; i < _buildTime; i++)
        {
            _onTick?.Invoke(new OnTickEventArgs
            {
                ElapsedSeconds = i,
                TotalSeconds = _buildTime
            });
            yield return _wait;
        }

        _gameManager.NextLevel();
    }

    public class OnTickEventArgs
    {
        public float SecondsLeft => TotalSeconds - ElapsedSeconds;
        public float ElapsedSeconds { get; set; }
        public float TotalSeconds { get; set; }
    }
}
