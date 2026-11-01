# 五属性药瓶

五张透明 PNG 使用 Codex 内置 image_gen 生成，以现有 Player_Idle、Crate_Wood、Pistol_Right 为画风参考，再以生命瓶统一其余四瓶。沿用手绘奇幻卡通、深色描边、皮革、旧黄铜和左上方光照。

| 图片 / 同名预制体 | 颜色与徽章 | PlayerStats 属性 | PotionType |
|---|---|---|---|
| Potion_HealthUp | 红色、心形 | playerHealth（生命上限） | HealthUp = 0 |
| Potion_SpeedUp | 绿色、羽翼 | playerSpeed（移动速度） | SpeedUp = 1 |
| Potion_DamageUp | 橙色、剑 | playerTakeDamage（攻击伤害） | DamageUp = 2 |
| Potion_InvulnerableDurationUp | 蓝色、盾牌 | playerinvulnerableDuration（受伤后无敌时长） | InvulnerableDurationUp = 3 |
| Potion_FireCoolDownUp | 紫色、沙漏 | fireCooldown（开火冷却间隔） | FireCoolDownUp = 4 |

## 在 Unity 中使用

预制体位于 Assets/Prefab/Potions/，可以直接拖进场景。每个预制体已经配置 SpriteRenderer 和现有 Potion 组件，以及对应的 potionType。显示使用 Player 排序层、Order in Layer = 0。

图片均为 1254 × 1254 RGBA；Sprite (2D and UI)、Single、中心 Pivot、1400 Pixels Per Unit、Bilinear、Clamp、不压缩、关闭 Mip Maps、启用 Alpha Is Transparency。预制体 Scale = (1, 1, 1)，瓶体可见高度约 0.8 个世界单位，可按场景需要缩放。

本次提供美术与可摆放预制体，没有添加碰撞体或自动拾取行为。现有 Potion.UsePotion() 的 switch 仍为空，因此摆放药瓶不会自动修改数值。生命瓶表达生命上限属性，不等同于已实现的即时回血。

开火冷却是时间间隔，数值越小开火越快；后续若实现提高射速的药水，应减少该间隔。为了匹配现有代码，文件名和枚举保留 FireCoolDownUp。

完整生成提示词保存在同目录的 GENERATION_PROMPTS.md；所有图片均已复制至项目，不依赖生成缓存。

## 导入验证

已在 Unity 2022.3.59f1c1 批处理实际导入并检查：5/5 Sprite 加载成功，5/5 预制体实例化成功，贴图、材质、排序层、Potion 类型和导入设置均通过，无丢失脚本。验证日志位于 Library/PotionAssetImport.log，结果摘要位于 Library/PotionAssetValidation.txt。临时校验脚本已移除，没有修改现有玩法脚本或场景。
