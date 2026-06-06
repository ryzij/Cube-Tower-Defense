using UnityEngine;

public class TimerViewer : MonoBehaviour
{
    [SerializeField] private TurretsBuildTimer _timer;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMPro.TextMeshProUGUI _btnText;

    private bool _isFirstTime = true;

    private void OnEnable()
    {
        _timer.OnTick += OnTick;
        _gameManager.OnLevelChanged += OnLevelStart;
    }

    private void OnDisable()
    {
        _timer.OnTick -= OnTick;
        _gameManager.OnLevelChanged -= OnLevelStart;
    }

    private void OnTick(TurretsBuildTimer.OnTickEventArgs args)
    {
        if (_isFirstTime)
        {
            _btnText.text = $"Start ({args.SecondsLeft:0})";
            return;
        }

        _btnText.text = $"Next wave ({args.SecondsLeft:0})";
    }

    private void OnLevelStart(GameManager.OnLevelChangedEventArgs _)
    {
        _isFirstTime = false;
        _btnText.text = "Next wave";
    }
}
