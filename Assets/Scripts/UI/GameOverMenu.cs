using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private WinLoseController _winLoseController;
    [SerializeField] private Transform _menuPanel;

    private void OnEnable()
    {
        _winLoseController.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _winLoseController.OnGameOver -= OnGameOver;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Island");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnGameOver(WinLoseController.OnGameOverEventArgs args)
    {
        Time.timeScale = 0f;
        _menuPanel.gameObject.SetActive(true);
    }
}
