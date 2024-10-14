using Mirror;
using UnityEngine;

public class GatesController : NetworkBehaviour
{
    [Header("Components")]
    public Transform turret;

    [Header("Movement")]
    public float movementSpeed = 3;

    [Header("Firing")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public GameObject projectilePrefab;
    public Transform  projectileMount;

    [Header("Stats")]
    [SyncVar] public int health = 5;

    void Update()
    {
        // take input from focused window only
        if(!Application.isFocused) return; 

        // movement for local player
        if (isLocalPlayer)
        {
            // move
            float horizontal = Input.GetAxis("Horizontal");
            transform.Translate(horizontal * movementSpeed * Time.deltaTime, 0f, 0f);
            // clamp position x from -8 to 8
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8f, 8f), 0, 0);

            // shoot
            if (Input.GetKeyDown(shootKey))
            {
                CmdFire();
            }

            RotateTurret();
        }
    }

    // this is called on the server
    [Command]
    void CmdFire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
        NetworkServer.Spawn(projectile);
    }

    //[ServerCallback]
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Projectile>() != null)
    //    {
    //        --health;
    //        if (health == 0)
    //            NetworkServer.Destroy(gameObject);
    //    }
    //}

    void RotateTurret()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Vector3 lookRotation = new Vector3(hit.point.x, turret.transform.position.y, hit.point.z);
            turret.transform.LookAt(lookRotation);
        }
    }
}
