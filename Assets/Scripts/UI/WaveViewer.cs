using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WaveViewer : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _gameManager.OnLevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnLevelChanged -= OnLevelChanged;
    }

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnLevelChanged(GameManager.OnLevelChangedEventArgs e)
    {
        _text.text = $"Wave {e.CurrentLevel} / {e.TotalLevels}";
    }
}
