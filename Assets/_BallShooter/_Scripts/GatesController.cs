using Mirror;
using Mirror.Examples.Tanks;
using StarterAssets;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class GatesController : NetworkBehaviour
{
    [Header("MainCamera")]
    public Transform mainCameraTransform;
    [Header("Components")]
    public Transform turret;

    [Header("Movement")]
    public float movementSpeed = 3;

    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;
    
    [Header("Firing")]
    public GameObject projectilePrefab;
    public Transform  projectileMount;
    public float chargeTime = 2f;
    public float baseBulletSpeed = 1000f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;
    

    [Header("Stats")]
    [SyncVar] public float _bulletChargeTime = 0f;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;
    private float _cinemachineTargetPitch;
    private float _rotationVelocity;
    

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    public override void OnStartAuthority()
    {
        mainCameraTransform.gameObject.SetActive(true);
        _input.enabled = true;
#if ENABLE_INPUT_SYSTEM
        _playerInput.enabled = true;
#endif
    }

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

    }
    

    void Update()
    {
        if(!Application.isFocused || !isLocalPlayer) return; 
        
        transform.Translate(_input.move.x * movementSpeed * Time.deltaTime, 0f, 0f);
        
        if (Mouse.current.leftButton.isPressed)
        {
            _bulletChargeTime += Time.deltaTime;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            CmdFire();
        }
    }
    
    private void LateUpdate()
    {
        if (isLocalPlayer)
            GunRotation();
    }
    
    [Command]
    void CmdFire()
    {
        float chargeValue = _bulletChargeTime / chargeTime;
        Debug.Log("Charge value: " + chargeValue);
        float bulletSpeed = baseBulletSpeed * chargeValue;
        GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
        projectile.GetComponent<Projectile>().force = bulletSpeed;
        NetworkServer.Spawn(projectile);
        _bulletChargeTime = 0f;
    }
    
    private void GunRotation()
    {
        // if there is an input
        if (_input.look.sqrMagnitude >= 0.01f)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            turret.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
