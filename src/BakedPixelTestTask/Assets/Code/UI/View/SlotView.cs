using Code.StaticData.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.View
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        public void Render(ItemConfig config, int count)
        {
            _icon.sprite = config.Icon;
            _icon.enabled = true;
            _count.text = config.MaxStack > 1 ? count.ToString() : string.Empty;
        }

        public void Clear()
        {
            _icon.sprite = null;
            _icon.enabled = false;
            _count.text = string.Empty;
        }
    }
}