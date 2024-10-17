using Mirror;
using UnityEngine;

public class BallBullet : NetworkBehaviour
{
    public float destroyAfter = 2f;
    public Rigidbody rigidBody;
    
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
            --playerGates.points;
            
            DestroySelf();
        }
    }
}
