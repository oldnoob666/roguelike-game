using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// 菜单用于重新生成接入结果；不会在打开项目时自动修改场景。
public static class EnemyIntegrationSetup
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";
    private const string PrefabPath = "Assets/Prefab/Enemy_Boar.prefab";
    private const string ArtPath = "Assets/Art/Enemies/Boar/";

    [MenuItem("Tools/Enemies/Setup Sample Scene")]
    public static void Setup()
    {
        if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        EditorSceneManager.OpenScene(ScenePath);
        var player = UnityEngine.Object.FindObjectOfType<Player>();
        if (player == null) throw new InvalidOperationException("SampleScene has no Player.");
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null) playerHealth = player.gameObject.AddComponent<PlayerHealth>();
        var collider = player.GetComponent<CapsuleCollider2D>();
        if (collider == null) collider = player.gameObject.AddComponent<CapsuleCollider2D>();
        collider.size = new Vector2(5.5f, 10f);
        player.rb.freezeRotation = true;

        var prototype = new GameObject("Enemy_Boar");
        prototype.transform.localScale = Vector3.one * 1.6f;
        var renderer = prototype.AddComponent<SpriteRenderer>();
        renderer.sprite = AssetDatabase.LoadAllAssetsAtPath(ArtPath + "Sprites/Idle/Idle-Sheet.png")
            .OfType<Sprite>().OrderBy(sprite => sprite.name).First();
        renderer.sortingLayerName = "Player";
        renderer.sortingOrder = 0;
        var animator = prototype.AddComponent<Animator>();
        animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
            ArtPath + "Animations/Enemy_boar.controller");
        var body = prototype.AddComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.freezeRotation = true;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        var hitbox = prototype.AddComponent<CapsuleCollider2D>();
        hitbox.isTrigger = true;
        hitbox.direction = CapsuleDirection2D.Horizontal;
        hitbox.size = new Vector2(1.7f, 1.05f);
        hitbox.offset = new Vector2(-0.1f, 0.65f);
        prototype.AddComponent<EnemyController>();
        var healthSettings = new SerializedObject(prototype.GetComponent<CharacterHealth>());
        healthSettings.FindProperty("invulnerableDuration").floatValue = 0.12f;
        healthSettings.ApplyModifiedPropertiesWithoutUndo();
        var prefab = PrefabUtility.SaveAsPrefabAsset(prototype, PrefabPath);
        UnityEngine.Object.DestroyImmediate(prototype);

        var positions = new[] { new Vector2(-5, -3), new Vector2(5, 1), new Vector2(0, 6) };
        for (int index = 0; index < positions.Length; index++)
        {
            string objectName = "Enemy_Boar_" + (index + 1);
            var instance = GameObject.Find(objectName);
            if (instance == null) instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = objectName;
            instance.transform.position = positions[index];
            var settings = new SerializedObject(instance.GetComponent<EnemyController>());
            settings.FindProperty("target").objectReferenceValue = playerHealth;
            settings.ApplyModifiedPropertiesWithoutUndo();
        }

        // 原场景里有一颗手工放置的子弹，只作为摆放参考，不应该在游戏中挡枪。
        foreach (var bullet in UnityEngine.Object.FindObjectsOfType<SimpleBullet>())
            bullet.gameObject.SetActive(false);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        var scenes = EditorBuildSettings.scenes.ToList();
        int existing = scenes.FindIndex(scene => scene.path == ScenePath);
        if (existing >= 0) scenes[existing] = new EditorBuildSettingsScene(ScenePath, true);
        else scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
        ValidateScene();
        Debug.Log("Enemy integration setup and scene validation passed.");
    }

    public static void ValidateScene()
    {
        var player = UnityEngine.Object.FindObjectOfType<Player>();
        if (player == null || player.GetComponent<PlayerHealth>() == null || player.GetComponent<Collider2D>() == null)
            throw new InvalidOperationException("Player combat components are missing.");
        var enemies = UnityEngine.Object.FindObjectsOfType<EnemyController>();
        if (enemies.Length != 3) throw new InvalidOperationException("Expected three scene enemies.");
        foreach (var enemy in enemies)
        {
            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(enemy.gameObject) != 0)
                throw new InvalidOperationException("Missing enemy script.");
            var animator = enemy.GetComponent<Animator>();
            if (animator == null || animator.runtimeAnimatorController == null)
                throw new InvalidOperationException("Enemy animator is missing.");
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
                    if (AnimationUtility.GetObjectReferenceCurve(clip, binding).Any(frame => frame.value == null))
                        throw new InvalidOperationException("Missing animation sprite: " + clip.name);
            }
            var settings = new SerializedObject(enemy);
            if (settings.FindProperty("target").objectReferenceValue == null)
                throw new InvalidOperationException("Enemy target is missing.");
        }
    }

    // 可在有图形设备的批处理模式下输出场景预览。
    public static void CapturePreview()
    {
        EditorSceneManager.OpenScene(ScenePath);
        ValidateScene();
        var camera = Camera.main;
        var texture = new RenderTexture(1280, 720, 24);
        camera.targetTexture = texture;
        camera.Render();
        RenderTexture.active = texture;
        var image = new Texture2D(1280, 720, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
        image.Apply();
        Directory.CreateDirectory("Logs");
        File.WriteAllBytes("Logs/enemy-scene-preview.png", image.EncodeToPNG());
        camera.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.DestroyImmediate(image);
        texture.Release();
        UnityEngine.Object.DestroyImmediate(texture);
    }
}
