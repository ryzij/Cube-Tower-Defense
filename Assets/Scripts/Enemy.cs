using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SandPathBuildService _pathService;
    [SerializeField] private float _health = 5f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private int _killReward = 10;
    [SerializeField] private UnityEvent<float> _onHealthChanged;
    [SerializeField] private UnityEvent<OnEnemyDeathEventArgs> _onDeath;
    [SerializeField] private UnityEvent _onDestroyed;
    
    private Queue<Vector3> _path;
    private Vector3 _target;

    public event UnityAction<float> OnHealthChanged
    {
        add => _onHealthChanged.AddListener(value);
        remove => _onHealthChanged.RemoveListener(value);
    }
    
    public event UnityAction<OnEnemyDeathEventArgs> OnDeath
    {
        add => _onDeath.AddListener(value);
        remove => _onDeath.RemoveListener(value);
    }

    public event UnityAction OnDestroyed
    {
        add => _onDestroyed.AddListener(value);
        remove => _onDestroyed.RemoveListener(value);
    }

    public SandPathBuildService PathBuildService
    {
        get => _pathService;
        set => _pathService = value;
    }
    
    public int PathLenght => _path?.Count ?? -1;

    public float Health
    {
        get => _health;
        private set
        {
            _health = value;
            _onHealthChanged?.Invoke(value);
        }
    }

    public float Damage { get => _damage; set => _damage = value; }

    private void Start()
    {
        _path = new Queue<Vector3>(_pathService.GetPath());
        _target = _path.Dequeue();
    }

    private void Update()
    {
        if (transform.position == _target)
        {
            if (_path.Count == 0) return;
            _target = _path.Dequeue();
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        transform.LookAt(_target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out TurretBullet bullet))
            return;
        
        TakeDamage(bullet.Damage);
        Destroy(other.gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            _onDeath?.Invoke(new OnEnemyDeathEventArgs { KillReward = _killReward });
            Destroy(gameObject);
        }
    }

    private void OnDestroy() => _onDestroyed?.Invoke();

    public class OnEnemyDeathEventArgs
    {
        public int KillReward { get; set; }
    }
}