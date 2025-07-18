using UnityEngine;

namespace Code.UI.View
{
    public class HudView : MonoBehaviour
    {
        [field: SerializeField] public InventoryView InventoryView { get; private set; }
    }
}