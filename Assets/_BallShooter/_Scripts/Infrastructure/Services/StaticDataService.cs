using _BallShooter._Scripts.Infrastructure.Data;
using Services;

namespace _BallShooter._Scripts.Infrastructure.Services
{
    public class StaticDataService: IStaticDataService
    {
        public GameSettings GameSettings { get; }
        
        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void Load()
        {
            _assetProvider.Load<GameSettings>(AssetAddress.GameSettings);
        }

    }

    public interface IStaticDataService: IService
    {
        void Load();
        GameSettings GameSettings { get; }
    }
}