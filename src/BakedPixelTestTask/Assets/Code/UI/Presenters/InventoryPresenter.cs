using Code.Gameplay.Inventory;
using Code.UI.View;

namespace Code.UI.Presenters
{
    public class InventoryPresenter
    {
        private readonly InventoryModel _model;
        private readonly InventoryView _view;

        public InventoryPresenter(InventoryModel model, InventoryView view)
        {
            _model = model;
            _view = view;

            _model.InventoryChanged += UpdateView;

            UpdateView();
        }

        private void UpdateView()
        {
            for (int i = 0; i < _model.Slots.Count; i++)
            {
                var slotModel = _model.Slots[i];
                var slotView = _view.GetSlotView(i);

                if (slotModel.IsEmpty)
                    slotView.Clear();
                else
                    slotView.Render(slotModel.Item.Config, slotModel.Item.Count);
            }
        }

        public void Dispose()
        {
            _model.InventoryChanged -= UpdateView;
        }
    }
}