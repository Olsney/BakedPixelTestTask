using UnityEngine;

namespace Code.Infrastructure.Factory.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Load(string path);
    }
}