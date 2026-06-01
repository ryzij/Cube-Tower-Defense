using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private AudioSource[] _grassBlockDestroySound;
    [SerializeField] private AudioSource[] _moneyChangeSound;

    private int _grassSoundIndex = 0;
    private int _moneyChangeSoundIndex = 0;

    private int _money;

    private void OnEnable()
    {
        _money = _wallet.Money;

        _wallet.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _wallet.OnMoneyChanged -= OnMoneyChanged;
    }

    public void PlayBlockDestroySound(BlockScript block)
    {
        switch (block.Type)
        {
            case BlockScript.BlockType.Grass:
            case BlockScript.BlockType.PathFinish:
                if (_grassBlockDestroySound == null)
                    return;

                _grassBlockDestroySound[_grassSoundIndex].Play();
                _grassSoundIndex = (_grassSoundIndex + 1) % _grassBlockDestroySound.Length;
                break;
        }
    }

    private void OnMoneyChanged(int money)
    {
        var oldMoney = _money;
        _money = money;

        if (_moneyChangeSound.Length == 0 || oldMoney <= money)
            return;

        _moneyChangeSound[_moneyChangeSoundIndex].Play();
        _moneyChangeSoundIndex = (_moneyChangeSoundIndex + 1) % _moneyChangeSound.Length;
    }
}
