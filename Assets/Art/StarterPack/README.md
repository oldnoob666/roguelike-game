# 基础美术资源包

这是一套用于当前 2D 鼠标瞄准射击原型的静态 PNG 素材，采用卡通手绘风格、青绿与土色配色。生成方式：Codex 内置 image_gen。所有生成图片均复制到本目录，使用时不依赖生成缓存目录。

本次只新增六张 PNG 和本说明文件，没有新增或修改 C# 脚本，也没有修改场景。尚未在 Unity 中完成挂载或 Play Mode 验证。

## 文件清单

| 文件 | 实际尺寸 | 说明 |
|---|---|---|
| Backgrounds/Arena_Ground.png | 1536 × 1024 | 不透明的苔藓石板竞技场地面；中心保持较空；未验证无缝平铺 |
| Characters/Player_Idle.png | 1254 × 1254 | 透明背景、空手站立人物，单帧，无行走动画 |
| Environment/Crate_Wood.png | 1254 × 1254 | 透明背景木箱装饰，可重复摆放 |
| Environment/Rock_Moss.png | 1434 × 1097 | 透明背景苔藓岩石装饰，可重复摆放 |
| Weapons/Pistol_Right.png | 1536 × 1024 | 透明背景枪械，枪口水平朝右，与现有 AimAt 的零度方向一致 |
| Projectiles/Bullet_Gold.png | 1254 × 1254 | 透明背景圆形金色光弹，可沿任意方向移动，无需旋转图像 |

人物、环境、枪械、子弹均已检查存在有效 Alpha 透明通道。背景为 RGB 图片。素材包含透明留白，画布尺寸不等于物体实际占用尺寸。

## Unity 导入建议

以下是需要在编辑器中设置的建议值，本次没有修改 TextureImporter 配置。

1. 在 Project 窗口中选择 PNG，将 Texture Type 设置为 Sprite (2D and UI)，Sprite Mode 选择 Single。
2. Filter Mode 建议 Bilinear，Wrap Mode 建议 Clamp；透明素材启用 Alpha Is Transparency。
3. 为保留源图细节，Max Size 可设为 2048，Compression 可先用 None。确认画面后再按平台要求压缩。
4. 不同图片的分辨率和透明留白不同，建议分别设置 Pixels Per Unit，避免全部按同一个数值导入后尺寸失衡。

可使用以下数值作为起点，再按实际场景调整：

| 素材 | Pixels Per Unit 起点 | Sorting Order 示例 |
|---|---:|---:|
| 地面背景 | 100 | -10 |
| 人物 | 1000 | 2 |
| 木箱 | 950 | 0 |
| 岩石 | 1000 | 0 |
| 枪械 | 2000 | 3 |
| 光弹 | 2000 | 4 |

这些 Sorting Order 只是静态摆放示例，没有实现人物与环境按 Y 坐标动态遮挡。

当前摄像机 Orthographic Size 为 5。在 16:9 的 Game 视图中，背景按 PPU=100 导入后，可从 Scale=(1.2,1,1) 开始调整以覆盖画面。其他视图比例请按实际覆盖情况调整；本图不是经过验证的无缝瓦片。

## 与现有四个脚本配合

- 人物图像可以放在玩家对象的 SpriteRenderer 上。SimplePlayerShooting 和 SimpleWeaponManager 仍需挂在同一对象上。
- 枪械图像可放在独立武器对象上，由现有 SimpleWeaponController 旋转。枪械初始朝右，不需要额外补偿 90 度。
- 枪口可使用武器下的空子对象 MuzzlePoint，移动到图像右侧枪口处，并赋给现有 muzzlePoint 字段。
- 若希望枪围绕握柄旋转，可在 Sprite Editor 中把枪械 Pivot 调整到握柄附近，然后重新摆放枪口。
- 光弹可放在带 SimpleBullet 的对象上，再保存成预制体并赋给现有 bulletPrefab 字段。圆形光弹适合当前以 Quaternion.identity 生成、仅修改移动方向的逻辑。
- 木箱与岩石目前只是图片。是否添加 Collider2D、是否阻挡玩家或子弹，需要后续另行配置；这些图片不会自行增加碰撞或伤害功能。

## 生成提示词

以下保留本套素材使用的英文提示词，便于后续延续风格。前景素材生成时均设置 transparent_background=true，背景为 false。

### Backgrounds/Arena_Ground.png

Use case: stylized-concept. Asset type: standalone 2D Unity roguelike arena background texture, landscape 1536x1024. Primary request: a usable empty combat arena floor for a small mouse-aim shooter. Orthographic overhead camera, flat XY playfield, NO horizon or perspective vanishing point. Unified art direction: clean chunky hand-painted cartoon game art, restrained muted moss-green ground, slate grey flagstones, warm ochre dirt, subtle dark outlines, soft upper-left lighting. Design a broad mostly clear softly textured green-grey stone courtyard floor with sparse worn paving seams and tiny grass tufts; low contrast center so characters and bullets read clearly. The floor extends to every edge with no border frame, no enclosing wall. Texture only: no characters, no guns, no projectiles, no large rocks, no crates, no trees, no text, no UI, no logos, no watermark. This is a gameplay background image, not a screenshot or concept presentation. Opaque background.

### Characters/Player_Idle.png

Use case: stylized-concept. Asset type: one standalone transparent PNG player character sprite for a Unity 2D top-down roguelike shooter, square canvas. A compact human adventurer wearing a muted teal hood and short teal jacket, tan undershirt, brown belt, charcoal trousers, chunky brown boots. Clean chunky hand-painted cartoon game art, restrained earthy colors, dark crisp outlines, soft upper-left lighting, matching a moss-green slate-stone courtyard. Camera: conventional top-down three-quarter dungeon-game sprite, see top of hood and front of face, facing screen-down, FULL BODY standing neutral with two feet visible. Calm idle pose, arms close to torso and empty hands; firearm will be a SEPARATE Unity sprite. Large readable head, small simple face. Exactly one character centered filling 80 percent of canvas height, transparent margin. No weapons, no scenery, no ground plane, no cast ground shadow, no text, no UI, no frame, no sprite sheet, no multiple views. Genuine transparent alpha background.

### Weapons/Pistol_Right.png

Use case: stylized-concept. Asset type: one isolated transparent PNG gun sprite for Unity 2D mouse-aim roguelike, landscape canvas. Exactly one compact fantasy adventurer's pistol, clean strictly SIDE PROFILE, barrel pointing exactly horizontally RIGHT (+X), grip points down. Practical short chunky silhouette, slate gunmetal barrel and action, warm brown wooden grip, tiny muted teal accent, a few aged brass fittings, no markings. Clean hand-painted cartoon game art with crisp dark outlines, restrained earthy palette, soft upper-left lighting; match a teal-hooded ranger and mossy grey stone courtyard. Entire gun fully visible, centered with 12 percent transparent padding, long axis horizontal; useful readable silhouette at small gameplay scale, flat orthographic profile with no perspective foreshortening. This sprite rotates in XY around Z so the bore must be horizontal. No hand, no character, no ammo, no projectile, no muzzle flash, no floor, no cast shadow, no diagram, no text or letters, no logos, no UI, no border, no multiple views. Genuine transparent alpha background.

### Projectiles/Bullet_Gold.png

Use case: stylized-concept. Asset type: ONE isolated transparent PNG projectile sprite for Unity 2D top-down roguelike. A small perfectly ROUND warm golden magical bullet / energy pellet, centered on square canvas. Circular cream-white luminous core, compact amber-gold rim, soft short radial golden glow fading completely into transparent alpha; restrained clean hand-painted cartoon game art to match a teal ranger and brass-trimmed wooden pistol. Radially balanced so it works moving in ANY direction without rotation, NO directional tail, NO motion streak, no arrow tip, no sparks flying off, no elaborate spell ring, no extra orbs. Clear round silhouette at tiny gameplay sizes. Core fills around 35 percent canvas width, all glow contained inside central 65 percent with wide fully transparent outer margin. No floor, no scenery, no dark rectangle or black background, no cast shadow, no text, no labels, no UI, no frame. Genuine transparent alpha background; all non-projectile pixels have alpha zero.

### Environment/Crate_Wood.png

Use case: stylized-concept. Asset type: one standalone transparent PNG environment prop sprite for a Unity 2D top-down roguelike shooter. Exactly ONE small square wooden supply crate, closed lid, warm weathered brown planks, sturdy diagonal wood brace on front, dark iron corner fasteners, restrained touches of moss on lower edge. Conventional top-down three-quarter orthographic dungeon-game view, top face and front face visible, no perspective vanishing point. Clean chunky hand-painted cartoon game art, crisp dark outlines, simple readable planes, earthy colors, soft upper-left lighting, matching a teal-hooded ranger and muted moss-green slate-stone courtyard. Fully visible centered crate filling 70 percent canvas, square image with clean transparent margin. No other objects, no weapon, no labels, no text, no floor or scenery, no pedestal, no cast ground shadow, no frame, no multiple views, no sprite sheet. Genuine transparent alpha background, outside object fully transparent.

### Environment/Rock_Moss.png

Use case: stylized-concept. Asset type: one standalone transparent PNG environment prop sprite for Unity 2D top-down roguelike shooter. Exactly ONE low chunky irregular slate-grey boulder with angular broad facets, slightly rounded weathered edges, a modest patch of muted moss-green on its upper left surface. Conventional top-down three-quarter orthographic dungeon-game view, top surface prominently visible and short front face. Clean hand-painted cartoon game art, crisp dark outlines, simple readable silhouette, restrained grey and earthy green palette, soft upper-left lighting, matching mossy grey flagstone courtyard, teal-hooded adventurer, and warm wooden crate. Entire rock centered filling 75 percent canvas width, comfortable transparent margin. No separate loose stones, no grass clump extending beyond base, no gems, no eyes, no face, no floor patch, no cast ground shadow, no scenery, no text, no labels, no UI, no frame, no multiple views or sprite sheet. Genuine transparent alpha background, outside rock fully transparent.

