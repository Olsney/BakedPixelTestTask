using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.View
{
    public class HudView : MonoBehaviour
    {
        [field: SerializeField] public InventoryView InventoryView { get; private set; }
        [field: SerializeField] public Button FireButton { get; private set; }
        [field: SerializeField] public Button AddAmmoButton { get; private set; }
        [field: SerializeField] public Button AddItemButton { get; private set; }
        [field: SerializeField] public Button DeleteItemButton { get; private set; }
        [field: SerializeField] public Button AddCoinsButton { get; private set; }
        [field: SerializeField] public TMP_Text CoinsText { get; private set; }
        [field: SerializeField] public TMP_Text WeightText { get; private set; }
    }
}