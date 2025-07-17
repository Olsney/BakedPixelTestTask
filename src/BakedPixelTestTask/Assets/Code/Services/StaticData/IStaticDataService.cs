using System.Collections.Generic;
using Code.StaticData.Item;

namespace Code.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadItems();
        ItemConfig ForItem(string id);
        IReadOnlyList<ItemConfig> AllItems { get; }
    }
}