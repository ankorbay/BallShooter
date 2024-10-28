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
        public Color defaultColor;
    }

    [Serializable]
    public class MovementSettings
    {
        public float speed;
        public float rotationSpeed;
        public float moveXLimit;
    }

    [Serializable]
    public class ShootingSettings
    {
        public GameObject ballPrefab;
        [Tooltip("The default force that will be applied to the ball when it is shot. Recommended value to start: 1000.")]
        public float defaultForce;
        [Tooltip("The minimum force multiplier that will be applied to the ball when it is shot. This is used to prevent the ball from being shot with a force of 0. E.g. if defaultForce is 1000 and minForceMultiplier is 0.5, the ball will be added a force of 500 when shot happens.")]
        public float minForceMultiplier;
        [Tooltip("Time in seconds that the player has to hold the shoot button to reach the maximum force. Also used as a maxForceMultiplier.")]
        public float forceChargingDuration;
        public float ballLifeTime;
    }
}