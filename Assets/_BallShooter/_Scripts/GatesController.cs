using System;
using Mirror;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class GatesController : NetworkBehaviour
{
    [Header("MainCamera")]
    public Transform mainCameraTransform;
    [Header("Components")]
    public Transform turret;

    public TextMeshProUGUI pointsText;

    [Header("Movement")]
    public float movementSpeed = 3;

    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;
    
    [Header("Firing")]
    public GameObject projectilePrefab;
    public Transform  projectileMount;
    public float fullChargeDuration = 3f;
    public float baseBulletSpeed = 1000f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;
    
    [Header("Stats")] 
    [SyncVar(hook = nameof(OnPointsChanged))]
    public int points;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;
    private float _rotationVelocityX;
    private float _rotationVelocityY;
    private bool _isCharging;
    private float _bulletChargeValue;
    private float _currentForce;
    private Material _cachedMaterial;

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
        enabled = true;
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
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCharging();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            ContinueCharging();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Fire();
        }
    }

    private void StartCharging()
    {
        Debug.Log("Start charging");
        _isCharging = true;
        _bulletChargeValue = 0f;
    }
    
    private void ContinueCharging()
    {
        if (_isCharging)
        {
            _bulletChargeValue += Time.deltaTime;
            _bulletChargeValue = Mathf.Clamp(_bulletChargeValue, 0.5f, fullChargeDuration); // Clamp to max force
        }
    }
    private void Fire()
    {
        Debug.Log("Fire is called, isCharging: " + _isCharging + " bulletChargeValue: " + _bulletChargeValue);
        if (_isCharging)
        {
            _isCharging = false;
            _currentForce = baseBulletSpeed * _bulletChargeValue;
            CmdFire(_currentForce);
            _bulletChargeValue = 0f;
            _currentForce = 0f;
        }
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
            GunRotation();
    }
    
    [Command]
    void CmdFire(float force)
    {
        BallBullet bullet = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation).GetComponent<BallBullet>();
        bullet.Initialize(this);
        bullet.rigidBody.AddForce(projectileMount.forward * force);
        NetworkServer.Spawn(bullet.gameObject);
    }

    private void GunRotation()
    {
        if (_input.look.sqrMagnitude >= 0.01f)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            
            _rotationVelocityX = _input.look.x * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocityY = _input.look.y * RotationSpeed * deltaTimeMultiplier;
            
            turret.Rotate(Vector3.up * _rotationVelocityX + Vector3.right * _rotationVelocityY);
            turret.localRotation = Quaternion.Euler(turret.localRotation.eulerAngles.x, turret.localRotation.eulerAngles.y, 0);
        }
    }
    
    private void OnPointsChanged(int oldPoints, int newPoints)
    {
        UpdatePointsText(newPoints);
    }
    
    private void UpdatePointsText(int newPoints)
    {
        if (pointsText != null)
        {
            pointsText.text = newPoints.ToString();
        }
    }
}
