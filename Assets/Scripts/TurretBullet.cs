using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _distance;

    private Vector3 _spawnPos;

    public Turret From { get; private set; }
    public float Damage => _damage;
    public float Distance => _distance;
    
    private void Start()
    {
        _spawnPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, _spawnPos) > _distance)
            Destroy(gameObject);
    }

    public TurretBullet Spawn(Turret turret, Vector3 spawnPoint, Vector3 direction)
    {
        var rot = turret.transform.rotation;
        rot.SetLookRotation(direction);
        var bullet = Instantiate(this, spawnPoint, rot, turret.transform);
        bullet.From = turret;
        bullet._damage += turret.Damage;
        bullet._distance += turret.Distance - Vector3.Distance(spawnPoint, bullet.transform.position);

        return bullet;
    }
}
