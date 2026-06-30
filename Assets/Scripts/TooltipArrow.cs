using UnityEngine;

public class TooltipArrow : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

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
        if (newState != GameManager.GameState.BuildingPath)
            gameObject.SetActive(false);
    }
}
