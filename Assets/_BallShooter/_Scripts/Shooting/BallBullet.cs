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
    private PrefabPool _ballPool;

    public void Initialize(GatesController shooter, PrefabPool prefabPool)
    {
        _shooter = shooter;
        _ballPool = prefabPool;
    }

    public void OnEnable()
    {
        float lifeTime = AllServices.Container.Single<IStaticDataService>().GameSettings.shootingSettings.ballLifeTime;
        Invoke(nameof(DestroySelf), lifeTime > 0.01f ? lifeTime : destroyAfter);
    }
    
    [Server]
    void DestroySelf()
    {
        NetworkServer.UnSpawn(gameObject);
        if(_ballPool != null) _ballPool.Return(gameObject);
    }
    
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
            
            DestroySelf();
        }
    }
}
