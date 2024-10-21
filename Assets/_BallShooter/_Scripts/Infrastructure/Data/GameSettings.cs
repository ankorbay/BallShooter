using System;
using UnityEngine;

namespace _BallShooter._Scripts.Infrastructure.Data
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
    public class GameSettings: ScriptableObject
    {
        public PlayersSettings playerSettings;
        public ShootingSettings shootingSettings;
        public MovementSettings movementSettings;
    }

    [Serializable]
    public class PlayersSettings
    {
        public GameObject playerPrefab;
    }

    [Serializable]
    public class MovementSettings
    {
        public float speed;
        public float rotationSpeed;
    }

    [Serializable]
    public class ShootingSettings
    {
        public float defaultForce;
        public float minForceMultiplier;
        public float maxForceMultiplier;
        public float forceChargingDuration;
    }
}