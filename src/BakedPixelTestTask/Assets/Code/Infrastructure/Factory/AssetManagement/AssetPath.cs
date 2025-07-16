using UnityEngine;

namespace Code.Infrastructure.Factory.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Load(string path) => 
            Resources.Load<GameObject>(path);
    }
}