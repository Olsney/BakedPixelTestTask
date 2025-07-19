using Code.Gameplay.Inventory;
using Code.Services.PersistentProgress;
using Code.UI.View;

namespace Code.UI.Presenters
{
    public class InventoryPresenter : Presenter<InventoryView>
    {
        private readonly InventoryModel _model;
        private readonly InventoryView _view;
        private readonly IPersistentProgressService _progress;

        public InventoryPresenter(InventoryModel model, InventoryView view, IPersistentProgressService progress) :base(view)
        {
            _model = model;
            _view = view;
            _progress = progress;
        }

        public void Initialize()
        {
            _model.InventoryChanged += UpdateView;

            UpdateView();
            BindSlots();
        }

        private void UpdateView()
        {
            for (int i = 0; i < _model.Slots.Count; i++)
            {
                SlotModel slotModel = _model.Slots[i];
                SlotView slotView = _view.GetSlotView(i);
                slotView.SetLocked(slotModel.IsLocked);

                if (slotModel.IsLocked)
                    continue;

                if (slotModel.IsEmpty)
                    slotView.Clear();
                else
                    slotView.Render(slotModel.Item.Config, slotModel.Item.Count);
            }
        }
        
        private void BindSlots()
        {
            for (int i = 0; i < _view.SlotCount; i++)
            {
                int index = i;
                _view.GetSlotView(i).Button.onClick.AddListener(() => OnSlotClicked(index));
            }
        }

        private void OnSlotClicked(int index)
        {
            SlotModel slot = _model.Slots[index];
            if (!slot.IsLocked)
                return;

            if (_progress.Progress.Coins < _model.UnlockSlotPrice)
                return;

            _progress.Progress.Pay(_model.UnlockSlotPrice);
            slot.Unlock();
        }


        public override void Dispose()
        {
            _model.InventoryChanged -= UpdateView;
        }
    }
}