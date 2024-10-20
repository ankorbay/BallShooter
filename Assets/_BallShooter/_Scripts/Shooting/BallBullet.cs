using Mirror;
using UnityEngine;

public class BallBullet : NetworkBehaviour
{
    public float destroyAfter = 2f;
    public Rigidbody rigidBody;

    private GatesController _shooter;

    public void Initialize(GatesController shooter)
    {
        _shooter = shooter;
    }
    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }
    
    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
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
            
            DestroySelf();
        }
    }
}