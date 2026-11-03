# 子弹袋素材

- `Ammo_Restore.png`：恢复当前子弹数量。青绿色束口袋、三颗子弹、环形补充箭头。
- `Ammo_CapacityUp.png`：增加子弹数量上限。紫色宽型双层袋、五颗子弹、金色向上箭头。

两者采用现有角色、手枪和药水的手绘奇幻风格：深色描边、棕色皮革、黄铜配件和分面笔触。PNG 保留生成的透明通道，未缩放、裁切或重绘。每张为 1254 × 1254，Sprite (2D and UI) / Single / 1400 Pixels Per Unit / Bilinear / 无 mipmap / 不压缩，导入参数沿用现有恢复生命药水。

此次仅添加美术素材与导入配置，没有创建拾取逻辑、预制体或更改场景。

## 生成方式与原始提示词

使用内置 image_gen，透明背景；每种道具分别生成。第二张同时参考第一张以保持配套风格。

### 恢复子弹

```text
Use case: stylized-concept. Asset type: a single transparent 2D pickup sprite for a hand-painted fantasy roguelike Unity game. Input images are STYLE REFERENCES ONLY: match the existing project's teal-hooded adventurer, brown leather straps, brass flintlock pistol and ornate potion icons. Draw ONE AMMUNITION REFILL POUCH. A small squat rounded soft teal canvas drawstring ammunition bag, chocolate brown leather reinforced bottom and rim, a short tan cord tied at the neck. Three clearly visible chunky gold brass cartridges with rounded projectile tips poke out of the open mouth. On the FRONT is a very large simple ivory/gold circular refill-arrow emblem, readable when reduced to 64px; this must look like ammunition replenishment and NOT health. Two little brass rivets, broad simple forms, limited detail. Compact round silhouette, teal is the dominant fabric color. Three-quarter FRONT view from slightly above, single centered isolated item, fills about 78% of a square canvas with clear transparent margins on all sides. Match painterly faceted brush texture, strong dark brown ink outline, warm top-left highlights, deep warm shadow, charming high quality fantasy inventory art of supplied references. Coherent materials rather than photorealism. ACTUAL transparent alpha background, clean edges. No floor, no scene, no drop shadow outside the object, no vignette, no glow haze, no frame, no text or letters, no health cross, no extra objects, no other bags. This is the refill member of a pair, the later capacity upgrade member will have a wider purple reinforced silhouette and up-arrow instead.
```

### 增加子弹上限

```text
Use case: stylized-concept. Asset type: single transparent 2D pickup sprite for a hand-painted fantasy roguelike Unity game. Input image 1 is the style reference AND companion AMMO REFILL pouch: match its exact faceted painted texture, dark warm brown outlines, upper left lighting, chunky metallic bullets, aged brown leather and gold brass hardware. Input images 2 and 3 are additional style references from the game. Create a NEW distinct companion item: AMMUNITION CAPACITY UPGRADE BAG, communicates increasing maximum ammo capacity. Draw ONE WIDE rectangular expandable satchel ammunition pouch, stout plum-purple canvas body with TWO visibly stacked expanding compartments/gusset tiers, broad brown leather belt straps, chunky brass reinforced corner caps, brass rivets, neat small top handle/loop. Five brass cartridges with rounded gray projectile tips peek out of the upper opening in a short row. Front face features ONE VERY LARGE polished gold UPWARD ARROW EMBLEM with short thick stem and wide simple triangle arrowhead, extremely readable at 64px. Arrow occupies the central purple front panel; no letters. Purple fabric dominates, gold edge reinforces an upgraded item. Its silhouette is distinctly WIDER and BOXIER than the round teal drawstring reference, with visibly expanded double-compartment construction, same game item family. View from slightly above and mostly front, mild three-quarter perspective. Center isolated ONE object filling about 80% of square canvas, generous clear margins, nothing clipped. Artisanal medieval fantasy materials, painterly faceted brushwork and bold dark contours exactly like reference, subtle warm highlights. GENUINE transparent alpha background; crisp cutout. No floor, no backdrop, no cast shadow outside the object, no glow cloud, no border or frame, no words or numbers, no health cross, no circular refill arrows, no scene, no additional bags, no loose external objects. Purple and gold, upward arrow and broad twin-tier silhouette must clearly distinguish this CAPACITY UPGRADE from the teal round REFILL pouch.
```
