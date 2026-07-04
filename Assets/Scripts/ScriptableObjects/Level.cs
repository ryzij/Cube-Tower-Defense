using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Level_No", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [SerializeField] private bool _shuffle = false;
    [Tooltip("Если элемент не задан, будет выбран случайный из списка префабов спавнера")]
    [SerializeField] private Enemy[] _enemiesPrefabs;
    [SerializeField] private float _levelTimeSec = 10f;
    [SerializeField] private float _enemiesHealthMultiplier = 1;
    [SerializeField] private float _enemiesSpeedMultiplier = 1;
    [SerializeField] private float _enemiesDamagingMultiplier = 1;
    
    public IReadOnlyCollection<Enemy> EnemiesPrefabs =>
        _shuffle ? _enemiesPrefabs.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)).ToList() : _enemiesPrefabs;
    public float LevelTimeSec => _levelTimeSec;
    public float EnemiesHealthMultiplier => _enemiesHealthMultiplier;
    public float EnemiesSpeedMultiplier => _enemiesSpeedMultiplier;
    public float EnemiesDamagingMultiplier => _enemiesDamagingMultiplier;
}
