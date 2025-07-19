using System;
using System.Collections.Generic;
using Code.Data;
using Code.Gameplay.Inventory;
using Code.Services.PersistentProgress;
using Code.Services.StaticData;
using Code.StaticData.GameBalance;
using Code.StaticData.Item;
using Code.UI.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.UI.Presenters
{
    public class HudPresenter
    {
        private readonly InventoryModel _inventory;
        private readonly IPersistentProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly HudView _view;

        private readonly List<ItemConfig> _ammoConfigs = new List<ItemConfig>();
        private readonly List<ItemConfig> _weaponConfigs = new List<ItemConfig>();
        private readonly List<ItemConfig> _armorConfigs = new List<ItemConfig>();
        private readonly GameBalanceConfig _gameBalanceConfig;
        private readonly Action<int> _coinsChangedHandler;
        private readonly Action _inventoryChangedHandler;

        public HudPresenter(InventoryModel inventory,
            IPersistentProgressService progress,
            IStaticDataService staticData,
            HudView view)
        {
            _inventory = inventory;
            _progress = progress;
            _staticData = staticData;
            _view = view;
            _gameBalanceConfig = _staticData.GetGameBalanceConfig();

            foreach (ItemConfig config in _staticData.AllItems)
            {
                switch (config.Category)
                {
                    case ItemCategory.Ammo:
                        _ammoConfigs.Add(config);
                        break;
                    case ItemCategory.Weapon:
                        _weaponConfigs.Add(config);
                        break;
                    case ItemCategory.Armor:
                        _armorConfigs.Add(config);
                        break;
                }
            }

        }

        public void Initialize()
        {
            _inventory.InventoryChanged += UpdateWeight;
            _progress.Progress.CoinsChanged += UpdateCoins;

            BindButtons();

            UpdateWeight();
            UpdateCoins(_progress.Progress.Coins);
        }

        private void BindButtons()
        {
            _view.FireButton.onClick.AddListener(Fire);
            _view.AddAmmoButton.onClick.AddListener(AddAmmo);
            _view.AddItemButton.onClick.AddListener(AddItem);
            _view.DeleteItemButton.onClick.AddListener(DeleteItem);
            _view.AddCoinsButton.onClick.AddListener(AddCoins);
        }

        private void Fire()
        {
            List<int> weaponSlots = new List<int>();
            
            foreach (SlotModel slot in _inventory.Slots)
            {
                if (!slot.IsLocked && !slot.IsEmpty && slot.Item.Config.Category == ItemCategory.Weapon)
                    weaponSlots.Add(_inventory.Slots.IndexOf(slot));
            }

            if (weaponSlots.Count == 0)
            {
                Debug.LogError("No weapons to fire");
                
                return;
            }

            int weaponIndex = weaponSlots[Random.Range(0, weaponSlots.Count)];
            SlotModel weaponSlot = _inventory.Slots[weaponIndex];
            ItemConfig weaponConfig = weaponSlot.Item.Config;

            int ammoSlotIndex = -1;
            ItemConfig ammoConfig = null;
            
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                SlotModel slot = _inventory.Slots[i];
                if (!slot.IsLocked && !slot.IsEmpty && slot.Item.Config.Type == weaponConfig.AmmoType)
                {
                    ammoSlotIndex = i;
                    ammoConfig = slot.Item.Config;
                    break;
                }
            }

            if (ammoSlotIndex == -1)
            {
                Debug.LogError("No ammo for selected weapon");
                
                return;
            }

            SlotModel ammoSlot = _inventory.Slots[ammoSlotIndex];
            ammoSlot.Item.Remove(1);
            
            if (ammoSlot.Item.Count == 0)
                ammoSlot.Clear();

            Debug.Log($"Fired {weaponConfig.DisplayName} with {ammoConfig.DisplayName}. Damage {weaponConfig.Damage}");
        }

        private void AddAmmo()
        {
            foreach (ItemConfig ammo in _ammoConfigs)
            {
                if (_inventory.TryAddItem(ammo, count: _gameBalanceConfig.AmmoPerClick, out List<int> slots))
                {
                    foreach (int index in slots)
                        Debug.Log($"Added {ammo.DisplayName} to slot {index}");
                }
                else
                {
                    Debug.LogError($"Not enough space for {ammo.DisplayName}");
                }
            }
        }

        private void AddItem()
        {
            List<ItemConfig> itemsConfigs = new List<ItemConfig>();
            itemsConfigs.AddRange(_weaponConfigs);
            itemsConfigs.AddRange(_armorConfigs);

            if (itemsConfigs.Count == 0)
                return;
            
            string id = _gameBalanceConfig.RandomItemIds[Random.Range(0, _gameBalanceConfig.RandomItemIds.Length)];
            ItemConfig item = _staticData.GetItemConfig(id);
            
            if (item == null)
            {
                Debug.LogError($"No item config with id {id}");
                
                return;
            }
            
            
            if (!_inventory.TryAddItem(item))
            {
                if (_inventory.HasLockedSlots && _progress.Progress.Coins >= _inventory.UnlockSlotPrice)
                {
                    _progress.Progress.Pay(_inventory.UnlockSlotPrice);
                    _inventory.UnlockNextSlot();

                    if (_inventory.TryAddItem(item))
                        Debug.Log($"Unlocked slot for {_inventory.UnlockSlotPrice} coins and added {item.DisplayName}");
                    else
                        Debug.LogError("Failed to add item after unlocking slot");
                }
                else
                {
                    Debug.LogError("No free slots to add item");
                }
            }
            else
            {
                Debug.Log($"Added {item.DisplayName}");
            }
        }

        private void DeleteItem()
        {
            List<int> occupied = _inventory.FindAllOccupiedSlotIndexes();
            if (occupied.Count == 0)
            {
                Debug.LogError("Inventory empty");
                
                return;
            }

            int index = occupied[Random.Range(0, occupied.Count)];
            ItemConfig cfg = _inventory.Slots[index].Item.Config;
            
            _inventory.RemoveItem(index);
            
            Debug.Log($"Removed {cfg.DisplayName} from slot {index}");
        }

        private void AddCoins()
        {
            _progress.Progress.AddCoins(_gameBalanceConfig.CoinsPerClick);
            
            Debug.Log($"Coins: {_progress.Progress.Coins}");
        }
        
        
        private void UpdateCoins(int value)
        {
            if (_view.CoinsText != null)
                _view.CoinsText.text = value.ToString();
        }

        private void UpdateWeight()
        {
            if (_view.WeightText != null)
                _view.WeightText.text = _inventory.GetTotalWeight().ToString();
        }
    }
}