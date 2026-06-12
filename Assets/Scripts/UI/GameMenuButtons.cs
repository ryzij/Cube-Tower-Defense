using UnityEngine;
using UnityEngine.UI;

public class GameMenuButtons : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SandPathBuildService _sandPathBuildService;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _resetPathButton;

    private void OnEnable()
    {
        _gameManager.OnStateChanged += OnStateChanged;
        _gameManager.OnLevelChanged += OnLevelChanged;
        _mainMenuButton.onClick.AddListener(ExitToMainMenu);
        
        if (_resetPathButton != null)
            _resetPathButton.onClick.AddListener(OnResetButtonClick);
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= OnStateChanged;
        _gameManager.OnLevelChanged -= OnLevelChanged;
        _mainMenuButton.onClick.RemoveListener(ExitToMainMenu);

        if (_resetPathButton != null)
            _resetPathButton.onClick.RemoveListener(OnResetButtonClick);
    }

    private void OnStateChanged(GameManager.GameState newState)
    {
        _nextLevelButton.interactable = newState != GameManager.GameState.BuildingPath;
        if (!_nextLevelButton.interactable || _resetPathButton == null)
            return;

        _resetPathButton.onClick.RemoveAllListeners();
        Destroy(_resetPathButton.gameObject);
        _resetPathButton = null;
    }

    private void OnLevelChanged(GameManager.OnLevelChangedEventArgs args)
    {
        _nextLevelButton.interactable = !args.IsLastLevel;
    }

    private void OnResetButtonClick()
    {
        _sandPathBuildService.ResetPath();
        _gameManager.CurrentState = GameManager.GameState.BuildingPath;
    }

    public void ExitToMainMenu() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
}
