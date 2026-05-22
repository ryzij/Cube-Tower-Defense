using UnityEngine;

[CreateAssetMenu(fileName = "Lvl_", menuName = "Scriptable Objects/Turret Upgrade")]
public class TurretUpgrade : ScriptableObject
{
    [Tooltip("Процент стоисти от базовой цены турели для улучшения")]
    [SerializeField] private float _upgradeCostMultiplier = 0.2f;
    [Tooltip("null, чтобы не менять префаб пули")]
    [SerializeField] private TurretBullet _bulletPrefab;
    [Tooltip("-1, чтобы не менять урон")]
    [SerializeField] private float _upgradeDamageMultiplier = -1f;
    [Tooltip("-1, чтобы не менять дальность")]
    [SerializeField] private float _upgradeDistanceMultiplier = -1f;
    [Tooltip("-1, чтобы не менять время перезарядки")]
    [SerializeField] private float _upgradeReloadTimeMultiplier = -1f;

    public float UpgradeConstMultiplier => _upgradeCostMultiplier;
    public TurretBullet BulletPrefab => _bulletPrefab;
    public float UpgradeDamageMultiplier => _upgradeDamageMultiplier;
    public float UpgradeDistanceMultiplier => _upgradeDistanceMultiplier;
    public float UpgradeReloadTimeMultiplier => _upgradeReloadTimeMultiplier;
}
