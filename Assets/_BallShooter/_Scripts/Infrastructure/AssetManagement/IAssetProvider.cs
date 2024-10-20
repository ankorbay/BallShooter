using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Services
{
    public interface IAssetProvider: IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path, Transform parent);
        
        Task<GameObject> InstantiateAsync(string path);
        Task<GameObject> InstantiateAsync(string path, Vector3 at);
        Task<GameObject> InstantiateAsync(string path, Transform parent);
        
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void CleanUp();
        void Ititialize();
    }
}