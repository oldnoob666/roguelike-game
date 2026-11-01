using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerStatsIntegrationTests
{
    private GameObject player;
    private Scene combatScene;

    [UnityTearDown]
    public IEnumerator Cleanup()
    {
        if (player != null) Object.DestroyImmediate(player);
        if (combatScene.IsValid() && combatScene.isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.CreateScene("EmptyStatsTestScene"));
            yield return SceneManager.UnloadSceneAsync(combatScene);
        }
    }

    [Test]
    public void PlayerHealthUsesStatsAtSpawnAndReadsNewMaximum()
    {
        player = new GameObject("StatsPlayer");
        player.SetActive(false);
        var stats = player.AddComponent<PlayerStats>();
        stats.playerBaseHealth = 150;
        var health = player.AddComponent<CharacterHealth>();
        player.SetActive(true);

        Assert.AreEqual(150, health.CurrentHealth);
        Assert.AreEqual(150, health.MaxHealth);
        health.TakeDamage(10, Vector2.right);
        stats.AddHealth(20);
        Assert.AreEqual(170, health.MaxHealth);
        Assert.AreEqual(140, health.CurrentHealth, "Maximum health bonuses do not heal the player.");
    }

    [Test]
    public void PlayerInvulnerabilityUsesStatsIncludingBonuses()
    {
        player = new GameObject("StatsPlayer");
        var stats = player.AddComponent<PlayerStats>();
        stats.playerBaseInvulnerableDuration = 0.5f;
        stats.AddInvulnerableDuration(-0.5f);
        var health = player.AddComponent<CharacterHealth>();

        Assert.IsTrue(health.TakeDamage(10, Vector2.right));
        Assert.IsTrue(health.TakeDamage(10, Vector2.right));
        Assert.AreEqual(80, health.CurrentHealth);
    }

    [Test]
    public void PlayerMovementReadsSpeedBonusesWithoutRestarting()
    {
        player = new GameObject("StatsPlayer");
        var stats = player.AddComponent<PlayerStats>();
        stats.playerBaseSpeed = 7f;
        var body = player.AddComponent<Rigidbody2D>();
        var movement = player.AddComponent<Player>();
        movement.rb = body;
        movement.InputDir = Vector2.right;
        movement.SendMessage("Move");
        Assert.AreEqual(new Vector2(7f, 0f), body.velocity);

        stats.AddSpeed(3f);
        movement.SendMessage("Move");
        Assert.AreEqual(new Vector2(10f, 0f), body.velocity);
    }

    [UnityTest]
    public IEnumerator ActualWeaponUsesPlayerDamageAndFireCooldown()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");
        combatScene = SceneManager.GetActiveScene();
        var scenePlayer = Object.FindObjectOfType<Player>();
        var stats = scenePlayer.GetComponent<PlayerStats>();
        Assert.IsNotNull(stats, "The scene player must have PlayerStats attached.");
        scenePlayer.GetComponent<SimplePlayerShooting>().enabled = false;
        stats.playerBaseTakeDamage = 20;
        stats.AddDamage(15);
        stats.fireBaseCooldown = 1f;
        stats.AddFireBaseCooldown(-0.5f);
        foreach (var other in Object.FindObjectsOfType<EnemyController>())
            other.enabled = false;
        var enemy = GameObject.Find("Enemy_Boar_2").GetComponent<EnemyController>();
        enemy.transform.position = new Vector2(5f, 0f);
        Physics2D.SyncTransforms();
        var weapon = scenePlayer.GetComponent<SimpleWeaponManager>();
        weapon.AimAt(enemy.GetComponent<Collider2D>().bounds.center);

        Assert.IsTrue(weapon.TryFire());
        stats.AddDamage(10); // Already fired bullets retain their damage at firing time.
        yield return new WaitForSeconds(0.3f);
        Assert.AreEqual(65, enemy.GetComponent<CharacterHealth>().CurrentHealth);
        Assert.IsFalse(weapon.TryFire());
        yield return new WaitForSeconds(0.3f);
        Assert.IsTrue(weapon.TryFire());

    }
}
