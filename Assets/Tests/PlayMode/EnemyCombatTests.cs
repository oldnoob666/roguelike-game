using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyCombatTests
{
    private readonly List<GameObject> objects = new List<GameObject>();

    private GameObject Make(string name, Vector2 position)
    {
        var obj = new GameObject(name);
        obj.transform.position = position;
        objects.Add(obj);
        return obj;
    }

    private EnemyController Enemy(Vector2 position)
    {
        var obj = Make("Enemy", position);
        obj.AddComponent<CircleCollider2D>().isTrigger = true;
        return obj.AddComponent<EnemyController>();
    }

    private PlayerHealth Target(Vector2 position)
    {
        var obj = Make("Target", position);
        obj.AddComponent<Rigidbody2D>().gravityScale = 0f;
        obj.AddComponent<CircleCollider2D>();
        return obj.AddComponent<PlayerHealth>();
    }

    private SimpleBullet Bullet(Vector2 position, Vector2 direction)
    {
        var obj = Make("Bullet", position);
        obj.AddComponent<CircleCollider2D>().radius = 0.1f;
        var bullet = obj.AddComponent<SimpleBullet>();
        bullet.Initialize(direction);
        return bullet;
    }

    [TearDown]
    public void Cleanup()
    {
        foreach (var obj in objects)
            if (obj != null) Object.DestroyImmediate(obj);
        objects.Clear();
    }

    [Test]
    public void DamageHasInvulnerabilityAndDeathOnlyOnce()
    {
        var health = Make("Health", Vector2.zero).AddComponent<CharacterHealth>();
        int deaths = 0;
        health.Died += () => deaths++;
        Assert.IsTrue(health.TakeDamage(20, Vector2.right));
        Assert.AreEqual(80, health.CurrentHealth);
        Assert.IsFalse(health.TakeDamage(20, Vector2.right));
        Assert.IsTrue(health.TakeDamage(1000, Vector2.right, true));
        Assert.IsFalse(health.TakeDamage(1000, Vector2.right, true));
        Assert.AreEqual(0, health.CurrentHealth);
        Assert.AreEqual(1, deaths);
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void NonPositiveDamageDoesNotHealOrHurt(int damage)
    {
        var health = Make("Health", Vector2.zero).AddComponent<CharacterHealth>();
        Assert.IsFalse(health.TakeDamage(damage, Vector2.zero));
        Assert.AreEqual(100, health.CurrentHealth);
    }

    [UnityTest]
    public IEnumerator InvulnerabilityExpires()
    {
        var health = Make("Health", Vector2.zero).AddComponent<CharacterHealth>();
        health.TakeDamage(10, Vector2.right);
        yield return new WaitForSeconds(0.65f);
        Assert.IsTrue(health.TakeDamage(10, Vector2.right));
        Assert.AreEqual(80, health.CurrentHealth);
    }

    [UnityTest]
    public IEnumerator EnemyFindsAndChasesPlayerAboveIt()
    {
        Target(new Vector2(0, 4));
        var enemy = Enemy(Vector2.zero);
        yield return new WaitForSeconds(0.2f);
        Assert.AreEqual(EnemyController.EnemyState.Chase, enemy.CurrentState);
        Assert.Greater(enemy.GetComponent<Rigidbody2D>().velocity.y, 0f);
        Assert.AreEqual(0f, enemy.GetComponent<Rigidbody2D>().velocity.x, 0.01f);
    }

    [UnityTest]
    public IEnumerator EnemyLosesDistantTargetAfterDelay()
    {
        var target = Target(new Vector2(0, 4));
        var enemy = Enemy(Vector2.zero);
        yield return new WaitForSeconds(0.2f);
        target.transform.position = new Vector2(100, 100);
        yield return new WaitForSeconds(0.2f);
        Assert.AreEqual(EnemyController.EnemyState.Chase, enemy.CurrentState);
        yield return new WaitForSeconds(2.2f);
        Assert.AreEqual(EnemyController.EnemyState.Patrol, enemy.CurrentState);
    }

    [UnityTest]
    public IEnumerator HurtKnocksBackVerticallyAndRecovers()
    {
        Target(new Vector2(0, 4));
        var enemy = Enemy(Vector2.zero);
        yield return new WaitForSeconds(0.1f);
        enemy.GetComponent<CharacterHealth>().TakeDamage(10, Vector2.down);
        Assert.AreEqual(EnemyController.EnemyState.Hurt, enemy.CurrentState);
        Assert.Less(enemy.GetComponent<Rigidbody2D>().velocity.y, 0f);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(EnemyController.EnemyState.Chase, enemy.CurrentState);
        Assert.Greater(enemy.GetComponent<Rigidbody2D>().velocity.y, 0f);
    }

    [UnityTest]
    public IEnumerator BulletDamagesEnemyOnceAndDisappears()
    {
        var enemy = Enemy(new Vector2(1, 0));
        enemy.enabled = false;
        var bullet = Bullet(Vector2.zero, Vector2.right);
        yield return new WaitForSeconds(0.2f);
        Assert.AreEqual(80, enemy.GetComponent<CharacterHealth>().CurrentHealth);
        Assert.IsTrue(bullet == null);
    }

    [UnityTest]
    public IEnumerator BulletHitsThinTargetEvenAtHighSpeed()
    {
        var enemy = Enemy(new Vector2(2, 0));
        enemy.enabled = false;
        enemy.GetComponent<CircleCollider2D>().radius = 0.01f;
        var bullet = Bullet(Vector2.zero, Vector2.right);
        // A frame traverses much more than the target's diameter.
        typeof(SimpleBullet).GetField("speed", System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance).SetValue(bullet, 1000f);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(80, enemy.GetComponent<CharacterHealth>().CurrentHealth);
    }

    [UnityTest]
    public IEnumerator WallStopsBulletBeforeEnemy()
    {
        Make("Wall", new Vector2(0.6f, 0)).AddComponent<BoxCollider2D>().size = new Vector2(0.1f, 2);
        var enemy = Enemy(new Vector2(1.5f, 0));
        enemy.enabled = false;
        var bullet = Bullet(Vector2.zero, Vector2.right);
        yield return new WaitForSeconds(0.25f);
        Assert.AreEqual(100, enemy.GetComponent<CharacterHealth>().CurrentHealth);
        Assert.IsTrue(bullet == null);
    }

    [UnityTest]
    public IEnumerator ContactDamageHasCooldown()
    {
        var player = Target(Vector2.zero);
        Enemy(Vector2.zero);
        yield return new WaitForSeconds(0.25f);
        Assert.AreEqual(90, player.GetComponent<CharacterHealth>().CurrentHealth);
    }

    [UnityTest]
    public IEnumerator DeadEnemyCannotMoveOrDamagePlayer()
    {
        var player = Target(Vector2.zero);
        var enemy = Enemy(Vector2.zero);
        enemy.GetComponent<CharacterHealth>().TakeDamage(1000, Vector2.right);
        Assert.AreEqual(EnemyController.EnemyState.Dead, enemy.CurrentState);
        Assert.AreEqual(Vector2.zero, enemy.GetComponent<Rigidbody2D>().velocity);
        Assert.IsFalse(enemy.GetComponent<Collider2D>().enabled);
        yield return new WaitForSeconds(0.15f);
        Assert.AreEqual(100, player.GetComponent<CharacterHealth>().CurrentHealth);
    }

    [UnityTest]
    public IEnumerator PlayerDeathStopsMovementAndShooting()
    {
        var player = Target(Vector2.zero);
        var movement = player.gameObject.AddComponent<Player>();
        movement.rb = player.GetComponent<Rigidbody2D>();
        var shooting = player.gameObject.AddComponent<SimplePlayerShooting>();
        // Allow Start to wire the components just like a scene load.
        yield return null;
        player.GetComponent<CharacterHealth>().TakeDamage(1000, Vector2.right);
        Assert.IsFalse(movement.enabled);
        Assert.IsFalse(shooting.enabled);
        Assert.AreEqual(Vector2.zero, movement.rb.velocity);
    }
}
