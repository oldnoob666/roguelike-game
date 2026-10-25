第一阶段：简易 2D 射击脚本

本包包含此前教程中的四个完整脚本，保留原有引用检查。
仅包含源码，没有修改现有项目，没有预制体或场景。
请勿把本包当成完整 Unity 项目。

放入项目：
把压缩包里的 Assets 文件夹解压到：
D:\unity01\Roguelike new game
最终路径应为：
D:\unity01\Roguelike new game\Assets\SimpleShooting\SimpleBullet.cs
其余三个脚本与它在同一文件夹。

导入前：如果项目里已有同名类，请先合并或移除重复代码，避免重复定义。

场景配置：
1. 创建 Player，位置 (0, 0, 0)，添加 SimplePlayerShooting 和 SimpleWeaponManager。
2. 在 Player 下创建空物体 WeaponPivot，本地位置 (0, 0, 0)，添加 SimpleWeaponController。
3. 在 WeaponPivot 下创建 GunVisual（正方形精灵）和 MuzzlePoint（空物体）。
   GunVisual 本地位置 (0.45, 0, 0)，缩放 (0.6, 0.16, 1)，精灵层级排序为 1。
   MuzzlePoint 本地位置 (0.85, 0, 0)。
4. 创建 Bullet（圆形精灵），缩放 (0.15, 0.15, 1)，添加 SimpleBullet，
   精灵层级排序为 2；把它拖到 Project（项目）面板制作预制体，然后删除场景中的 Bullet。
5. 将 WeaponPivot 拖入 Player 的 Current Weapon 字段。
   将 Bullet 预制体和 MuzzlePoint 分别拖入武器的 Bullet Prefab、Muzzle Point 字段。
6. 摄像机使用 Orthographic（正交）模式，Tag（标签）为 MainCamera，
   位置 (0, 0, -10)，正对 XY 平面。
7. 本代码使用旧输入接口。若项目只启用了新版输入系统，需要在
   Project Settings > Player > Other Settings > Active Input Handling
   启用 Both 或 Input Manager (Old)。
8. 在 Game（游戏）窗口移动鼠标瞄准，按住左键连续发射。

功能范围：瞄准、射击冷却、子弹生成与移动、超时销毁。
不包含移动、碰撞、伤害和对象池。
未在你的项目或 Unity 编辑器中编译、运行验证。
