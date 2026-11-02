using UnityEngine;

public class EnemyHurtState : EnemyStateBase
{
    private float hurtUntil;

    public EnemyHurtState(EnemyController enemy) : base(enemy) { }

    // 每次成功受伤都更新，包括尚未恢复时再次受伤的情况。
    public void ApplyHit(Vector2 hitDirection)
    {
        hurtUntil = Time.time + enemy.HurtDuration;
        enemy.Body.velocity = hitDirection * enemy.KnockbackSpeed;
        enemy.TriggerAnimation("hurt");
    }

    public override void LogicUpdate()
    {
        if (Time.time < hurtUntil) return;

        enemy.SwitchState(enemy.FoundPlayer || enemy.RemembersPlayer
            ? EnemyController.EnemyState.Chase
            : EnemyController.EnemyState.Patrol);
    }

    // 不执行普通移动，让受伤时设置的击退速度生效。
}
