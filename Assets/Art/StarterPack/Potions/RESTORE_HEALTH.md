# 回血药水（美术素材）

文件：Potion_RestoreHealth.png；预制体：Assets/Prefab/Potions/Potion_RestoreHealth.prefab。

本瓶用于表达恢复当前生命值：粉色药液、象牙色十字徽章、细长圆柱瓶。原生命上限瓶 Potion_HealthUp.png 为红色药液、爱心徽章、宽圆梨形瓶；用颜色、符号和轮廓同时区分。保持同系列手绘描边、旧黄铜、皮革、青色玻璃和左上光照。

图片由 Codex 内置 image_gen 生成，透明 RGBA，Sprite Single，1400 PPU、Bilinear、Clamp、无压缩、无 Mip Maps。预制体沿用当前上限瓶美术子对象的 Scale=(1.5,1.5,1.5)、Player 排序层、Order=0，可直接拖入场景摆放。

目前只完成美术与显示预制体，没有添加拾取/回血行为。当前 PotionType 没有纯回血类型，HealthUp 会增加生命上限并回血，因此新预制体没有挂该组件。后续纯回血功能应调用 CharacterHealth.RestoreHealth(amount)，不调用 PlayerStats.AddHealth(amount)。现有代码、预制体和场景均不因本次新增而修改。

## 生成提示词

Use case: stylized-concept. Asset type: ONE standalone transparent PNG HEALING / RESTORE CURRENT HEALTH potion pickup sprite for the same Unity 2D top-down roguelike. Input image is a STYLE REFERENCE showing an existing MAXIMUM HEALTH UP bottle. Create a new matching healing potion with clear visual distinction from that red heart pear-shaped bottle. Subject: ONE slender upright rounded cylindrical glass vial with curved shoulders and a short neck; noticeably narrower than the reference's broad pear flask, warm brown cork, aged brass neck band, brown leather tie, dark brown leather badge on front. Liquid is distinctly luminous light ROSE PINK with pale coral highlights, visibly lighter and pinker than the reference's red. Badge has one large simple raised IVORY PLUS SIGN / healing cross, equal-length thick arms, NO HEART. Match the reference's hand-painted chunky fantasy cartoon game art, crisp dark brown-black outlines, broad painted facets and highlights, teal-tinted glass, old brass and brown leather material treatment, soft upper-left light, elevated three-quarter orthographic perspective so top of cork and front are visible. Centered full bottle fills 80-85 percent of a square canvas height with comfortable transparent margin. Readable silhouette and plus emblem at small game size. No extra bottles, no heart, no scenery, no ground or cast shadow, no surrounding glow haze, no text/letters/numbers, no labels, no UI/frame, no watermark, no sprite sheet. Genuine fully transparent alpha background outside the bottle.

