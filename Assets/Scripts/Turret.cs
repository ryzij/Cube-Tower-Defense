using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TurretBullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [Header("Stats")]
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private float _damage;
    [SerializeField] private float _distance = 3f;
    [Header("Economy")]
    [SerializeField] private int _price;
    [SerializeField] private float _sellingMultiplier = 0.5f;
    [Header("Upgrade")]
    [SerializeField] TurretUpgrade[] _upgrades;

    private float _reloadTimer;
    private int _lvl;

    private float _baseReloadTime;
    private float _baseDamage;
    private float _baseDistance;

    private float _currentPrice;

    private readonly Dictionary<Vector3, Enemy> _enemiesInRange = new();

    public int Price => _price;
    public float Damage => _damage;
    public float Distance => _distance;
    public int SellPrice => Mathf.RoundToInt(_currentPrice * _sellingMultiplier);
    public int UpgradeCost
    {
        get
        {
            if (IsMaxLevel)
                return 0;

            return Mathf.RoundToInt(_price * _upgrades[_lvl].UpgradeConstMultiplier);
        }
    }
    public bool IsMaxLevel => _lvl >= _upgrades.Length;
    public int Level => _lvl + 1;
    public int MaxLevel => _upgrades.Length;

    private void Start()
    {
        if (_bulletSpawnPoint == null)
            _bulletSpawnPoint = transform;

        _baseReloadTime = _reloadTime;
        _baseDamage = _damage;
        _baseDistance = _distance;
        _currentPrice = _price;
    }

    public void Upgrade()
    {
        if (IsMaxLevel)
            return;

        var upgrade = _upgrades[_lvl++];
        if (upgrade.BulletPrefab != null)
            _bulletPrefab = upgrade.BulletPrefab;

        if (upgrade.UpgradeDistanceMultiplier != -1)
        {
            if (_damage == 0)
                _baseDamage = _damage = upgrade.UpgradeDamageMultiplier;
            else
                _damage = _baseDamage * upgrade.UpgradeDamageMultiplier;
        }

        if (upgrade.UpgradeDistanceMultiplier != -1)
        {
            if (_distance == 0)
                _baseDistance = _distance = upgrade.UpgradeDistanceMultiplier;
            else
                _distance = _baseDistance * upgrade.UpgradeDistanceMultiplier;
        }

        if (upgrade.UpgradeReloadTimeMultiplier != -1)
            _reloadTime = _baseReloadTime * upgrade.UpgradeReloadTimeMultiplier;

        _currentPrice += _price * 0.25f;
    }

    private void Update()
    {
        // SpawnBullet(Vector3.forward);
        // SpawnBullet(Vector3.back);
        // SpawnBullet(Vector3.left);
        // SpawnBullet(Vector3.right);

        var direction = GetMinPathEnemyDirection();
        if (direction != Vector3.zero)
            SpawnBullet(direction);

        _reloadTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TurretBullet bullet) && bullet.From != this)
            Destroy(bullet.gameObject);
    }

    private void SpawnBullet(Vector3 direction)
    {
        if (_reloadTimer < _reloadTime/* || !DetectEnemy(direction)*/)
            return;

        _bulletPrefab.Spawn(this, _bulletSpawnPoint.position, direction);
        _reloadTimer = 0f;
    }

    // private bool DetectEnemy(Vector3 direction) =>
    //     Physics.Raycast(transform.position, direction, out var hit, _bulletPrefab.Distance + _distance) &&
    //     hit.transform.TryGetComponent(out Enemy _);

    private Enemy DetectEnemy(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out var hit, _bulletPrefab.Distance + _distance) &&
            hit.transform.TryGetComponent(out Enemy enemy))
            return enemy;

        return null;
    }

    private Vector3 GetMinPathEnemyDirection()
    {
        _enemiesInRange[Vector3.forward] = DetectEnemy(Vector3.forward);
        _enemiesInRange[Vector3.back] = DetectEnemy(Vector3.back);
        _enemiesInRange[Vector3.left] = DetectEnemy(Vector3.left);
        _enemiesInRange[Vector3.right] = DetectEnemy(Vector3.right);

        Vector3 direction = Vector3.zero;
        Enemy minPathEnemy = null;
        foreach (var (dir, enemy) in _enemiesInRange)
        {
            if (enemy == null)
                continue;
            
            if (minPathEnemy == null || enemy.PathLenght < minPathEnemy.PathLenght)
            {
                direction = dir;
                minPathEnemy = enemy;
            }
        }

        if (minPathEnemy == null)
            return Vector3.zero;

        return direction;
    }
}
