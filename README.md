# 🎮 FirstProjectGame — 2D Top-Down RPG

> Game RPG 2D top-down pixel art được phát triển bằng **Unity 6000.3.9f1 (Unity 6)**, sử dụng **Universal Render Pipeline (URP)** và các asset pixel art miễn phí.

---

## 📖 Giới thiệu

**FirstProjectGame** là một tựa game nhập vai hành động 2D top-down lấy bối cảnh post-apocalypse. Người chơi điều khiển nhân vật phiêu lưu qua nhiều bản đồ, chiến đấu với quái vật và boss, nhận nhiệm vụ từ NPC, thu thập vật phẩm, chế tạo trang bị, xây dựng công trình và khám phá thế giới mở với hệ thống thời gian ngày/đêm cùng thời tiết động.

---

## 🌟 Tính năng nổi bật

### ⚔️ Hệ thống chiến đấu đa dạng
- **Cận chiến (Melee):** Đấm tay không và sử dụng vũ khí cận chiến (dùi cui, gậy, v.v.).
- **Tầm xa (Ranged):** Hệ thống súng đa loại — Pistol, Shotgun, Rifle, Energy Gun — mỗi loại có đạn riêng, tốc độ bắn và hiệu ứng khác nhau.
- **Chí mạng (Critical Hit):** Hệ thống tỷ lệ chí mạng và sát thương chí mạng có thể nâng cấp.
- **Equip slot nhanh (1-5):** Chuyển đổi nhanh giữa các vũ khí/vật phẩm qua phím số.

### 🧟 Hệ thống kẻ địch thông minh
- **Enemy AI với Flow Field Pathfinding:** Quái vật sử dụng thuật toán Flow Field + Distance Field trên Grid để di chuyển thông minh, tránh vật cản và bám theo người chơi.
- **Hai loại quái:** Enemy Melee (cận chiến) và Enemy Range (tầm xa) với dữ liệu cấu hình qua ScriptableObject.
- **Enemy Spawner:** Hệ thống spawn quái tự động theo khu vực, kích hoạt sự kiện khi tiêu diệt hết.
- **Object Pool:** Tối ưu hiệu năng bằng hệ thống Object Pooling cho quái, đạn, loot, floating text và hiệu ứng.

### 👑 Boss Fight
- **Nhiều boss độc đáo:** Ghost King, Huy Boss — mỗi boss có cơ chế chiến đấu, skill và hành vi riêng biệt.
- **Boss Stat & Visual:** Hệ thống chỉ số boss, hiệu ứng hình ảnh, kỹ năng đặc biệt (ground zone, turn shoot, v.v.).
- **Sự kiện kích hoạt boss:** Boss được kích hoạt qua hệ thống Event, lưu trạng thái đã đánh bại.

### 🎒 Hệ thống Inventory & Equipment
- **Inventory động:** Kho đồ với kích thước tùy chỉnh, hỗ trợ stack vật phẩm, kéo thả (drag & drop).
- **Equipment System:** Trang bị vật phẩm vào các slot (vũ khí, buildable), thay đổi animation và hành vi nhân vật tương ứng.
- **Nhiều loại Item:** Vũ khí cận chiến, súng, đạn, vật phẩm tiêu hao, vật liệu xây dựng — tất cả quản lý qua `ItemDataBase` (ScriptableObject).

### 🛠️ Hệ thống Crafting
- **Chế tạo vật phẩm:** Ghép nguyên liệu theo công thức (Recipe) để tạo ra trang bị, đạn dược, bandage, tường gỗ, tường gia cố, v.v.
- **Recipe Data:** Dữ liệu công thức dạng ScriptableObject, dễ mở rộng.

### 🏗️ Hệ thống Build (Xây dựng)
- **Chế độ xây dựng:** Bật/tắt Build Mode, đặt công trình với Ghost Preview hiển thị vị trí trước khi đặt.
- **Build Placement trên Grid:** Vật thể được đặt theo lưới, có kiểm tra va chạm.
- **Buildable Object:** Định nghĩa các vật thể xây dựng qua dữ liệu.

### 🗺️ Hệ thống Scene & Map
- **Chuyển cảnh mượt mà:** Hỗ trợ Additive Scene Loading và Single Scene Loading với màn hình loading.
- **Scene Graph:** Đồ thị liên kết các scene cho phép điều hướng đa chiều giữa các bản đồ.
- **Scene Navigation Manager:** Quản lý điều hướng giữa các cảnh dựa trên SceneData (loại map, ánh sáng, môi trường).
- **Teleport Portal:** Cổng dịch chuyển giữa các khu vực.
- **Đa loại map:** Main Map (ngoài trời) và Secondary Map (indoor/dungeon), mỗi loại có cấu hình ánh sáng và môi trường riêng.

### 💬 Hệ thống NPC & Dialogue
- **NPC tương tác:** Tiếp cận NPC và nhấn phím E để nói chuyện, nhận nhiệm vụ hoặc mua bán.
- **Dialogue UI với hiệu ứng gõ chữ:** Chữ hiện dần từng ký tự (typewriter effect), có thể nhấn Space để skip.
- **Avatar NPC:** Hiển thị avatar nhân vật đang nói chuyện.
- **Nhiều nhánh hội thoại:** NPC có nhiều đoạn hội thoại, hỗ trợ nút Accept/Refuse cho nhiệm vụ.

### 📜 Hệ thống Quest
- **Nhiều loại mục tiêu:** Collect (thu thập vật phẩm), Kill (tiêu diệt kẻ địch), TalkToNpc (nói chuyện), ReachArea (đến khu vực).
- **Quest Progress tracking:** Theo dõi tiến độ nhiệm vụ theo từng mục tiêu.
- **Phần thưởng quest:** Vàng, kinh nghiệm và vật phẩm.
- **Arrow Quest:** Mũi tên chỉ hướng dẫn người chơi đến mục tiêu nhiệm vụ.
- **Quest items:** Vật phẩm nhiệm vụ không thể xóa khỏi inventory.

### 🏪 Hệ thống Shop (Cửa hàng)
- **Shopkeeper NPC:** NPC bán hàng với danh sách vật phẩm riêng.
- **Mua bán vật phẩm:** Hệ thống mua (Buy System) với giao diện shop UI.

### 📊 Hệ thống RPG & Stat
- **Hệ thống Level & EXP:** Lên cấp tự động khi đủ EXP, nhận điểm kỹ năng.
- **Stat nâng cấp:** Dùng điểm để tăng HP, ATK, Crit Rate — mỗi stat growth riêng.
- **Hệ thống vàng (Gold):** Dùng để mua sắm, nhận từ quest và tiêu diệt quái.
- **Floating Text:** Hiển thị EXP, sát thương, crit trên nhân vật với hiệu ứng gradient.

### 🌦️ Hệ thống Thời gian & Thời tiết
- **Chu kỳ ngày đêm:** Day → MidDay → Night → MidNight, ảnh hưởng đến ánh sáng và gameplay.
- **Hệ thống thời tiết:** Mưa, sương mù (fog), bình thường — random theo trọng số (weighted random).
- **Hiệu ứng mưa:** Particle system + âm thanh mưa ambient.

### 🏠 Hệ thống Môi trường
- **Indoor / Outdoor:** Chuyển đổi giữa môi trường trong nhà (Indoor) và ngoài trời (Outdoor).
- **Ánh sáng động:** Cường độ và màu ánh sáng thay đổi theo scene và thời gian trong ngày.
- **Parallax scrolling:** Nền cuộn tự động tạo chiều sâu.

### 🗝️ Hệ thống Map Object & Event
- **Chest (Rương):** Mở rương nhận loot, lưu trạng thái (đã mở / chưa mở).
- **Door, CheckPoint, Button Trigger:** Cơ chế puzzle — nhấn nút mở cửa, checkpoint lưu tiến trình.
- **Event System (Sender/Receiver):** Hệ thống sự kiện linh hoạt — SenderEvent gửi tín hiệu, ReceiverEvent nhận và kích hoạt hành động.
- **Destructible Objects:** Vật thể phá hủy được (cây, đá) với hệ thống loot khi phá.
- **Unlocked Statue, Wall Anim:** Các đối tượng interactive với animation.

### 🎨 Loot & Drop
- **Loot Table:** Bảng drop ngẫu nhiên (weighted) cho mỗi loại quái/rương.
- **Loot Item với Object Pool:** Vật phẩm rơi ra thế giới, người chơi loot bằng cách đến gần.

### 💾 Hệ thống Save/Load
- **Lưu toàn bộ tiến trình:** Player data (vị trí, stat, level, EXP, vàng, đạn), Inventory, Equipment, Quest progress, World state (rương đã mở, boss đã hạ, sự kiện đã kích hoạt), thời gian.
- **JSON Save:** Lưu file JSON tại `Application.persistentDataPath`.
- **Load & Resume:** Tải game và quay lại đúng scene, vị trí, trạng thái.

### 🔊 Hệ thống Audio
- **Background Music:** BGM riêng cho Main Menu, Forest, Indoor, Boss Combat.
- **SFX phong phú:** Footstep, các loại súng, đánh cận chiến, hit, heal, UI click, equip, level up, pick up money, die, mission complete.
- **Ambient Sound:** Âm thanh môi trường (mưa).

### 🖥️ UI & UX
- **Menu Controller với Tab system:** Chuyển tab Inventory / Equipment / Stat / Quest / Craft.
- **Setting UI:** Tùy chỉnh cài đặt.
- **Scene Loading UI:** Màn hình chuyển cảnh.
- **Popup thông báo:** Thông báo khi nhận vật phẩm, cảnh báo khi inventory đầy, không đủ nguyên liệu, v.v.
- **Bullet UI:** Hiển thị số đạn hiện tại và tổng đạn.
- **Custom Cursor:** Con trỏ chuột tùy chỉnh, thay đổi khi tương tác.
- **Aspect Ratio Enforce:** Đảm bảo tỷ lệ màn hình nhất quán.

### 📷 Camera
- **Cinemachine Virtual Camera:** Camera theo dõi nhân vật mượt mà với Confiner 2D giới hạn theo bản đồ.

---

## 🛠️ Công nghệ sử dụng

| Thành phần | Chi tiết |
|---|---|
| **Engine** | Unity 6 (6000.3.9f1) |
| **Render Pipeline** | Universal Render Pipeline (URP) |
| **Ngôn ngữ** | C# |
| **Animation** | DOTween, Unity Animator + Blend Tree |
| **UI** | TextMeshPro, Unity UI |
| **Camera** | Cinemachine |
| **2D Tools** | Tilemap, Sprite Shape, 2D Animation, 2D IK, Pixel Perfect |
| **AI Pathfinding** | Custom Flow Field + Distance Field trên Grid |
| **Data** | ScriptableObject cho Item, Enemy, Quest, Recipe, Scene |
| **Save System** | JSON serialization |
| **Object Pooling** | Custom Generic Object Pool |
| **Audio** | Unity AudioSource (BGM, SFX, Ambient) |
| **Art Style** | Pixel Art (Free Adventurer, Pixel Crawler, PostApocalypse Asset Pack) |

---

## 📁 Cấu trúc thư mục chính

```
Assets/
├── Animation/            # Animation clips & controllers
├── Audio/                # Âm thanh (BGM, SFX)
├── Material/             # Materials & Shaders
├── Pallet/               # Tilemap palettes
├── Prefab/               # Prefab nhân vật, quái, UI, vũ khí...
├── Resources/            # Resources load runtime
├── Scenes/               # Unity Scenes (MenuScene, Map1, House)
├── Script/               # Toàn bộ source code
│   ├── Build/            # Hệ thống xây dựng
│   ├── DataSave/         # Save/Load data classes
│   ├── Dialogue/         # Dialogue UI
│   ├── Effect/           # Hiệu ứng (flash, floating text, ghost)
│   ├── Enemy/            # Enemy AI, Boss, Spawner
│   ├── Enviroment/       # Quản lý môi trường & thời tiết
│   ├── EquipMent/        # Hệ thống trang bị
│   ├── Grid/             # Grid, Flow Field, Distance Field
│   ├── Inventory/        # Inventory, Item, Weapon data
│   ├── LootAndDrop/      # Loot table & drop system
│   ├── MapBuild/         # Map objects (Chest, Door, Event...)
│   ├── NPC/              # NPC & Shopkeeper
│   ├── Player/           # Player controller, stat, health
│   ├── Pool/             # Object pooling system
│   ├── Quest/            # Quest system
│   ├── Scene/            # Scene loading & navigation
│   └── UI/               # UI controllers (Menu, Shop, Craft...)
├── Shader/               # Custom shaders
└── Tilemap/              # Tilemap tiles
```

---

## 🎮 Điều khiển

| Phím | Hành động |
|---|---|
| **WASD / Arrow Keys** | Di chuyển |
| **Mouse** | Hướng nhắm / Tương tác |
| **Click chuột trái** | Tấn công / Bắn |
| **E** | Tương tác (NPC, Chest, Portal) |
| **1-5** | Chọn Equipment Slot |
| **Space** | Skip dialogue |
| **ESC / Tab** | Mở Menu |

---

## 🚀 Cách chạy

1. Cài đặt **Unity Hub** và tải **Unity 6000.3.9f1**.
2. Clone hoặc tải project về.
3. Mở project bằng Unity Hub.
4. Mở scene `Assets/Scenes/MenuScene.unity`.
5. Nhấn **Play** để chơi.

---

## 📝 Ghi chú

- Dự án đang trong giai đoạn phát triển.
- Sử dụng các asset pixel art miễn phí: **FREE Adventurer 2D Pixel Art**, **Pixel Crawler Free Pack**, **PostApocalypse Asset Pack**.
- Font chữ: **Press Start 2P** (Google Fonts).

---

## 📄 License

Dự án cá nhân phục vụ mục đích học tập và phát triển kỹ năng lập trình game.
