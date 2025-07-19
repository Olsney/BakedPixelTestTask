<h1 align="center">🔥 Baked Pixel — Unity Developer Test Task</h1>

<p align="center">
  <img width="320" height="720" alt="image_2025-07-20_01-52-09" src="https://github.com/user-attachments/assets/81b7073f-875b-4b83-af16-9c58a0a44e1d" />
</p>

---

## 🕹 Project Summary

This project is a mobile inventory prototype created as a **test task** for **Baked Pixel**.  
It demonstrates clean architecture, data-driven configuration, save/load functionality, and responsive UI.  
The entire test task was completed in **3 days**.

---

## ✅ Features Implemented

- ✅ Full **inventory system** with 30 slots (scrollable, 5 items per row)
- ✅ **15 locked slots**, which can be **unlocked for coins** (customizable price)
- ✅ **MVP (Passive View)** architectural pattern
- ✅ **Zenject** for dependency injection
- ✅ **Service-based architecture**
- ✅ **State Machine** for controlling inventory/game flow
- ✅ Data **saving/loading** between sessions (not using `PlayerPrefs`)
- ✅ Config-driven setup for all items and parameters
- ✅ Full **UI sync and dynamic updates**

---

## 💼 Functional Requirements Completed

### 🔫 Fire
- Consumes one random ammo (if a matching weapon exists)
- Logs what was fired, with what ammo, and damage dealt
- Logs error if firing is not possible

### 💥 Add Ammo
- Adds 30 bullets of each type
- Stacks ammo if possible, fills new slots if needed
- Logs type, amount, and slot info
- Logs error if no space is available

### 🎁 Add Item
- Adds one random item (weapon/head/torso)
- Logs what was added and to which slot
- Logs error if no free slot

### ❌ Delete Item
- Removes all items from a random non-empty slot
- Logs what was deleted and from where
- Logs error if all slots are empty

### 💰 Add Coins
- Adds 50 coins to balance

---

## 🧩 Item System Overview

| Type       | Name             | Stackable | Weight | Notes                          |
|------------|------------------|-----------|--------|--------------------------------|
| Ammo       | Pistol Ammo      | ✅ (50)   | 0.01kg | Used by pistol                 |
| Ammo       | Rifle Ammo       | ✅ (50)   | 0.01kg | Used by rifle                  |
| Weapon     | Pistol           | ❌        | 1kg    | Deals 10 damage, uses pistol ammo |
| Weapon     | Rifle            | ❌        | 5kg    | Deals 20 damage, uses rifle ammo  |
| Armor      | Jacket           | ❌        | 1kg    | +3 torso defense               |
| Armor      | Vest             | ❌        | 10kg   | +10 torso defense              |
| Headgear   | Cap              | ❌        | 0.2kg  | +3 head defense                |
| Headgear   | Helmet           | ❌        | 1kg    | +10 head defense               |

---

## 💾 Persistence

- Inventory and currency data persist between sessions using a **custom JSON save system**
- Does **not** rely on `PlayerPrefs`
- Save data is modular and expandable

---

## 🧠 Architecture

- **Zenject**: clean dependency injection and modular structure
- **MVP Passive View**: separation of UI and logic
- **Service Layer**: manages logic like inventory, item creation, save/load, etc.
- **Configurable StaticData**: easily editable values for stacking, weights, costs, etc.
- **Scalable and extendable**: adding new item types or UI elements requires minimal changes

---

## 🛠 Tech Stack

- Unity 2022.3.6f2 (LTS)
- Zenject (Extenject)
- C#
- JSON-based save system

---

## 📱 Platform

- Target: **Mobile (Portrait)**
- Responsive UI & scrollable inventory
- Optimized for performance and extensibility

---

## 🧑‍💻 Author

**Maksym Kastorskyi**  
[GitHub](https://github.com/Olsney) · [LinkedIn](https://linkedin.com/in/maksym-kastorskyi) · [Telegram](https://t.me/M_Kast)

---

> Thanks for the opportunity!  
> I focused on building a clean, scalable, and feature-complete solution that meets all provided requirements and is ready for further development.
