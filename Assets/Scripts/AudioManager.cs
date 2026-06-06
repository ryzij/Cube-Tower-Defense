using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioSource _levelBackgroundMusic;
    [SerializeField] private AudioSource _buildBackgroundMusic;
    [SerializeField] private AudioSource _winMusic;
    [SerializeField] private AudioSource _loseMusic;
    [SerializeField] private AudioSource[] _grassBlockDestroySound;
    [SerializeField] private AudioSource[] _moneyChangeSound;

    private int _grassSoundIndex = 0;
    private int _moneyChangeSoundIndex = 0;

    private int _money;
    private GameManager.GameState _state;

    private void OnEnable()
    {
        _money = _wallet.Money;
        _state = _gameManager.CurrentState;

        _wallet.OnMoneyChanged += OnMoneyChanged;
        _gameManager.OnStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        _wallet.OnMoneyChanged -= OnMoneyChanged;
        _gameManager.OnStateChanged -= OnStateChanged;
    }

    public void PlayBlockDestroySound(BlockScript block)
    {
        switch (block.Type)
        {
            case BlockScript.BlockType.Grass:
            case BlockScript.BlockType.PathFinish:
                Play(_grassBlockDestroySound, ref _grassSoundIndex);
                break;
        }
    }

    private void OnMoneyChanged(int money)
    {
        var oldMoney = _money;
        _money = money;

        if (_moneyChangeSound.Length == 0 || oldMoney <= money)
            return;

        Play(_moneyChangeSound, ref _moneyChangeSoundIndex);
    }

    private void OnStateChanged(GameManager.GameState newState)
    {
        if (newState == _state ||
            _state == GameManager.GameState.BuildingPath && newState == GameManager.GameState.BuildingTurrets)
            return;

        _state = newState;
        switch (newState)
        {
            case GameManager.GameState.Level:
                Stop(_buildBackgroundMusic);
                Play(_levelBackgroundMusic);
                break;
            case GameManager.GameState.BuildingPath:
            case GameManager.GameState.BuildingTurrets:
                Stop(_levelBackgroundMusic);
                Play(_buildBackgroundMusic);
                break;
            case GameManager.GameState.Win:
                Stop(_levelBackgroundMusic);
                Stop(_buildBackgroundMusic);
                Play(_winMusic);
                break;
            case GameManager.GameState.Lose:
                Stop(_levelBackgroundMusic);
                Stop(_buildBackgroundMusic);
                Play(_loseMusic);
                break;
        }
    }

    private static void Play(AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.Play();
    }

    private static void Play(AudioSource[] audioSources, ref int index)
    {
        if (audioSources == null || index >= audioSources.Length)
            return;

        Play(audioSources[index]);
        index = (index + 1) % audioSources.Length;
    }

    private static void Stop(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}
