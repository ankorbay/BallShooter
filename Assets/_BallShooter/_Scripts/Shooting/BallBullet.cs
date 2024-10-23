using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.ObjectPool;
using Infrastructure.Services;
using Mirror;
using UnityEngine;

public class BallBullet : NetworkBehaviour
{
    public float destroyAfter = 2f;
    public Rigidbody rigidBody;

    private GatesController _shooter;
    private PrefabPoolManager _pool;

    public void Initialize(GatesController shooter, PrefabPoolManager pool)
    {
        _shooter = shooter;
        _pool = pool;
    }
    public void OnEnable()
    {
        Invoke(nameof(ReturnToPool), AllServices.Container.Single<IStaticDataService>().GameSettings.shootingSettings.ballLifeTime);
    }
    
    [Server]
    void ReturnToPool()
    {
        _pool.PutBackInPool(gameObject);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name);
        if (other.transform.parent.TryGetComponent(out GatesController playerGates))
        {
            if (playerGates != _shooter)
            {
                --playerGates.points;
                ++_shooter.points;

                Debug.Log($"{_shooter.name} gained a point! Current points: {_shooter.points}");
            }
            else
            {
                Debug.Log("Shooter hit their own gates. No points awarded.");
            }
            
            ReturnToPool();
        }
    }
}
