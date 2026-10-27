using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneCombatTests
{
    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");
    }

    [UnityTearDown]
    public IEnumerator UnloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.CreateScene("EmptyTestScene"));
        yield return SceneManager.UnloadSceneAsync(scene);
    }

    [UnityTest]
    public IEnumerator SceneContainsConfiguredPlayerAndThreeEnemies()
    {
        var player = Object.FindObjectOfType<Player>();
        Assert.IsNotNull(player.GetComponent<PlayerHealth>());
        Assert.IsNotNull(player.GetComponent<Collider2D>());
        Assert.AreEqual(3, Object.FindObjectsOfType<EnemyController>().Length);
        yield return new WaitForSeconds(0.1f);
        // 第一只距玩家约 6.05，仍在半径 6 之外；另外两只已能发现玩家。
        Assert.AreEqual(EnemyController.EnemyState.Patrol,
            GameObject.Find("Enemy_Boar_1").GetComponent<EnemyController>().CurrentState);
        Assert.AreEqual(EnemyController.EnemyState.Chase,
            GameObject.Find("Enemy_Boar_2").GetComponent<EnemyController>().CurrentState);
        Assert.AreEqual(EnemyController.EnemyState.Chase,
            GameObject.Find("Enemy_Boar_3").GetComponent<EnemyController>().CurrentState);
        foreach (var enemy in Object.FindObjectsOfType<EnemyController>())
        {
            Assert.IsNotNull(enemy.GetComponent<Animator>().runtimeAnimatorController);
        }
    }

    [UnityTest]
    public IEnumerator ExistingWeaponCanDamageActualBoarPrefab()
    {
        var player = Object.FindObjectOfType<Player>();
        player.GetComponent<SimplePlayerShooting>().enabled = false;
        var enemy = GameObject.Find("Enemy_Boar_2").GetComponent<EnemyController>();
        // 保留真实 Animator、碰撞体、预制体参数，只固定目标位置方便瞄准。
        foreach (var other in Object.FindObjectsOfType<EnemyController>())
            other.enabled = false;
        enemy.transform.position = new Vector2(5, 0);
        Physics2D.SyncTransforms();
        var weapon = player.GetComponent<SimpleWeaponManager>();
        weapon.AimAt(enemy.GetComponent<Collider2D>().bounds.center);
        Assert.IsTrue(weapon.TryFire());
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(80, enemy.GetComponent<CharacterHealth>().CurrentHealth);
    }

    [UnityTest]
    public IEnumerator ActualBoarCanDieDuringHurtAnimation()
    {
        var enemy = GameObject.Find("Enemy_Boar_2").GetComponent<EnemyController>();
        var health = enemy.GetComponent<CharacterHealth>();
        health.TakeDamage(20, Vector2.right);
        Assert.AreEqual(EnemyController.EnemyState.Hurt, enemy.CurrentState);
        yield return new WaitForSeconds(0.05f);
        health.TakeDamage(1000, Vector2.right, true);
        Assert.IsTrue(enemy.GetComponent<Animator>().GetBool("dead"));
        yield return new WaitForSeconds(0.65f);
        Assert.IsTrue(enemy == null);
    }

    [UnityTest]
    public IEnumerator EnemiesStopChasingDeadPlayer()
    {
        var player = Object.FindObjectOfType<PlayerHealth>();
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<CharacterHealth>().TakeDamage(1000, Vector2.right);
        yield return new WaitForSeconds(0.1f);
        foreach (var enemy in Object.FindObjectsOfType<EnemyController>())
            Assert.AreEqual(EnemyController.EnemyState.Patrol, enemy.CurrentState);
    }
}
