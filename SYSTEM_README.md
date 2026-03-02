# 📘 Tài liệu Kiến trúc Hệ thống — FirstProjectGame

> Tài liệu này giải thích **cách dự án được tổ chức và hoạt động ở mức code**.
> Đọc xong tài liệu này, bạn sẽ hiểu flow chạy của game, cách các hệ thống kết nối với nhau, và biết file nào chịu trách nhiệm gì.

---

## Mục lục

1. [Tổng quan kiến trúc](#1-tổng-quan-kiến-trúc)
2. [Vòng đời khởi động Game](#2-vòng-đời-khởi-động-game)
3. [Singleton Managers — Lõi điều phối](#3-singleton-managers--lõi-điều-phối)
4. [Player System](#4-player-system)
5. [Enemy AI & Grid Pathfinding](#5-enemy-ai--grid-pathfinding)
6. [Inventory & Equipment System](#6-inventory--equipment-system)
7. [Item Data Architecture](#7-item-data-architecture)
8. [Combat & Weapon System](#8-combat--weapon-system)
9. [Quest System](#9-quest-system)
10. [NPC & Dialogue System](#10-npc--dialogue-system)
11. [Scene Management](#11-scene-management)
12. [Environment, Time & Weather](#12-environment-time--weather)
13. [Map Object & Event System](#13-map-object--event-system)
14. [Loot & Drop System](#14-loot--drop-system)
15. [Object Pool System](#15-object-pool-system)
16. [Build System](#16-build-system)
17. [UI Architecture](#17-ui-architecture)
18. [Save / Load System](#18-save--load-system)
19. [Audio System](#19-audio-system)
20. [Sơ đồ tham chiếu giữa các hệ thống](#20-sơ-đồ-tham-chiếu-giữa-các-hệ-thống)

---

## 1. Tổng quan kiến trúc

Dự án sử dụng mô hình **Singleton + Component-based** của Unity:

- **Các Manager Singleton** (`GameManageMent`, `UIManageMent`, `SaveLoadManager`, `SceneLoader`, `AudioManager`, `PlayerManager`) tồn tại xuyên suốt game nhờ `DontDestroyOnLoad`. Chúng là điểm truy cập toàn cục qua `Instance`.
- **Dữ liệu cấu hình** được lưu trong **ScriptableObject** (ItemData, EnemyBaseData, QuestDefinition, RecipeData, SceneData…). Điều này cho phép chỉnh sửa trực tiếp qua Inspector mà không cần sửa code.
- **Logic runtime** (InventorySystem, EquipMentSystem, WorldManager) là **plain C# class** — không kế thừa MonoBehaviour, được các Manager khởi tạo và giữ tham chiếu.
- **Giao tiếp giữa hệ thống** chủ yếu dùng **C# event/Action** và **UnityEvent**, không dùng message bus bên thứ ba.

```
┌──────────────────────────────────────────────────────────┐
│                  DontDestroyOnLoad Layer                  │
│                                                          │
│  GameManageMent ◄── trung tâm, giữ ref tới tất cả       │
│  ├── PlayerManager                                       │
│  ├── PoolManager                                         │
│  ├── DropSystem                                          │
│  ├── QuestManager                                        │
│  ├── TimeManager                                         │
│  ├── EnviromentManager                                   │
│  ├── BuildManager                                        │
│  ├── InventoryAndEquipmentManager                        │
│  ├── EffectController                                    │
│  ├── GridManagement (ref, set từ scene)                  │
│  └── WorldManager (plain C# class)                       │
│                                                          │
│  UIManageMent ◄── trung tâm UI                           │
│  SaveLoadManager                                         │
│  SceneLoader                                             │
│  AudioManager                                            │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│                    Per-Scene Layer                        │
│  GridManagement (mỗi scene có grid riêng)                │
│  Enemy instances, NPC instances, Chest, Door…            │
│  Map Bound (PolygonCollider2D)                           │
└──────────────────────────────────────────────────────────┘
```

---

## 2. Vòng đời khởi động Game

### Thứ tự Awake → Start → Gameplay

```
[MenuScene load]
  │
  ├─ GameManageMent.Awake()
  │   ├─ Instance = this, DontDestroyOnLoad
  │   ├─ GetComponent tất cả sub-manager trên cùng GameObject
  │   │   (PlayerManager, PoolManager, DropSystem, QuestManager,
  │   │    TimeManager, EnviromentManager, EffectController, BuildManager,
  │   │    InventoryAndEquipmentManager)
  │   ├─ **Disable hết** các sub-manager (enabled = false)
  │   │   → Chúng không chạy Update/Start cho đến khi game thật sự bắt đầu
  │   ├─ Tạo WorldManager (new WorldManager())
  │   ├─ gameState = Pause
  │   └─ Set cursor, target FPS = 120
  │
  ├─ UIManageMent.Awake() → Instance, DontDestroyOnLoad, init UI
  ├─ SaveLoadManager.Awake() → Instance, DontDestroyOnLoad
  ├─ SceneLoader.Awake() → Instance
  ├─ AudioManager.Awake() → Instance, DontDestroyOnLoad, play BGM menu
  │
  [Người chơi bấm "New Game" hoặc "Load Game"]
  │
  ├─ New Game → SceneLoader.LoadScene(Map1, defaultPos)
  │   hoặc
  ├─ Load Game → SaveLoadManager.LoadGame()
  │     ├─ Đọc JSON → GameSaveData
  │     ├─ Lấy SceneData từ worldData.idSceneData
  │     └─ SceneLoader.LoadScene(sceneData, savedPos)
  │
  [SceneLoader.LoadSceneAsync coroutine]
  │
  ├─ Hiển thị Loading UI, Pause game
  ├─ SceneManager.LoadSceneAsync (Single mode)
  ├─ Khi scene loaded lần đầu:
  │   └─ GameManageMent.StartGame()
  │       ├─ **Enable** tất cả sub-manager
  │       ├─ Bật camera, bật light
  │       └─ isGameStarted = true
  ├─ SaveLoadManager.LoadDataRemain()  ← load player/inventory/quest/world state
  ├─ Set player position, set camera bound
  ├─ Switch environment (Indoor/Outdoor), play BGM
  └─ GameManageMent.Continue() → gameState = Continue
```

**Điểm mấu chốt:** Tất cả sub-manager nằm trên **cùng 1 GameObject** với `GameManageMent` và bị **disabled** cho đến khi scene gameplay đầu tiên load xong. Điều này tránh NullReference khi các object chưa sẵn sàng.

---

## 3. Singleton Managers — Lõi điều phối

### GameManageMent (`Script/GameManageMent.cs`)

**Vai trò:** God object trung tâm, nắm giữ tham chiếu tới mọi hệ thống.

| Property | Kiểu | Mô tả |
|---|---|---|
| `PlayerManager` | PlayerManager | Quản lý player stat, health, bullet |
| `PoolManager` | PoolManager | Các object pool (loot, bullet, enemy, text…) |
| `DropSystem` | DropSystem | Xử lý drop item khi giết quái / phá vật |
| `QuestManager` | QuestManager | Nhiệm vụ đang làm, đã hoàn thành |
| `TimeManager` | TimeManager | Chu kỳ ngày đêm |
| `EnviromentManager` | EnviromentManager | Indoor/Outdoor, ánh sáng |
| `BuildManager` | BuildManager | Chế độ xây dựng |
| `InventoryAndEquipmentManager` | InventoryAndEquipmentManager | Kho đồ + trang bị |
| `GridManagement` | GridManagement | Grid pathfinding (set từ scene) |
| `_WorldManager` | WorldManager | Trạng thái thế giới (chest, boss, event) |
| `EffectController` | EffectController | Flash damage, hiệu ứng |
| `ItemDataBase` | ItemDataBase (SO) | Database toàn bộ item |
| `EnemyDataBase` | EnemyDataBase (SO) | Database toàn bộ enemy |

**Chức năng chính:**
- `PauseGame()` / `Continue()` — toggle `GameState` enum (Pause/Continue). Khi Pause, player và enemy đều dừng Update.
- `ControlMenu()` — xử lý ESC mở/đóng menu, Tab chuyển tab.
- `SetBoundMap(PolygonCollider2D)` — cập nhật camera confiner khi đổi scene.
- `CalculateDirType(x, y)` — tính hướng gần nhất (LEFT/RIGHT/UP/DOWN) từ vector, dùng cho animation và logic hướng.

### UIManageMent (`Script/UIManageMent.cs`)

**Vai trò:** Trung tâm điều khiển tất cả UI.

Giữ tham chiếu tới: `ShopSystem`, `ExpStatSystemUI`, `EquipmentSystemUI`, `InventoryUI`, `BulletUIController`, `DialogueUI`, `QuestUI`, `LoadingAdditive`, `PopUpAddedItem`.

**Cơ chế đặc biệt:**
- **HP Bar & EXP Bar**: Không set trực tiếp mà dùng `fillTarget` + `Mathf.Lerp` trong `Update()` → thanh bar chạy mượt.
- **Item Popup Queue**: Khi nhặt vật phẩm, thay vì hiển thị ngay, item được đẩy vào `addedItemQueue`. Update() kiểm tra nếu popup hiện tại đã fade xong → dequeue item tiếp theo.
- **Warning**: Dùng DOTween DOFade để cảnh báo tự mờ dần sau 2 giây.

### GameConfig (`Script/GameConfig.cs`)

**Vai trò:** Chứa hằng số dùng chung (tag name, layer name, animation parameter, color code). Tất cả là `static string` → truy cập `GameConfig.PLAYER_TAG0` ở mọi nơi, tránh hard-code string.

---

## 4. Player System

### Sơ đồ component trên Player GameObject

```
Player (GameObject, tag: "Player")
├── PlayerController      : MonoBehaviour   ← điều khiển input, di chuyển, tấn công
├── SlotPlayerController  : MonoBehaviour   ← quản lý equip slot (1-5), switch weapon
├── LootSystem            : MonoBehaviour   ← nhặt vật phẩm từ LootItem
├── ExpSystem             : MonoBehaviour   ← level, EXP, point stat
├── GoldPlayer            : MonoBehaviour   ← tiền tệ
├── StatPlayer            : MonoBehaviour   ← ATK, MaxHP, CritRate, Speed, upgrade
├── Health                : MonoBehaviour   ← HP, damage, heal, die event
├── PunchController       : MonoBehaviour   ← hitbox đấm cận chiến  
├── Rigidbody2D
├── Animator (Blend Tree)
└── Light (bật/tắt theo đêm)
```

### PlayerController (`Script/Player/PlayerController.cs`)

**Flow mỗi frame:**

```
Update() [chỉ chạy khi GameState == Continue]
├── Move()                  ← di chuyển theo input WASD
│   ├── Nếu đang punch hoặc weapon attacking → return
│   ├── AnimUpdate() → set Blend Tree parameter (MoveX, MoveY, Speed)
│   └── rb.MovePosition() với StatPlayer.Speed
├── InteractNpc()           ← Raycast chuột lên NPC layer
│   ├── Hit → đổi cursor sang interact, chuột phải → NPC.TurnOnInteract()
│   └── Không hit → reset cursor
├── UpdateCountDown()       ← đếm cooldown tấn công
│   └── Attack()            ← chuột trái → vũ khí.Attack() hoặc punch
├── SlotPlayerController.ChooseSlot() ← phím 1-5 đổi slot
├── LootSystem.CheckHoverItem()       ← hover vật phẩm trên đất
└── Build mode check        ← nếu đang build, click = đặt object
```

**Kết nối quan trọng:**
- `Awake()` → gọi `GameManageMent.Instance.PlayerManager.SetPlayerComponent(...)` để đăng ký các component con lên PlayerManager.
- `Start()` → đăng ký event:
  - `Health.OnHealthChanged` → `UIManageMent.SetHealthBar`
  - `ExpSystem.OnExpChange` → `UIManageMent.SetExpBar`
  - `TimeManager.ChangeToMidNight` → `TurnOnLight`
  - `TimeManager.ChangeToDay` → `TurnOffLight`
  - Camera Follow: `CinemachineVirtualCamera.Follow = this.transform`

### PlayerManager (`Script/Player/PlayerManager.cs`)

**Vai trò:** Trung gian truy cập tất cả sub-component của Player. Được gắn trên cùng GameObject với GameManageMent (DontDestroyOnLoad).

**Logic quan trọng:**
- `CalculateCritDamage(ref float damage)` — cộng `stat.Atk` vào damage, roll random tỷ lệ crit, nếu crit → nhân `CritDamagePercentage`.
- `GetSavePlayerData()` → tạo `PlayerData` chứa toàn bộ stat hiện tại để save.
- `LoadPlayerData(PlayerData)` → restore stat, position, bullet.
- Quản lý đạn theo `GunType` (SHOTGUN/PISTOL/GUN): `totalBullet` và `curBullet` cho mỗi loại.

### SlotPlayerController (`Script/Player/SlotPlayerController.cs`)

**Cơ chế equip slot (phím 1-5):**

```
EquipSlot(int slot)
├── Lấy ItemData từ EquipMentSystem.Slots[slot]
├── Destroy weapon prefab cũ (nếu có)
├── Tắt build mode (nếu slot cũ là Buildable)
├── Switch theo ItemType:
│   ├── Gun → Instantiate gun prefab, set weapon, cập nhật anim
│   ├── Melee → Instantiate melee prefab
│   ├── Buildable → bật BuildManager.TurnOnBuildMode()
│   └── null/other → unequip weapon anim
└── Update UI slot highlight
```

### StatPlayer (`Script/Player/Stat/StatPlayer.cs`)

Chứa stat nâng cấp được: `maxHP`, `atk`, `critRate`, `speed`, `critDamagePercentage`.

**Cơ chế upgrade:**
1. Kiểm tra `ExpSystem.PointStat > 0`
2. Trừ 1 point → tăng stat theo `growth` value (ví dụ: `maxHP += healthGrowth`)
3. Tăng counter `pointMaxHp/pointAtk/pointCritRate` (để save/load)
4. Cập nhật UI

### Health (`Script/Player/Stat/Health.cs`)

Dùng chung cho **Player** và **destructible object**:
- `OnDamaged(float)` → trừ HP, flash effect (đổi material tạm thời), fire `OnHealthChanged` event.
- `OnHealed(float)` → cộng HP, hiệu ứng xanh bằng DOTween.
- Phân biệt object qua `tag` — nếu tag = "Player" mới fire event cập nhật UI.

---

## 5. Enemy AI & Grid Pathfinding

### Tổng quan luồng AI

```
GridManagement (mỗi scene)
├── GridBuilder        ← tạo lưới 2D từ Tilemap/collider
├── DistanceField      ← BFS tính khoảng cách từ mỗi ô đến player
└── FlowField          ← tính hướng đi tối ưu cho mỗi ô

Enemy (EnemyBase)
├── Đọc FlowField → biết hướng đi tới player
├── Context Steering → tránh vật cản
└── Separation Force → tránh chồng chất enemy
```

### GridBuilder (`Script/Grid/GridBuilder.cs`)

- Tạo lưới `720×515` ô (cấu hình qua Inspector), mỗi ô `0.7 unit`.
- `initGrid()`: Loop qua tất cả ô, dùng `Physics2D.OverlapBox` để phát hiện collider → đánh dấu `CellType`: Walkable / Blocked / Breakable.
- `WorldToGridPosition(Vector2)` ↔ `GridToWorldPosition(Vector2)`: chuyển đổi tọa độ.

### DistanceField (`Script/Grid/DistanceField.cs`)

- **Thuật toán BFS** từ vị trí player, lan ra trong chunk (ChunkX × ChunkY).
- Mỗi ô lưu `distanceFromPlayer` = số bước BFS (8 hướng).
- Chạy lại định kỳ qua `GridManagement.UpdateGridField()` coroutine với `timeUpdateGrid` giây.

### FlowField (`Script/Grid/FlowField.cs`)

- Sau khi có DistanceField, mỗi ô tính `flowDirection` = hướng trung bình tới ô neighbour có distance nhỏ nhất.
- Enemy chỉ cần đọc `gridCells[x][y].FlowDirection` → biết đi hướng nào.

### EnemyBase (`Script/Enemy/EnemyController/EnemyBase.cs`) — abstract class

**State Machine đơn giản:**

```
enum State { Idle, Attack, Chase }

FixedUpdate()
├── switch(curState)
│   ├── Idle → OnIdle()
│   │   ├── Kiểm khoảng cách → chuyển Attack hoặc Chase
│   │   ├── Wandering: random hướng mỗi `wanderingCooldown` giây
│   │   └── OnMove(wanderingDir, walkSpeed)
│   ├── Chase → OnChase()
│   │   ├── Đọc FlowField → OnMove(flowDir, runSpeed)
│   │   ├── Nếu distance = ∞ → random dir (mất đường)
│   │   └── Chuyển Idle nếu quá xa, chuyển Attack nếu đủ gần
│   └── Attack → OnAttack() [abstract, override bởi subclass]
```

**OnMove() — Context Steering:**

```
OnMove(flow, speed)
├── Tính Interest: mỗi hướng (8 hướng) = dot(dir, flow) × wFlow
├── Tính Danger: BoxCast 8 hướng, nếu hit → danger = wAvoid × (1 - distance/avoidDistance)
├── Chọn hướng có score (interest - danger) cao nhất
├── Cộng Separation Force: OverlapCircle tìm enemy gần → đẩy ra xa
├── Lerp currentDir → bestDir (mượt)
└── rb.MovePosition theo dir × speed
```

**Cơ chế spawn/die:**
- Enemy không ở pool: `SetDie()` → ẩn sprite, chờ `timeSpawn` giây → `SpawnEnemy()` tại vị trí gốc.
- Enemy trong pool: `SetDie()` → `PoolManager.EnemytPoolsList[index].DeSpawn(this)`.

### EnemyMelee (`Script/Enemy/EnemyController/EnemyMelee.cs`)

- Override `OnAttack()`: Trigger animation "IsAttack", gọi `CauseDamage()` (Animation Event) → `player.Health.OnDamaged(attack)`.

### EnemyRange (`Script/Enemy/EnemyController/EnemyRange.cs`)

- Override `OnAttack()`: Trigger animation, gọi `SpawnBullet()` (Animation Event) → spawn bullet từ pool theo hướng player.

### Boss System (`Script/Enemy/BossController/`)

- `BossManagerInterface` — interface/base cho boss manager.
- Mỗi boss có folder riêng (GhostKing, HuyBoss) với `Manager` + `Combat` class.
- `ActiveBossEvent` — kích hoạt boss khi player tới khu vực.
- Boss có `BossStat`, `BossMovement`, `BossVisual`, `SkillBoss` riêng.
- Khi boss chết → `WorldManager.AddDefeatedBoss(id)` để save.

---

## 6. Inventory & Equipment System

### Kiến trúc phân tầng

```
InventoryAndEquipmentManager (MonoBehaviour, trên GameManageMent GO)
├── InventorySystem (plain C# class)
│   └── List<InventorySlot>   ← mỗi slot chứa ItemData + count
├── EquipMentSystem (plain C# class)
│   └── List<EquipMentSlot>   ← 5 slot equip nhanh
├── InventoryUI (MonoBehaviour) ← hiển thị UI inventory
└── EquipmentSystemUI (MonoBehaviour) ← hiển thị UI equipment bar
```

### Slot hierarchy

```
Slot (abstract)
├── itemData: ItemData
├── count: int
├── Set(), Remove(), Clear(), IsEmpty()
│
├── InventorySlot : Slot
│   └── Add(int amount)
│
└── EquipMentSlot : Slot
```

### InventorySystem (`Script/Inventory/InventoryData/InventorySystem.cs`)

**Các thao tác chính:**

| Method | Mô tả |
|---|---|
| `TryAdd(ItemData, amount)` | Kiểm tra có slot trống/stackable không, return bool |
| `Add(ItemData, amount)` | Thêm item vào inventory, cập nhật `itemCount` dict |
| `TryRemoveItem(ItemData, amount)` | Kiểm tra có đủ item để xóa |
| `RemoveItem(ItemData, amount)` | Xóa item, fire `OnChangeInventory` event |

**Event:**
- `OnChangeInventory` → QuestManager lắng nghe để cập nhật progress quest collect.

**Đặc biệt:**
- `ItemType.Bullet` bỏ qua check slot (đạn lưu ở `PlayerManager.shotgunBullet/pistolBullet/gunBullet`, không chiếm slot).

### EquipMentSystem (`Script/EquipMent/EquipMentData/EquipMentSystem.cs`)

- 5 slot equip (kích thước cấu hình qua Inspector).
- `TryEquip(ItemData, amount)` — tìm slot rỗng đầu tiên, gán vào.
- `UseSlot(index, amount)` — tiêu hao vật phẩm (ví dụ: đặt tường xây dựng → trừ 1).
- `OnEquipmentChange` event → UI refresh.

---

## 7. Item Data Architecture

### Class hierarchy (tất cả là ScriptableObject)

```
ItemData (abstract SO)                    Script/Inventory/InventoryData/Item/Code/ItemData.cs
├── description, stackable, maxStack
├── type: ItemType enum { HpPotion, Gun, Melee, Bullet, Material, Buildable, QuestItem }
├── itemName, index (ID duy nhất), icon, value (giá bán)
│
├── HpPotionData : ItemData               ← thuốc hồi máu
├── MaterialData : ItemData               ← nguyên liệu crafting
├── BulletData : ItemData                  ← đạn (không chiếm slot inventory)
├── BuildableData : ItemData               ← vật thể xây dựng
│   └── Index_BuildableObject (trỏ tới BuildManager.buildableObjects)
│
├── WeaponData : ItemData                  ← base vũ khí
│   ├── damaged, coolDown
│   │
│   ├── MeleeData : WeaponData            ← vũ khí cận chiến
│   │   └── Melee prefab reference
│   │
│   └── GunData : WeaponData              ← vũ khí bắn
│       ├── gunType: GunType { PISTOL, GUN, SHOTGUN }
│       ├── magSize, reloadTime
│       ├── gun: Weapon prefab
│       ├── indexBullet (trỏ tới PoolManager.BulletPoolsList)
│       └── bulletUI: Sprite
```

### ItemDataBase (`Script/Inventory/InventoryData/Item/ItemDataBase.cs`)

- ScriptableObject chứa `List<ItemData>` — toàn bộ item trong game.
- Truy cập qua `GameManageMent.Instance.ItemDataBase.ItemDatas[index]`.
- **`index` của ItemData = vị trí trong list** → dùng làm ID serialize khi save/load.

---

## 8. Combat & Weapon System

### Weapon hierarchy

```
Weapon (abstract MonoBehaviour)            Script/Inventory/InventoryData/Weapon/Weapon.cs
├── weaponData: WeaponData
├── attacking: bool
├── Attack(dirX, dirY) — virtual
├── UpdateAnim(dirX, dirY) — virtual
├── EndAttack() — gọi từ Animation Event
│
├── Gun : Weapon                          Script/Inventory/InventoryData/Weapon/Gun.cs
│   ├── curBullet, totalBullet, reloading
│   ├── Attack() → SpawnBullet() → BulletPool.Spawn()
│   ├── Reload() → coroutine chờ reloadTime giây
│   ├── Recoil effect (Cinemachine Impulse Source)
│   ├── Fire visual (FireShoot sprite flash)
│   └── Update bullet UI mỗi lần bắn
│
├── Melee : Weapon
│   └── Attack() → trigger animation, hitbox active
│
└── SpecialGun : Weapon (?)
```

### BulletController (`Script/Inventory/InventoryData/Weapon/BulletController.cs`)

- Spawn từ ObjectPool, bay theo hướng, kiểm tra va chạm.
- `SetInfo(damage, indexBullet)` → gán damage và pool index.
- Khi hit → despawn về pool.

### Luồng chiến đấu tổng thể

```
Player nhấn chuột trái
├── PlayerController.Attack()
│   ├── Có weapon (Gun):
│   │   └── weapon.Attack(dir) → Gun.SpawnBullet()
│   │       ├── PoolManager.BulletPoolsList[gunData.IndexBullet].Spawn(pos)
│   │       ├── bullet.Fire(dir)
│   │       ├── Recoil camera shake
│   │       ├── curBullet--
│   │       └── Update bullet UI
│   ├── Có weapon (Melee):
│   │   └── weapon.Attack(dir) → trigger anim → hitbox → damage
│   └── Không có weapon:
│       └── UpdatePunchAnim() → PunchController hitbox

Damage tính toán:
├── PlayerManager.CalculateCritDamage(ref damage)
│   ├── damage += stat.Atk
│   ├── Random critRate → nếu crit: damage *= critDamagePercentage
│   └── return isCrit (để hiển thị floating text khác màu)

Enemy nhận damage:
├── HealthEnemy.OnDamaged(damage)
│   ├── cur_health -= damage
│   ├── Flash effect
│   ├── Hiển thị floating text (từ pool)
│   └── Nếu die → DropSystem.DropItem() + GainExp + quest update
```

---

## 9. Quest System

### Cấu trúc dữ liệu

```
QuestDefinition (ScriptableObject)
├── id, nameQuest, description
├── npcId (NPC giao quest)
├── List<ItemStack> itemQuestList    ← vật phẩm đưa cho player khi nhận quest
├── List<Objective> objectives        ← danh sách mục tiêu
├── goldReward, expReward
└── List<ItemStack> itemIdReward     ← vật phẩm thưởng

Objective (Serializable class)
├── objectiveType: { Collect, Kill, TalkToNpc, ReachArea }
├── targetId (item ID / enemy ID / NPC ID)
├── requiredCount
├── haveDirection → destinationPosition, destinationIdSceneData

QuestProgress (runtime tracking)
├── questId
├── List<int> curCount  ← tiến độ mỗi objective
├── UpdateProgressKill(), UpdateCollectProgress(), UpdateProgressTalkToNpc()
└── checkProgress() → return true nếu tất cả objective đạt requiredCount
```

### QuestManager (`Script/Quest/QuestManager.cs`)

**Luồng quest:**

```
1. NPC giao quest → AcceptQuest(QuestDefinition)
   ├── Thêm vào curQuestDefinitions + questProgresses
   ├── Thêm quest items vào inventory (nếu có)
   └── Fire OnQuestChange event → QuestUI refresh

2. Trong gameplay:
   ├── Giết quái → UpdateProgressAllQuestKill(amount, enemyId, objectiveType)
   ├── Thu thập item → InventorySystem.OnChangeInventory
   │   → QuestManager.UpdateProgressAllQuestCollect()
   └── Nói chuyện NPC → UpdateProgressAllQuestTalkToNpc(npcId)

3. Quay về NPC → CompleteQuest:
   ├── questProgress.checkProgress() → nếu false → báo chưa xong
   ├── Nếu true:
   │   ├── Spawn reward items (LootPool)
   │   ├── AddGold, GainExp
   │   ├── Chuyển quest sang completedQuest
   │   └── Fire OnCompleteQuest event
```

**NPC Data tracking:** `Dictionary<int, NpcDataValue>` lưu trạng thái hội thoại mỗi NPC (đang trong quest, dialogue index nào) → save/load được.

---

## 10. NPC & Dialogue System

### NPC (`Script/NPC/NPC.cs`)

**Dữ liệu NPC (Inspector):**
- `npcId`, `nameNpc`, `npcAvatar`
- `List<NpcDialogue>` — mảng các chuỗi hội thoại, mỗi chuỗi có:
  - `List<DialogueLine>` (name + content)
  - `QuestDefinition` gắn với chuỗi hội thoại này
  - `indexQuestDialogue` — dòng dialogue nào bật nút Accept/Refuse
  - `acceptQuestLine` / `refuseQuestLine` — text cho button
- `interactRadius`, `unlocked`, `questIdRequired` (cần hoàn thành quest nào để mở khóa NPC)

**Luồng tương tác:**

```
Player đến gần NPC (< interactRadius) → hiện phím tương tác
├── Nhấn E → NPC.TurnOnInteract()
│   ├── UnlockNPC() → kiểm tra questIdRequired đã complete chưa
│   ├── Lấy NpcDataValue từ QuestManager (restore trạng thái hội thoại)
│   ├── Mở DialogueUI
│   └── StartDialogue()
│
├── StartDialogue() logic:
│   ├── Nếu hết dialogue / NPC chưa unlock → hiện endTalk
│   ├── Nếu đang trong quest (onQuest = true):
│   │   ├── Hiện "đang làm quest" dialogue
│   │   └── Bật nút Complete / OnGoing quest
│   ├── Bình thường:
│   │   ├── Hiện DialogueLine[indexDialogue]
│   │   ├── Phân biệt talker bằng name → đổi avatar
│   │   ├── Nếu indexDialogue == indexQuestDialogue:
│   │   │   └── Bật nút Accept / Refuse quest
│   │   └── indexDialogue++
│
├── Nhấn Space → tiếp tục dialogue / tắt nếu hết
└── TurnOffInteract() → lưu NpcDataValue lại QuestManager
```

### DialogueUI (`Script/Dialogue/DialogueUI.cs`)

- **Typewriter effect:** Coroutine hiện từng ký tự, tốc độ = `charsPerSecond`, delay thêm sau dấu câu.
- **Nhấn Space** giữa chừng → `ShowInstant()` hiện hết ngay.
- Nút Accept/Refuse: gọi callback (`UnityAction`) do NPC truyền vào.

---

## 11. Scene Management

### Kiến trúc

```
SceneLoader (Singleton, DontDestroyOnLoad)
├── sceneDatabase: List<SceneData>     ← tất cả scene trong game
├── currentSceneData: SceneData        ← scene đang active
├── SceneNavigationManager

SceneData (ScriptableObject)
├── idSceneData (int, dùng save/load)
├── nameScene (tên scene trong Build Settings)
├── nameBounder (tên GameObject chứa PolygonCollider2D boundary)
├── typeMap: { MAINMAP, SECONDARYMAP }
├── environmentType: { Indoor, Outdoor }
├── lightIntense, lightColor (cho Indoor)
├── parentSceneData (scene cha, dùng khi unload additive)

SceneGraph (ScriptableObject)
└── List<SceneEdge>   ← đồ thị liên kết giữa các scene

SceneEdge
├── fromScene, toScene
├── entry/exit points
```

### Hai cách load scene

**1. LoadScene (Single) — chuyển hoàn toàn sang scene mới:**

```
SceneLoader.LoadScene(sceneData, startPoint)
└── LoadSceneAsync coroutine:
    ├── Hiện Loading UI, Pause game
    ├── GC.Collect() (dọn rác lúc loading)
    ├── SceneManager.LoadSceneAsync(name, Single)
    ├── Chờ progress = 0.9 (Unity max khi allowSceneActivation=false)
    ├── Fill loading bar đến 100%
    ├── allowSceneActivation = true
    ├── Nếu game chưa start → StartGame()
    ├── LoadDataRemain() → restore toàn bộ data
    ├── Set player position, camera bound
    ├── SwitchEnvironment (Indoor/Outdoor)
    └── Play BGM phù hợp
```

**2. LoadSceneAdditive — load scene phụ chồng lên scene chính:**

- Dùng cho indoor (nhà, dungeon): scene indoor load additive trên Map1.
- `BackToMainScene()` → `UnLoadAsync()` unload scene phụ, quay về parent scene.
- Khi unload: set lại camera bound của parent, switch environment.

### TeleportScenePortal (`Script/Scene/TeleportScenePortal.cs`)

- Đặt tại cổng/cửa. Khi player trigger → gọi `SceneLoader.LoadSceneAdditive()` hoặc `BackToMainScene()`.

---

## 12. Environment, Time & Weather

### TimeManager (`Script/TimeManager.cs`)

```
Update() mỗi frame:
├── elapseTime += Time.deltaTime
├── t = (elapseTime / (timerPerDay * 60)) % 1    ← giá trị 0→1 lặp lại
├── t ∈ [0, 0.15)    → Day        → fire ChangeToDay event
├── t ∈ [0.15, 0.4)  → MidDay     → fire ChangeToMidDay event
├── t ∈ [0.4, 0.55)  → Night      → fire ChangeToNight event
├── t ∈ [0.55, 1.0)  → MidNight   → fire ChangeToMidNight event
```

`timerPerDay` (phút) = thời gian thực cho 1 ngày trong game.

### EnviromentManager (`Script/Enviroment/EnviromentManager.cs`)

```
EnviromentManager
├── IndoorEnvironment : EnviromentBase
│   └── Apply(): set light2D.intensity và color từ SceneData
│
└── OutdoorEnvironment : EnviromentBase
    ├── Đăng ký event TimeManager (ChangeToDay/MidDay/Night/MidNight)
    ├── Mỗi phase → đổi light color, cường độ sáng theo AnimationCurve
    ├── Mỗi ngày mới (OnDay) → WeatherSystem.RandomWeatherState()
    └── WeatherSystem
        ├── List<(WeatherState, weight)> ← tỷ lệ random
        ├── NORMAL / RAIN / FOG
        └── RAIN → ParticleSystem.Play() + AudioManager.PlayRain()
```

**SwitchEnvironment(type):** Khi chuyển scene, gọi Indoor.SetActive / Outdoor.SetActive. Outdoor lắng nghe TimeManager event để đổi ánh sáng; Indoor chỉ dùng giá trị cố định từ SceneData.

---

## 13. Map Object & Event System

### Sender / Receiver Pattern

```
SenderEvent (MonoBehaviour, IRestorable)
├── uniqueId: string (GUID)
├── List<string> eventSend  ← tên sự kiện gửi đi
├── SendEvent() → EventManager.OnSignalSent(eventName, true)
├── sendOneTime → chỉ gửi 1 lần rồi disable

ReceiverEvent (MonoBehaviour, IRestorable)
├── uniqueId: string (GUID)
├── List<Pair<string, bool>> requiredEvents
├── OnEnable → đăng ký EventManager.OnSignalSent
├── CheckCondition() → kiểm tra tất cả required events đã thỏa mãn
│   └── Kích hoạt hành động (mở cửa, hiện vật thể, play anim…)

EventManager (Singleton, plain C# class)
├── Dictionary<string, bool> currentEventSignal
├── Action<string, bool> OnSignalSent  ← event broadcast
├── Save/Load: chỉ lưu danh sách event đã xảy ra
```

**Ứng dụng:**
- `EnemySpawner : SenderEvent` → giết hết quái → `SendEvent()` → mở cửa (`Door : ReceiverEvent`).
- `ButtonTrigger : SenderEvent` → player nhấn nút → gửi event → kích hoạt `ReceiverEvent`.
- `Chest` → mở rương → drop loot → lưu `WorldManager.AddOpenedChest(uniqueId)`.
- `UnlockedStatue`, `WallHaveAnim` → nhận event để thay đổi trạng thái.

### IRestorable interface

```csharp
interface IRestorable {
    void Restore(string id);
}
```

Khi load game, `WorldManager` fire `OnLoadDataObject(id)` cho mọi chest/object đã tương tác → object tự kiểm tra uniqueId và restore trạng thái.

---

## 14. Loot & Drop System

```
DropSystem (trên GameManageMent GO)
├── List<LootTableData> lootTableDataBase
│
├── DropItem(int index, int amount, Vector2 pos)
│   ├── LootTableData[index].GetRandomItem() ← weighted random
│   └── Loop amount lần:
│       ├── LootItem = PoolManager.LootPool.Spawn(pos)
│       └── lootItem.SetInfo(itemId, count)

LootItem (MonoBehaviour, IPoolable)
├── Vật phẩm trên sàn, có sprite, bob animation
├── Player đến gần → LootSystem.CheckHoverItem() detect
├── Nhấn loot → thêm vào Inventory → DeSpawn về pool

LootTableData (ScriptableObject)
├── List<(ItemData, weight)>
└── GetRandomItem() → weighted random chọn 1 ItemData
```

---

## 15. Object Pool System

### ObjectPool<T> (`Script/Pool/ObjectPool.cs`)

Generic pool cho mọi loại Component:

```csharp
ObjectPool<T> where T : Component
├── Queue<T> pool
├── Spawn(Vector2 pos) → dequeue hoặc Instantiate, SetActive(true)
│   └── (as IPoolable)?.OnSpawn()
├── DeSpawn(T obj) → nếu pool chưa full → SetActive(false), enqueue
│   └── (as IPoolable)?.OnDeSpawn()
│   └── nếu pool đầy → Destroy
```

### PoolManager (`Script/Pool/PoolManager.cs`)

| Pool name | Type | Prefab | Mục đích |
|---|---|---|---|
| `lootPool` | ObjectPool\<LootItem\> | LootItem | Vật phẩm trên sàn |
| `floatingTextPool` | ObjectPool\<FloatingText\> | FloatingText | Hiển thị damage/EXP lên màn hình |
| `bulletPoolsList` | List\<ObjectPool\<BulletController\>\> | Nhiều loại đạn | Đạn player |
| `enemyPoolsList` | List\<ObjectPool\<EnemyBase\>\> | Nhiều loại quái | Quái spawn từ spawner |
| `ghostSpritePools` | ObjectPool\<GhostSprite\> | GhostSprite | Hiệu ứng ghost trail |
| `skill3GhostKingPool` | ObjectPool\<SkillBoss\> | SkillBoss | Kỹ năng boss GhostKing |

---

## 16. Build System

```
BuildManager (trên GameManageMent GO)
├── buildMode: bool
├── List<BuildableObject> buildableObjects ← danh sách vật thể xây được
├── BuildPlacement
│   ├── GhostPreview: hiển thị bóng mờ vật thể trước khi đặt
│   ├── SetPos() — update vị trí ghost theo chuột, snap grid
│   ├── CanPlace() — kiểm tra collision
│   └── PlaceObject() — Instantiate thực tế
│
├── TurnOnBuildMode(index) → bật từ SlotPlayerController khi equip BuildableData
└── TurnOffBuildMode() → tắt khi hết vật liệu hoặc đổi slot
```

**Luồng sử dụng:**
1. Player equip BuildableData vào slot → `SlotPlayerController.EquipSlot()` → `BuildManager.TurnOnBuildMode(buildableData.Index_BuildableObject)`
2. Mỗi frame `BuildPlacement.SetPos()` → ghost preview theo chuột
3. Click chuột trái → kiểm tra `CanPlace()` → `PlaceObject()` + trừ 1 item khỏi equip slot
4. Hết vật liệu → `TurnOffBuildMode()`

---

## 17. UI Architecture

### Cây UI chính

```
UIManageMent (Canvas, DontDestroyOnLoad)
├── HUD luôn hiển thị:
│   ├── HP Bar (Image fill)
│   ├── EXP Bar (Image fill) + Level Text
│   ├── Gold Text
│   ├── BulletUIController (loại đạn, số lượng)
│   ├── Reloading Text
│   └── PopUpAddedItem (popup nhặt item)
│
├── Menu (MenuController → toggle ESC)
│   ├── MenuTab[] (Tab system: Inventory, Equipment, Stat, Quest, Craft)
│   │   ├── InventoryUI → slot grid, drag & drop
│   │   ├── EquipmentSystemUI → 5 slot equip
│   │   ├── ExpStatSystemUI → hiển thị stat, nút upgrade
│   │   ├── QuestUI → danh sách quest + QuestViewInfo chi tiết
│   │   └── CraftUI → danh sách RecipeUI
│   └── SettingUI
│
├── ShopSystem → BuySystem + ShopSlot[]
├── DialogueUI → typewriter, avatar, accept/refuse buttons
├── LoadingAdditive → màn hình loading (fill bar)
└── Warning Text (auto fade)
```

### Pattern UI thường gặp trong dự án

1. **MenuLayOutUI** — base class cho mỗi tab, cung cấp `TurnOn()` / `TurnOff()`.
2. **MenuTab** — quản lý highlight tab active/inactive, đổi màu text và background sprite.
3. **Slot UI** — mỗi slot inventory/equipment là một button + image + text count, kết nối với data Slot bên dưới.

---

## 18. Save / Load System

### SaveLoadManager (`Script/DataSave/SaveLoadManager.cs`)

```
SaveGame()
├── Tạo GameSaveData mới
├── Collect data từ mọi hệ thống:
│   ├── playerData = PlayerManager.GetSavePlayerData()
│   ├── equipmentSaveData = EquipMentSystem.GetEquipmentSaveData()
│   ├── inventorySaveData = InventorySystem.GetSaveInventoryData()
│   ├── questData = QuestManager.GetCompletedQuestIDs() + GetQuestProgressSaveData() + GetNpcSaveData()
│   ├── worldData = WorldManager.GetWorldSaveData()
│   └── eventSaveData = EventManager.GetEventSaveData()
├── JsonUtility.ToJson(dataToSave, prettyPrint)
└── File.WriteAllText(persistentDataPath/MyGameSave.json)

LoadGame()
├── File.ReadAllText → JsonUtility.FromJson<GameSaveData>
├── Lấy SceneData từ worldData.idSceneData
├── gameSaveData = loadedData (lưu tạm)
├── SceneLoader.LoadScene(sceneData, savedPos)
│   → Khi scene loaded xong → LoadDataRemain()
│
LoadDataRemain()
├── PlayerManager.LoadPlayerData()
├── InventorySystem.LoadInventoryData()
├── EquipMentSystem.LoadEquipmentSaveData()
├── QuestManager.LoadQuestData()
├── EventManager.LoadEventSaveData()
└── WorldManager.LoadWorldSaveData()
    └── Fire OnLoadDataObject/OnLoadDataBoss → restore chest, boss, object state
```

### Cấu trúc GameSaveData

```
GameSaveData
├── PlayerData
│   └── HP, level, exp, point, stat points, gold, position, bullets
├── InventorySaveData
│   └── List<ItemSaveData> { itemId, count }
├── EquipmentSaveData
│   └── List<ItemSaveData> { itemId, count }
├── QuestData
│   ├── List<int> completedQuestId
│   ├── List<QuestProgressSaveData> { questId, List<int> curCount }
│   └── List<NpcSaveData> { npcId, NpcDataValue }
├── WorldData
│   ├── List<string> chestOpenedId
│   ├── List<int> defeatedBossId
│   ├── List<string> activatedObjectId
│   ├── float timeSaveData
│   └── int idSceneData
└── EventSaveData
    └── List<string> eventSaveDatas (tên sự kiện đã xảy ra)
```

**Lưu ý:** LoadGame chia 2 phase —
1. **Phase 1** (trong `LoadGame()`): xác định scene → gọi LoadScene.
2. **Phase 2** (trong `LoadDataRemain()`): sau khi scene loaded → restore toàn bộ data. Vì Awake/Start của object trong scene cần chạy trước thì mới có object để restore.

---

## 19. Audio System

### AudioManager (`Script/AudioManager.cs`)

Singleton DontDestroyOnLoad, giữ 3 AudioSource:
- `bgmSource` — nhạc nền (loop)
- `sfxSource` — sound effect (one shot)
- `ambientSource` — âm thanh môi trường (mưa, loop)

**Cách gọi:** `AudioManager.Instance.PlayBGMForest()`, `AudioManager.Instance.PlayPunch()`, vv.

**BGM chuyển theo scene:**
- MenuScene → `PlayBGMMainMenu()`
- Outdoor scene → `PlayBGMForest()`
- Indoor scene → `PlayBGMInDoor()`
- Boss fight → `PlayBossCombat()`
- Chuyển BGM luôn `StopBGM()` trước rồi play mới.

---

## 20. Sơ đồ tham chiếu giữa các hệ thống

```
                         ┌─────────────────┐
                         │  GameManageMent  │ (Singleton, DontDestroyOnLoad)
                         │    .Instance     │
                         └────────┬────────┘
           ┌──────────┬──────────┼──────────┬──────────┬──────────┐
           ▼          ▼          ▼          ▼          ▼          ▼
    PlayerManager  PoolManager  QuestMgr  TimeMgr  EnvMgr    BuildMgr
           │          │          │          │        │          │
           │          │          │          │        │          │
    ┌──────┴──────┐   │    ┌────┴────┐     │   ┌────┴────┐     │
    │PlayerCtrl   │   │    │QuestDef │     │   │Indoor   │     │
    │ExpSystem    │   │    │QuestProg│     │   │Outdoor  │     │
    │StatPlayer   │   │    │NpcData  │     │   │Weather  │     │
    │Health       │   │    └─────────┘     │   └─────────┘     │
    │GoldPlayer   │   │                    │                    │
    │SlotPlayerCtrl│  │         ┌──────────┘              BuildPlacement
    └──────┬──────┘   │         ▼                         GhostPreview
           │          │    OutdoorEnv
           │          │    lắng nghe ChangeToDay/Night...
           │          │
           │     ┌────┴──────────────────────┐
           │     │ObjectPool<LootItem>       │
           │     │ObjectPool<FloatingText>   │
           │     │ObjectPool<BulletCtrl>[]   │
           │     │ObjectPool<EnemyBase>[]    │
           │     │ObjectPool<GhostSprite>    │
           │     └───────────────────────────┘
           │
    ┌──────┴────────────────────────────────────────┐
    │                UIManageMent.Instance            │
    │  ┌─────────┬──────────┬──────────┬───────────┐ │
    │  │InventUI │EquipUI  │QuestUI  │DialogueUI │ │
    │  │ShopSys  │BulletUI │CraftUI  │LoadingUI  │ │
    │  └─────────┴──────────┴──────────┴───────────┘ │
    └────────────────────────────────────────────────┘

    InventoryAndEquipmentManager
    ├── InventorySystem (C#) ──event──► QuestManager.UpdateCollect
    └── EquipMentSystem (C#) ──event──► EquipmentSystemUI.Refresh

    SceneLoader.Instance
    ├── LoadScene → loading UI → set environment → camera bound → BGM
    └── SceneData (SO) → environment type, light, parent scene

    SaveLoadManager.Instance
    ├── SaveGame → collect all → JSON → file
    └── LoadGame → file → JSON → LoadScene → LoadDataRemain

    EventManager.Instance() (plain C# Singleton)
    ├── SenderEvent.SendEvent() → OnSignalSent
    └── ReceiverEvent ← lắng nghe OnSignalSent → CheckCondition

    WorldManager (plain C# class, held by GameManageMent)
    ├── chestOpenedId, defeatedBossId, activatedObjectId
    └── OnLoadDataObject → Chest/SenderEvent.Restore()
```

---

## Tóm tắt nhanh: "File nào làm gì?"

| File | Vai trò |
|---|---|
| `GameManageMent.cs` | God singleton, giữ ref mọi hệ thống, pause/continue, menu |
| `UIManageMent.cs` | Singleton UI, HP/EXP bar, warning, popup queue |
| `GameConfig.cs` | Hằng số tag/layer/anim parameter |
| `PlayerController.cs` | Input, di chuyển, tấn công, tương tác NPC |
| `PlayerManager.cs` | Trung gian truy cập player component, tính crit, save/load |
| `SlotPlayerController.cs` | Equip slot 1-5, switch weapon/buildable |
| `StatPlayer.cs` | Stat nâng cấp (HP, ATK, CritRate) |
| `ExpSystem.cs` | Level, EXP, LevelUp logic |
| `Health.cs` | HP, damage/heal, flash effect, die event |
| `EnemyBase.cs` | Abstract AI: Idle/Chase/Attack, FlowField steering |
| `EnemyMelee.cs` | Enemy cận chiến |
| `EnemyRange.cs` | Enemy tầm xa, spawn bullet |
| `EnemySpawner.cs` | Spawn quái từ pool, gửi event khi clear |
| `GridManagement.cs` | Quản lý grid, trigger update DistanceField + FlowField |
| `GridBuilder.cs` | Tạo lưới từ collider |
| `DistanceField.cs` | BFS tính distance từ player |
| `FlowField.cs` | Tính hướng đi tối ưu cho mỗi ô |
| `Context.cs` | Context Steering data (Interest/Danger 8 hướng) |
| `InventorySystem.cs` | Logic inventory: add, remove, stack |
| `EquipMentSystem.cs` | Logic 5 slot equip |
| `ItemData.cs` | Abstract SO base cho mọi item |
| `GunData.cs / MeleeData.cs` | SO vũ khí, kế thừa WeaponData |
| `Weapon.cs / Gun.cs` | MonoBehaviour vũ khí runtime, bắn/chém/reload |
| `QuestManager.cs` | Quản lý quest active/complete, update progress |
| `QuestDefinition.cs` | SO định nghĩa 1 quest |
| `NPC.cs` | Logic tương tác NPC, dialogue flow, quest giao/nhận |
| `DialogueUI.cs` | UI typewriter, accept/refuse button |
| `SceneLoader.cs` | Load/unload scene, loading UI, set environment |
| `SceneData.cs` | SO cấu hình mỗi scene |
| `TimeManager.cs` | Chu kỳ ngày đêm |
| `WeatherSystem.cs` | Random thời tiết theo weight |
| `EnviromentManager.cs` | Switch Indoor/Outdoor |
| `EventManager.cs` | Event bus cho map puzzle |
| `SenderEvent.cs / ReceiverEvent.cs` | Gửi/nhận tín hiệu event |
| `Chest.cs` | Rương: mở → drop loot → save |
| `DropSystem.cs` | Drop item từ loot table |
| `ObjectPool.cs` | Generic object pool |
| `PoolManager.cs` | Quản lý tất cả pool |
| `BuildManager.cs / BuildPlacement.cs` | Chế độ xây dựng |
| `SaveLoadManager.cs` | Save/Load JSON |
| `GameSaveData.cs` | Root save data class |
| `AudioManager.cs` | BGM, SFX, ambient |

---

*Tài liệu cập nhật: Tháng 3/2026*
