using System;
using _BallShooter._Scripts.Infrastructure.Data;
using _BallShooter._Scripts.Infrastructure.Services;
using Infrastructure.Services;
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
    public float rotationSpeed = 1.0f;
    
    [Header("Firing")]
    public GameObject projectilePrefab;
    public Transform  projectileMount;
    
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
    private float _moveDistance;
    private float _rotationVelocityX;
    private float _rotationVelocityY;
    private bool _isCharging;
    private float _bulletChargeValue;
    private float _currentForce;
    private Material _cachedMaterial;
    private ShootingSettings _shootingSettings;
    private MovementSettings _movementSettings;

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
        GameSettings gameSettings = AllServices.Container.Single<IStaticDataService>().GameSettings;
        _shootingSettings = gameSettings.shootingSettings;
        _movementSettings = gameSettings.movementSettings;

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

        MovePlayer();
        ProcessShooting();
    }

    private void ProcessShooting()
    {
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

    private void MovePlayer()
    {
        _moveDistance += _input.move.x * _movementSettings.speed * Time.deltaTime;
        if (Mathf.Abs(_moveDistance) < _movementSettings.moveXLimit)
        {
            transform.Translate(_input.move.x * _movementSettings.speed * Time.deltaTime, 0f, 0f);
        }
        else if (_moveDistance > 0)
        {
            _moveDistance = _movementSettings.moveXLimit;
        }
        else
        {
            _moveDistance = -_movementSettings.moveXLimit;
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
            _bulletChargeValue = Mathf.Clamp(_bulletChargeValue, _shootingSettings.minForceMultiplier, _shootingSettings.forceChargingDuration); // Clamp to max force
        }
    }
    private void Fire()
    {
        Debug.Log("Fire is called, isCharging: " + _isCharging + " bulletChargeValue: " + _bulletChargeValue);
        if (_isCharging)
        {
            _isCharging = false;
            _currentForce = _shootingSettings.defaultForce * _bulletChargeValue;
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
            
            _rotationVelocityX = _input.look.x * _movementSettings.rotationSpeed * deltaTimeMultiplier;
            _rotationVelocityY = _input.look.y * _movementSettings.rotationSpeed * deltaTimeMultiplier;
            
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
