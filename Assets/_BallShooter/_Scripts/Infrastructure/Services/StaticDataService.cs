using _BallShooter._Scripts.Infrastructure.Data;
using Services;
using UnityEngine;

namespace _BallShooter._Scripts.Infrastructure.Services
{
    public class StaticDataService: IStaticDataService
    {
        public GameSettings GameSettings { get; private set; }
        public StaticDataService Load()
        {
            GameSettings = Resources.Load<GameSettings>(AssetAddress.GameSettings);
            return this;
        }

    }

    public interface IStaticDataService: IService
    {
        StaticDataService Load();
        GameSettings GameSettings { get; }
    }
}