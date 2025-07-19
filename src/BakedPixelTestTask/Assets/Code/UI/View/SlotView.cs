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
        [SerializeField] private Image _lockIcon;

        private Button _button;

        public Button Button => _button;

        private void Awake() =>
            _button = GetComponent<Button>();

        public void Render(ItemConfig config, int count)
        {
            _icon.sprite = config.Icon;
            _icon.enabled = true;
            _count.text = config.MaxStack > 1 ? count.ToString() : string.Empty;
            _lockIcon.gameObject.SetActive(false);
        }

        public void Clear()
        {
            _icon.sprite = null;
            _icon.enabled = false;
            _count.text = string.Empty;
            _lockIcon.gameObject.SetActive(false);
        }

        public void SetLocked(bool value)
        {
            _lockIcon.gameObject.SetActive(value);

            if (value)
            {
                _icon.enabled = false;
                _count.text = string.Empty;
            }
        }
    }
}