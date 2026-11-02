using UnityEngine;

public class EnemyDeadState : EnemyStateBase
{
    public EnemyDeadState(EnemyController enemy) : base(enemy) { }

    public override void OnEnter()
    {
        enemy.Body.velocity = Vector2.zero;
        foreach (var hitbox in enemy.GetComponentsInChildren<Collider2D>())
            hitbox.enabled = false;
        enemy.SetAnimationBool("dead", true);
        // 即使没有动画，或者动画事件丢失，也保证能销毁尸体。
        Object.Destroy(enemy.gameObject, enemy.DeathDelay);
    }

    // 死亡后不再执行普通逻辑或移动。
}
