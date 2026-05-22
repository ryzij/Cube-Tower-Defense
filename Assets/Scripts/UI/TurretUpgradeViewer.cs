using UnityEngine;
using UnityEngine.UI;

public class TurretUpgradeViewer : MonoBehaviour
{
    [SerializeField] private Transform _viewerHolder;
    [SerializeField] private TurretUpdater _turretUpdater;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMPro.TextMeshProUGUI _upgradeCostText;

    private int _upgradeCost;
    private bool _isMaxLevel;

    private void OnEnable()
    {
        _wallet.OnMoneyChanged += OnMoneyChanged;
        _turretUpdater.OnTurretClicked += OnTurretClicked;
        _turretUpdater.OnTurretUpgraded += UpdateBtn;
    }

    private void OnDisable()
    {
        _wallet.OnMoneyChanged -= OnMoneyChanged;
        _turretUpdater.OnTurretClicked -= OnTurretClicked;
        _turretUpdater.OnTurretUpgraded -= UpdateBtn;
    }

    private void OnMoneyChanged(int newMoney)
    {
        _upgradeButton.interactable = newMoney >= _upgradeCost && !_isMaxLevel;
    }

    private void OnTurretClicked(Turret turret)
    {
        if (turret == null)
        {
            _viewerHolder.gameObject.SetActive(false);
            return;
        }
        
        _viewerHolder.gameObject.SetActive(true);
        UpdateBtn(turret);
    }

    private void UpdateBtn(Turret turret)
    {
        _isMaxLevel = turret.IsMaxLevel;
        if (_isMaxLevel)
        {
            _upgradeCostText.text = "MAX";
            _upgradeButton.interactable = false;
            return;
        }
        
        _upgradeCost = turret.UpgradeCost;
        _upgradeCostText.text = _upgradeCost.ToString();
        _upgradeButton.interactable = _wallet.Money >= _upgradeCost;
    }
}
